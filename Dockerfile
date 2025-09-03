##DESARROLLO
#Imagen base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

#Directorio dentro de contenedor
WORKDIR /src
#Copia archivo
COPY . ./
#Compila aplicación en modo release y coloca archivos en esa ruta
RUN dotnet publish -c Release -o /app



##PRODUCCION
#Imagen que utiliza para ejecutar la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
##Directorio de trabajo
WORKDIR /app
#Copia ficheros del build a ese directorio
COPY --from=build /app ./
#Copiar ficheros json del local para tener ya unos datos en el contenedor
COPY Data /app/data
# Instalar locales para mostrar euros
RUN apt-get update && apt-get install -y locales \
    && locale-gen es_ES.UTF-8
# Configuración de las variables de entorno de locales
ENV LANG=es_ES.UTF-8
ENV LANGUAGE=es_ES:es
ENV LC_ALL=es_ES.UTF-8
#Declaración de path para la lectura/escritura del json
ENV DATA_PATH=/app/data
ENV LOG_PATH=/app/logs
#Ruta del volumen persistente
VOLUME ["/app/data"]
VOLUME ["/app/logs"]
#Puerto donde escucha la app
EXPOSE 7877
#Comando para ejecutar la app
ENTRYPOINT ["dotnet", "ComicManager.dll"]
