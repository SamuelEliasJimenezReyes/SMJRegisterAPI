# syntax=docker/dockerfile:1

########################
# Stage 1: build (SDK)
########################
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copiamos todo (respeta .dockerignore)
COPY . .

# Diagnóstico: verás esto en los logs de Railway
RUN echo "=== root files (/src) ===" && ls -la /src || true
RUN echo "=== SMJRegisterAPI folder (/src/SMJRegisterAPI) ===" && ls -la /src/SMJRegisterAPI || true
RUN echo "=== Find .sln (maxdepth 2) ===" && find . -maxdepth 2 -type f -name \"*.sln\" || true
RUN echo "=== Find .csproj (top 20) ===" && find . -type f -name \"*.csproj\" | head -n 20 || true

# Extracción robusta del .csproj referenciado por la solución (si existe).
# Convertimos backslashes a slashes y quitamos CRs para que dotnet funcione en Linux.
RUN set -eux; \
    if [ -f "./SMJRegisterAPI.sln" ]; then \
      echo "Found solution: SMJRegisterAPI.sln - trying to parse referenced project path"; \
      proj=$(sed -n 's/.*\"\(.*\.csproj\)\".*/\1/p' SMJRegisterAPI.sln | head -n 1 || true); \
      # Normalizar: backslashes -> slashes, eliminar CR si existe, trim espacios
      proj=$(echo "$proj" | sed 's|\\|/|g' | tr -d '\r' | awk '{$1=$1;print}'); \
      if [ -n \"$proj\" ]; then \
        echo "Parsed project from sln (normalized): $proj"; \
        dotnet restore \"$proj\"; \
        dotnet publish \"$proj\" -c Release -o /app --no-restore; \
      else \
        echo "Could not parse a csproj from the .sln file, falling back to first .csproj found in repo"; \
        csproj=$(find . -type f -name \"*.csproj\" | head -n 1); \
        if [ -z \"$csproj\" ]; then echo \"ERROR: no .csproj found in build context\"; exit 1; fi; \
        echo \"Publishing fallback project: $csproj\"; \
        dotnet restore \"$csproj\"; \
        dotnet publish \"$csproj\" -c Release -o /app --no-restore; \
      fi; \
    else \
      echo \"No solution file found, publishing first csproj found\"; \
      csproj=$(find . -type f -name \"*.csproj\" | head -n 1); \
      if [ -z \"$csproj\" ]; then echo \"ERROR: no .csproj found in build context\"; exit 1; fi; \
      dotnet restore \"$csproj\"; \
      dotnet publish \"$csproj\" -c Release -o /app --no-restore; \
    fi

########################
# Stage 2: runtime
########################
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app

COPY --from=build /app ./

ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
EXPOSE 8080

ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
