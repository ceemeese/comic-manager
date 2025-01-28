FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ComicManager.csproj .
RUN dotnet restore

COPY . .

RUN dotnet publish -c release -o /app

ARG APP_NAME
ARG DATA_PATH

RUN echo "Path: $DATA_PATH, AppName: $APP_NAME" 

ENV DATA_PATH=/app/data
ENV APP_NAME=ComicManagerApp


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .


ENTRYPOINT ["dotnet", "ComicManager.dll"]
