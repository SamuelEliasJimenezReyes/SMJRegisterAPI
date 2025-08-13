FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copiar primero solo el archivo de solución
COPY ["SMJRegisterAPI.sln", "./"]

# 2. Crear la estructura de directorios necesaria
RUN mkdir -p SMJRegisterAPI/SMJRegisterAPI

# 3. Copiar explícitamente el archivo .csproj
COPY ["SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/SMJRegisterAPI/"]

# 4. Restaurar dependencias
RUN dotnet restore "SMJRegisterAPI.sln"

# 5. Copiar el resto del código fuente
COPY . .

# 6. Compilar el proyecto
WORKDIR "/src/SMJRegisterAPI/SMJRegisterAPI"
RUN dotnet build "SMJRegisterAPI.csproj" -c Release -o /app/build

# 7. Publicar
FROM build AS publish
RUN dotnet publish "SMJRegisterAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 8. Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]