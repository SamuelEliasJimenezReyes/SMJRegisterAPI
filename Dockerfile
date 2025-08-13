# Base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar solución y proyecto (rutas corregidas)
COPY SMJRegisterAPI.sln ./
COPY SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj ./SMJRegisterAPI/

# Restaurar dependencias
RUN dotnet restore ./SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj

# Copiar todo el código
COPY . .

# Build
RUN dotnet build ./SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish ./SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:$PORT \
    DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
