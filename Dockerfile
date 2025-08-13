# Fase de compilación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copiar archivos esenciales primero (para aprovechar caché de Docker)
COPY SMJRegisterAPI.sln .
COPY ./SMJRegisterAPI/SMJRegisterAPI.csproj ./SMJRegisterAPI/

# 2. Restaurar dependencias
RUN dotnet restore SMJRegisterAPI.sln

# 3. Copiar todo el código fuente
COPY ./SMJRegisterAPI ./SMJRegisterAPI

# 4. Publicar la aplicación
RUN dotnet publish ./SMJRegisterAPI/SMJRegisterAPI.csproj -c Release -o /app/publish

# Fase de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# 5. Copiar la aplicación publicada
COPY --from=build /app/publish .

# Configuración para Railway
ENV ASPNETCORE_URLS=http://+:$PORT \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE $PORT
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]