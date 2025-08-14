# Producción: Build multi-stage (suponiendo Dockerfile en la misma carpeta que SMJRegisterAPI.sln)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar todo el contexto (SMJRegisterAPI.sln y carpetas de proyecto deben estar en el contexto)
COPY . .

# Restaurar solución (correrá sobre los .csproj que estén en el contexto)
RUN dotnet restore "SMJRegisterAPI.sln"

# Build del proyecto principal (ajusta la ruta si quieres compilar un proyecto concreto)
WORKDIR /src/SMJRegisterAPI
RUN dotnet build "SMJRegisterAPI.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src/SMJRegisterAPI
RUN dotnet publish "SMJRegisterAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:$PORT DOTNET_RUNNING_IN_CONTAINER=true
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
