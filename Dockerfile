# Imagen base para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Imagen para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar la solución y el archivo de proyecto
COPY SMJRegisterAPI.sln ./
COPY SMJRegisterAPI/SMJRegisterAPI.csproj SMJRegisterAPI/

# Restaurar dependencias
RUN dotnet restore SMJRegisterAPI.sln

# Copiar el resto del código
COPY . .

# Compilar
WORKDIR /src/SMJRegisterAPI
RUN dotnet build -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Imagen final para producción
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
