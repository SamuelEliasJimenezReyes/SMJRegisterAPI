# Fase de compilación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos esenciales primero (para cache de Docker)
COPY SMJRegisterAPI.sln .
COPY SMJRegisterAPI/SMJRegisterAPI.csproj SMJRegisterAPI/

# Restaurar dependencias
RUN dotnet restore "SMJRegisterAPI/SMJRegisterAPI.csproj"

# Copiar todo el código fuente
COPY . .

# Publicar la aplicación
RUN dotnet publish "SMJRegisterAPI/SMJRegisterAPI.csproj" -c Release -o /app/publish

# Fase de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copiar la aplicación publicada
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:$PORT \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE $PORT
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
