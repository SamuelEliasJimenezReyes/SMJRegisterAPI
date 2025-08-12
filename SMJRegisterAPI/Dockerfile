# ----------------------------
# Etapa 1: Compilación
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copiamos primero los archivos de proyecto para aprovechar el caching de Docker
COPY ["SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/"]
COPY *.sln .

# Restauramos dependencias
RUN dotnet restore

# Copiamos el resto del código
COPY . .

# Publicamos en modo Release
WORKDIR "/src/SMJRegisterAPI"
RUN dotnet publish -c Release -o /app/publish

# ----------------------------
# Etapa 2: Runtime (ejecución)
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiamos solo lo necesario del build
COPY --from=build-env /app/publish .

# Exponemos los puertos
EXPOSE 80
EXPOSE 443

# Comando de inicio
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]