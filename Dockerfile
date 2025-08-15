# Fase base - tiempo de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

# Fase build - compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos esenciales primero
COPY ["SMJRegisterAPI.sln", "."]
COPY ["SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/"]
COPY ["global.json", "."]

# Restaurar dependencias
RUN dotnet restore "SMJRegisterAPI.sln"

# Copiar todo el código
COPY . .

# Compilar proyecto
WORKDIR "/src/SMJRegisterAPI"
RUN dotnet build "SMJRegisterAPI.csproj" -c Release -o /app/build

# Fase publish - publicación
FROM build AS publish
RUN dotnet publish "SMJRegisterAPI.csproj" -c Release -o /app/publish \
    --runtime linux-x64 \
    --self-contained false \
    /p:UseAppHost=false

# Fase final - imagen de producción
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]