# ----------------------------
# Etapa 1: Compilación
# ----------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /src

# Copiamos todo el código
COPY . .

# Restauramos dependencias (forzamos para evitar cache vieja)
RUN dotnet restore --force

# Publicamos en modo Release sin restaurar de nuevo
WORKDIR /src/SMJRegisterAPI
RUN dotnet publish -c Release -o /app/out --no-restore

# ----------------------------
# Etapa 2: Runtime (ejecución)
# ----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copiamos solo lo necesario del build
COPY --from=build-env /app/out .

# Exponemos los puertos (80 para HTTP y 443 para HTTPS si lo usas)
EXPOSE 80
EXPOSE 443

# Comando de inicio
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
