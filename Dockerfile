# syntax=docker/dockerfile:1

########################
# Stage 1: build (SDK)
########################
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copiamos todo (respeta .dockerignore)
COPY . .

# Mostrar estructura para diagnóstico (verás esto en los logs de Railway)
RUN echo "=== root files (/src) ===" && ls -la /src || true
RUN echo "=== SMJRegisterAPI folder (/src/SMJRegisterAPI) ===" && ls -la /src/SMJRegisterAPI || true
RUN echo "=== Find .sln (maxdepth 2) ===" && find . -maxdepth 2 -type f -name "*.sln" || true
RUN echo "=== Find .csproj (top 20) ===" && find . -type f -name "*.csproj" | head -n 20 || true

# Intentar usar la solución si existe; si no, usar el primer .csproj encontrado.
# También usamos `dotnet sln ... list` para mostrar qué proyectos apunta la solución.
RUN set -eux; \
    if [ -f "./SMJRegisterAPI.sln" ]; then \
      echo "Found solution: SMJRegisterAPI.sln - listing projects:"; \
      dotnet sln SMJRegisterAPI.sln list || true; \
      # intentar publicar el primer proyecto listado por dotnet sln list
      proj=$(dotnet sln SMJRegisterAPI.sln list | sed -n '2p' || true); \
      if [ -n "$proj" ]; then \
        echo "Publishing project from sln: $proj"; \
        dotnet restore "$proj"; \
        dotnet publish "$proj" -c Release -o /app --no-restore; \
      else \
        echo "No projects listed in solution (or unable to parse). Will try to publish first .csproj found."; \
        csproj=$(find . -type f -name \"*.csproj\" | head -n 1); \
        if [ -z \"$csproj\" ]; then echo \"ERROR: no .csproj found in build context\"; exit 1; fi; \
        dotnet restore \"$csproj\"; \
        dotnet publish \"$csproj\" -c Release -o /app --no-restore; \
      fi; \
    else \
      csproj=$(find . -type f -name \"*.csproj\" | head -n 1); \
      if [ -z \"$csproj\" ]; then echo \"ERROR: no .csproj found in build context\"; exit 1; fi; \
      echo \"Publishing standalone project: $csproj\"; \
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
