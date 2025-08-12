# Imagen base para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos el archivo .sln y el resto de los proyectos
COPY SMJRegisterAPI.sln ./
COPY SMJRegisterAPI/ ./SMJRegisterAPI/

# Restaurar dependencias
RUN dotnet restore SMJRegisterAPI/SMJRegisterAPI.csproj

# Compilar en modo Release
RUN dotnet publish SMJRegisterAPI/SMJRegisterAPI.csproj -c Release -o /app/publish

# Imagen base para correr la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
