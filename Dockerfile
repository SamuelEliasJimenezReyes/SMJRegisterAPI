FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia el archivo de solución
COPY ["SMJRegisterAPI.sln", "."]

# Copia el archivo .csproj (ajustado a tu estructura)
COPY ["SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/SMJRegisterAPI/"]

# Restaura dependencias
RUN dotnet restore "SMJRegisterAPI.sln"

# Copia todo
COPY . .

# Publica el proyecto (ruta corregida)
RUN dotnet publish "SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj" -c Release -o /app/publish

# Runtime final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]