# Imagen base para tiempo de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Imagen para compilación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiamos solo el csproj y restauramos dependencias
COPY SMJRegisterAPI/SMJRegisterAPI.csproj SMJRegisterAPI/
RUN dotnet restore "SMJRegisterAPI/SMJRegisterAPI.csproj"

# Copiamos el resto del código
COPY . .

# Compilamos
WORKDIR /src/SMJRegisterAPI
RUN dotnet build "SMJRegisterAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicamos
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SMJRegisterAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
