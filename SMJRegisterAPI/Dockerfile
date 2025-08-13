# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar la solución y el proyecto
COPY SMJRegisterAPI.sln ./
COPY SMJRegisterAPI/SMJRegisterAPI.csproj SMJRegisterAPI/

# Restaurar dependencias
RUN dotnet restore SMJRegisterAPI.sln

# Copiar todo el código
COPY . .

# Publicar la aplicación
WORKDIR /src/SMJRegisterAPI
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copiar publicación desde el build
COPY --from=build /app/publish .

# Exponer el puerto
EXPOSE 8080

ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
