# 📄 CVAnalyzer

**CVAnalyzer** es una aplicación web que analiza tu currículum en PDF utilizando **Gemini AI**, mostrando tu puntaje ATS, palabras clave detectadas y recomendaciones de mejora.  

👉 Desarrollado con **Blazor WebAssembly** en el frontend y **.NET 8 Minimal API** en el backend.

---
<img width="1366" height="683" alt="Principal" src="https://github.com/user-attachments/assets/ffa68055-fd20-4cd7-8a1e-21a2cd156c81" />
**Video demostrativo**


https://github.com/user-attachments/assets/38678872-91c0-418e-a809-68582cd1eac6



## ✨ Características principales
- ✅ **Subida de CV en PDF** con vista previa integrada  
- ✅ **Análisis ATS automático** (0-100)  
- ✅ **Detección de palabras clave** relevantes  
- ✅ **Recomendaciones de mejora** generadas por IA  
- ✅ **Interfaz fluida** con indicadores de carga  

---

## 🚀 Tecnologías utilizadas

### 🔹 Frontend
- [Blazor WebAssembly](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor)  
- C# / Razor Components  
- TailwindCSS  
- HTML + CSS

- <img width="1366" height="683" alt="cargaArchivo" src="https://github.com/user-attachments/assets/d59e297b-5c18-4451-bff0-126d9038619d" />
- <img width="1366" height="684" alt="ANaliza" src="https://github.com/user-attachments/assets/bc9e4880-4530-45eb-a008-a9fe8c1d986f" />
- <img width="1366" height="644" alt="REsultado 0" src="https://github.com/user-attachments/assets/9b3d7457-ed8d-4f29-8292-99b9071e7945" />




### 🔹 Backend
- .NET 8 Minimal API  
- [iText7](https://itextpdf.com/) → extracción de texto en PDFs  
- **Gemini API (Google AI)** → análisis inteligente

-    <img width="790" height="592" alt="UploadAPI" src="https://github.com/user-attachments/assets/2a580cce-a441-4c61-8efe-5709b141ce89" />
-    <img width="690" height="571" alt="AnalizeAPI" src="https://github.com/user-attachments/assets/326bb93a-26ff-42bd-b26f-551048e34b96" />
-  <img width="974" height="599" alt="API" src="https://github.com/user-attachments/assets/43944837-9be8-442d-b05b-58a32594e931" />





---

## ⚙️ Instalación y ejecución

### 1️⃣ Clonar el repositorio
```bash
git clone https://github.com/tuusuario/CVAnalyzer.git
cd CVAnalyzer
2️⃣ Backend
Ir al proyecto API:

bash
Copiar código
cd CVAnalyzer.Api
Configurar tu Gemini API Key en appsettings.json:

json
Copiar código
{
  "Gemini": {
    "ApiKey": "TU_API_KEY_AQUI"
  }
}

Ejecutar el backend:

bash
Copiar código
dotnet run
📍 Disponible en: http://localhost:5219 (puerto puede variar)

3️⃣ Frontend
Ir al proyecto cliente:

bash
Copiar código
cd CVAnalyzer.Client
Ejecutar el frontend:

bash
Copiar código
dotnet run
Abrir en el navegador:
👉 http://localhost:5104

🖼️ Capturas de pantalla
📌 Pantalla principal
📌 CV cargado
📌 Análisis generado por IA



📂 Estructura del proyecto
bash
Copiar código
CVAnalyzer/
│
├── CVAnalyzer.Client/     # Blazor WebAssembly (Frontend)
│   ├── Pages/UploadCV.razor
│   ├── wwwroot/
│   └── Program.cs
│
├── CVAnalyzer.Api/        # Minimal API (Backend)
│   ├── Program.cs
│   └── appsettings.json
│
└── README.md
🛠️ Roadmap / Próximas mejoras
💾 Guardado de CVs analizados en base de datos

📊 Dashboard con historial de análisis

📤 Exportación de resultados en PDF/Word

🔗 Integración con LinkedIn / portales de empleo

👨‍💻 Autor
Desarrollado por Dilan de Jesús Lucio Sustaita

🔗 LinkedIn: https://www.linkedin.com/in/dilanlucio/
