##DESARROLLO
#Imagen base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

#Directorio dentro de contenedor
WORKDIR /app

#Copia archivo
COPY ComicManager.csproj .

#Restaura dependencias
RUN dotnet restore

#Copia todos los archivos del directorio actual
COPY . .

#Compila aplicación en modo release y coloca arvhicos en esa ruta
RUN dotnet publish -c Release -o /app/out

#Variables de entorno
ARG DATA_PATH=/app/data
ARG APP_NAME=ComicManagerApp

##PRODUCCION
#Imagen que utiliza para ejecutar la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
##Directorio de trabajo
WORKDIR /app
#Copia ficheros del build a ese directorio
COPY --from=build /app/out .
#Declaración de variables de entorno
ENV DATA_PATH=/app/data
ENV APP_NAME=ComicManagerApp
#Ruta del volumen persistente
VOLUME ["/app/data"]
#Puerto donde escucha la app
EXPOSE 7877
#Comando para ejecutar la app
ENTRYPOINT ["dotnet", "ComicManager.dll"]
