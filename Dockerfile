FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia el archivo de solución
COPY SMJRegisterAPI.sln ./

# Copia el archivo .csproj (ruta correcta)
COPY SMJRegisterAPI/SMJRegisterAPI.csproj SMJRegisterAPI/

# Restaura dependencias
RUN dotnet restore SMJRegisterAPI.sln

# Copia todo el código
COPY . .

# Publica el proyecto
RUN dotnet publish SMJRegisterAPI/SMJRegisterAPI.csproj -c Release -o /app/publish

# Imagen final de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
