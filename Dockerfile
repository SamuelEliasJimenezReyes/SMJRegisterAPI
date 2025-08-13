# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar todo el código fuente
COPY . .

# Encontrar y restaurar el proyecto automáticamente
RUN find . -name "*.csproj" -type f | head -1 | xargs dotnet restore

# Publicar en modo Release - buscar el archivo csproj automáticamente
RUN find . -name "SMJRegisterAPI.csproj" -type f | head -1 | xargs -I {} dotnet publish {} -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copiar la aplicación publicada desde la etapa anterior
COPY --from=build /app/publish .

# Configuración de puerto para Railway
ENV ASPNETCORE_URLS=http://+:$PORT \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE $PORT

ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]