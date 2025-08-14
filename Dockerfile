# syntax=docker/dockerfile:1

########################
# Stage 1: build (SDK)
########################
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copiamos todo (si tu .dockerignore está correcto, no traerá bin/obj)
COPY . .

# Mostrar estructura para diagnóstico (verás esto en los logs de Railway)
RUN echo "=== root files ===" && ls -la /src && echo "=== find . -maxdepth 2 -type f -name \"*.sln\" ===" && find . -maxdepth 2 -type f -name "*.sln" || true \
 && echo "=== find . -type f -name \"*.csproj\" (top 10) ===" && find . -type f -name "*.csproj" | head -n 10 || true

# Detectar y publicar correctamente:
#  - Si existe un .sln en la raíz (SMJRegisterAPI.sln) lo usamos.
#  - Si no, tomamos el primer .csproj encontrado.
# Esto evita depender de rutas fijas que en Railway pueden variar.
RUN set -eux; \
    if [ -f "./SMJRegisterAPI.sln" ]; then \
      echo "Found solution: SMJRegisterAPI.sln - restoring & publishing solution"; \
      dotnet restore SMJRegisterAPI.sln; \
      dotnet publish SMJRegisterAPI.sln -c Release -o /app --no-restore; \
    else \
      csproj=$(find . -type f -name "*.csproj" | head -n 1); \
      if [ -z "$csproj" ]; then \
        echo "ERROR: no .csproj found in build context"; exit 1; \
      fi; \
      echo "Found csproj: $csproj - restoring & publishing project"; \
      dotnet restore "$csproj"; \
      dotnet publish "$csproj" -c Release -o /app --no-restore; \
    fi

########################
# Stage 2: runtime
########################
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app

# Copiamos los binarios publicados
COPY --from=build /app ./

# Exponer puerto y permitir que ASP.NET escuche en cualquier interfaz
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
EXPOSE 8080

# Entrypoint
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
