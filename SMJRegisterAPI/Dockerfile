# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar la solución y el archivo de proyecto (.csproj)
COPY SMJRegisterAPI.sln ./
COPY SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj SMJRegisterAPI/SMJRegisterAPI/

# Restaurar dependencias
RUN dotnet restore SMJRegisterAPI.sln

# Copiar todo el código fuente
COPY . .

# Publicar en modo Release
WORKDIR /src/SMJRegisterAPI/SMJRegisterAPI
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copiar la aplicación publicada desde la etapa anterior
COPY --from=build /app/publish .

# Configuración de puerto para Railway
ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 8080

ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
