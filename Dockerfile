FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Usar minúsculas consistentemente en TODAS las rutas
COPY ["SMJRegisterAPI.sln", "."]
# Cambiado a minúsculas
COPY ["SMJRegisterAPI/smjregisterapi.csproj", "SMJRegisterAPI/"]  
COPY ["global.json", "."]

# Verificar archivo (usando minúsculas)
RUN ls -la SMJRegisterAPI/smjregisterapi.csproj

# Restaurar dependencias usando el proyecto
RUN dotnet restore "SMJRegisterAPI.sln"

# Copiar todo
COPY . .

# Construir y publicar usando minúsculas
WORKDIR "/src/SMJRegisterAPI"
RUN dotnet build "smjregisterapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "smjregisterapi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]