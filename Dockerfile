# Fase base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0.21 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Fase de build
FROM mcr.microsoft.com/dotnet/sdk:9.0.21 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar solución y proyecto
COPY SMJRegisterAPI.sln ./
COPY SMJRegisterAPI/SMJRegisterAPI.csproj ./SMJRegisterAPI/

# Restaurar dependencias
RUN dotnet restore ./SMJRegisterAPI/SMJRegisterAPI.csproj

# Copiar todo el código fuente
COPY . .

# Build del proyecto
RUN dotnet build ./SMJRegisterAPI/SMJRegisterAPI.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publicar el proyecto
FROM build AS publish
RUN dotnet publish ./SMJRegisterAPI/SMJRegisterAPI.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Fase final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:$PORT \
    DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
