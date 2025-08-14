# === Stage: runtime base ===
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# === Stage: build (SDK) ===
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos todo el contexto (el Dockerfile se queda en la carpeta que usas como contexto)
COPY . .

# Buscamos automáticamente el primer .csproj dentro del contexto (soporta anidamientos)
# y lo usamos para restore/build/publish.
# - maxdepth 6 por seguridad (ajusta si tienes más niveles)
RUN set -eux; \
    csproj=$(find . -maxdepth 6 -type f -name '*.csproj' -print -quit); \
    if [ -z "$csproj" ]; then echo "ERROR: no .csproj found in context"; exit 1; fi; \
    echo "Using project: $csproj"; \
    dotnet restore "$csproj"; \
    dotnet build "$csproj" -c Release -o /app/build

# Publicar
FROM build AS publish
WORKDIR /src
RUN set -eux; \
    csproj=$(find . -maxdepth 6 -type f -name '*.csproj' -print -quit); \
    dotnet publish "$csproj" -c Release -o /app/publish /p:UseAppHost=false

# === Stage: final runtime image ===
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:$PORT \
    DOTNET_RUNNING_IN_CONTAINER=true
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
