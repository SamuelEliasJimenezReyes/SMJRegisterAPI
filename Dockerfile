FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar solución y archivos esenciales usando rutas relativas
COPY . .  
# Copia todo el contexto primero

# Mover los archivos esenciales a sus ubicaciones correctas
RUN mv SMJRegisterAPI.sln ./ && \
    mv global.json ./ && \
    mkdir -p SMJRegisterAPI && \
    mv SMJRegisterAPI/SMJRegisterAPI.csproj SMJRegisterAPI/

# Verificar que los archivos existen
RUN ls -la && \
    ls -la SMJRegisterAPI && \
    test -f SMJRegisterAPI/SMJRegisterAPI.csproj

# Restaurar dependencias
RUN dotnet restore "SMJRegisterAPI.sln"

# Continuar con el build
WORKDIR "/src/SMJRegisterAPI"
RUN dotnet build "SMJRegisterAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SMJRegisterAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]