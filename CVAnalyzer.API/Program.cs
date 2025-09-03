using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var geminiApiKey = builder.Configuration["Gemini:ApiKey"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50_000_000; 
});

var app = builder.Build();

app.UseCors("AllowBlazor");


app.MapPost("/validate-key", async (HttpRequest request) =>
{
    using var reader = new StreamReader(request.Body);
    var apiKey = await reader.ReadToEndAsync();

    if (string.IsNullOrWhiteSpace(apiKey))
        return Results.BadRequest("API Key vacía");

    try
    {
        using var client = new HttpClient();
        var testPrompt = new
        {
            contents = new[] {
                new {
                    role = "user",
                    parts = new[] { new { text = "Hola, responde solo con 'OK' si me escuchas." } }
                }
            }
        };

        var json = JsonSerializer.Serialize(testPrompt);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}",
            content);

        if (response.IsSuccessStatusCode)
            return Results.Ok(new { valid = true, message = "API Key válida ✅" });

        return Results.Json(new { valid = false, message = $"Error: {response.StatusCode}" }, statusCode: 401);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error validando API Key: {ex.Message}");
    }
});



app.MapPost("/upload-cv", static async (HttpRequest request) =>
{
    try
    {
        if (!request.Form.Files.Any())
            return Results.BadRequest("No se ha enviado ningún archivo.");

        var file = request.Form.Files["file"];
        if (file == null)
            return Results.BadRequest("No se ha encontrado el archivo con el nombre 'file'.");

        if (file.ContentType != "application/pdf")
            return Results.BadRequest($"Archivo inválido: {file.ContentType}");

        var extractedText = await Task.Run(() =>
        {
            using var stream = file.OpenReadStream();
            using var reader = new PdfReader(stream);
            using var pdfDoc = new PdfDocument(reader);
            var text = new System.Text.StringBuilder();

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                var page = pdfDoc.GetPage(i);
                var strategy = new SimpleTextExtractionStrategy();
                text.AppendLine(PdfTextExtractor.GetTextFromPage(page, strategy));
            }

            return text.ToString();
        });

        return Results.Ok(new { content = extractedText });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error procesando el archivo: {ex.Message}");
    }
});



app.MapPost("/analyze-cv", async (HttpRequest request) =>
{
    using var reader = new StreamReader(request.Body);
    var body = await reader.ReadToEndAsync();

    if (string.IsNullOrWhiteSpace(body))
        return Results.BadRequest("El cuerpo de la petición está vacío.");

    var data = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
    if (data == null || !data.ContainsKey("text"))
        return Results.BadRequest("Falta 'text' en el cuerpo.");

    var cvText = data["text"];

    try
    {
        using var client = new HttpClient();

        var requestBody = new
        {
            contents = new[] {
                new {
                    role = "user",
                    parts = new[] {
                        new { text = $"Analiza este CV y responde en texto normal:\n\n1. Puntaje ATS (0-100)\n2. Palabras clave detectadas\n3. Recomendaciones de mejora\n\nCV:\n{cvText}" }
                    }
                }
            }
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

 
        var response = await client.PostAsync(
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={geminiApiKey}",
            jsonContent);

        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return Results.Problem($"Gemini API error ({(int)response.StatusCode}): {responseString}");

   
        using var doc = JsonDocument.Parse(responseString);
        var textResult = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        if (string.IsNullOrEmpty(textResult))
            return Results.Problem("No se obtuvo texto de Gemini.");

        return Results.Ok(new { analysis = textResult });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error analizando CV: {ex.Message}");
    }
});

app.Run();
