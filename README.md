# 📚 Comic Manager 

**Comic Manager** es una aplicación de consola que permite gestionar cómics y usuarios
Incluye funcionalidades como la gestión de usuarios, administración de cómics y asignación de cómics a usuarios


## ✨ Características principales

➟ **Menú principal y secundarios** para navegar entre funcionalidades 
➟ **Modelo de datos** compuesto de 3 clases y almenos 6 atributos (int, string, decimal, boolean, datetime) 
➟ **Gestión de usuarios** (Alta, búsqueda, eliminar, modificar)  
➟ **Gestión de cómics** (Añadir, listar, eliminar)  
➟ **Gestión de géneros** (Añadir, listar, eliminar) 
➟ **Zona privada** para cada usuario  
➟ **Zona pública** con información general  
➟ **Funcionalidad de búsqueda** por nombre en cada una de las clases


# 🛠️ Tecnologías utilizadas
💻 **C# y .NET**
🎨 **Spectre.Console** (Interfaz visual en consola)
📂 **JSON** para almacenamiento de datos
📝 **Logger** para centralización de logs
📝 **Serilog** para registro de errores
🐳 **Docker** para contenerización:
    - Configuración de puerto personalizado
    - Volumen para acceso a ficheros de la aplicación y logs  
    - Variables de entorno de los path de los volúmenes

## 🐋 Uso con Docker

La aplicación está disponible como imagen en Docker Hub:
```bash
docker pull ceemeese/comicmanager:1.0
docker run -it -v data:/app/data -v logs:/app/logs -p 7877:7877 --name comicmanager ceemeese/comicmanager:1.0


