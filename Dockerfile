# syntax=docker/dockerfile:1

########################
# Stage 1: Build (SDK)
########################
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copiamos todo (respeta .dockerignore)
COPY . .

# Diagnóstico rápido (aparecerá en los logs; puedes borrarlo luego)
RUN echo "=== /src contents ===" && ls -la /src || true \
 && echo "=== /src/SMJRegisterAPI contents ===" && ls -la /src/SMJRegisterAPI || true

# Buscar el csproj dentro de la carpeta SMJRegisterAPI, normalizar la ruta y publicar
RUN set -eux; \
    # buscar .csproj en la carpeta del proyecto (ajusta la carpeta si tu proyecto está en otra ubicación)
    csproj=$(find ./SMJRegisterAPI -maxdepth 3 -type f -name "*.csproj" | head -n 1); \
    if [ -z "$csproj" ]; then \
      echo "ERROR: no .csproj found under ./SMJRegisterAPI"; \
      echo "Lista de .csproj en repo:"; find . -type f -name "*.csproj" || true; \
      exit 1; \
    fi; \
    # normalizar backslashes a slashes y quitar CRs si los hay
    csproj=$(echo "$csproj" | sed 's|\\|/|g' | tr -d '\r'); \
    echo "Publishing project: $csproj"; \
    dotnet restore "$csproj"; \
    dotnet publish "$csproj" -c Release -o /app --no-restore

########################
# Stage 2: Runtime
########################
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app

# Copiar los binarios publicados
COPY --from=build /app ./

# Activar escucha en el puerto que Railway inyecte (o 8080 por defecto)
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
EXPOSE 8080

# Entrypoint
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
