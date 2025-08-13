# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias (mejor para caching)
COPY SMJRegisterAPI/SMJRegisterAPI.csproj SMJRegisterAPI/
COPY SMJRegisterAPI.sln ./
RUN dotnet restore

# Copiar el resto del código
COPY . .

# Publicar en Release
WORKDIR /src/SMJRegisterAPI
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copiar publicación
COPY --from=build /app/publish .

# Variables recomendadas para Railway
ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 8080

ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
