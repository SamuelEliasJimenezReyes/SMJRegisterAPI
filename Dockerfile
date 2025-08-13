# Fase de compilación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar solución y proyecto usando rutas relativas correctas
COPY SMJRegisterAPI.sln .
COPY SMJRegisterAPI/SMJRegisterAPI.csproj ./SMJRegisterAPI/

# Restaurar dependencias
RUN dotnet restore ./SMJRegisterAPI/SMJRegisterAPI.csproj

# Copiar todo el código fuente
COPY . .

# Publicar
RUN dotnet publish ./SMJRegisterAPI/SMJRegisterAPI.csproj -c Release -o /app/publish

# Fase de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:$PORT \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE $PORT
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
