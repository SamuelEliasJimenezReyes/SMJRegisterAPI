# syntax=docker/dockerfile:1

# =========================
# Stage 1 - Build
# =========================
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

# Carpeta de trabajo para el código
WORKDIR /source

# Copiamos todo el código
COPY . .

# Detectar arquitectura
ARG TARGETARCH

# Publicar el proyecto
RUN dotnet publish $(find . -name "SMJRegisterAPI.csproj") \
    -a ${TARGETARCH/amd64/x64} \
    --use-current-runtime \
    --self-contained false \
    -o /app


# =========================
# Stage 2 - Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final

# Carpeta de trabajo
WORKDIR /app

# Copiar app publicada desde el stage build
COPY --from=build /app .

# Usar usuario no root (opcional)
USER $APP_UID

# Exponer puerto
EXPOSE 8080

# Entrypoint
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
