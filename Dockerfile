FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar solución
COPY ["SMJRegisterAPI.sln", "./"]

# Copiar recursivamente todos los .csproj manteniendo estructura
COPY . .
RUN find . -name "*.csproj" -exec sh -c '\
    dir="/src/$(dirname "{}")"; \
    mkdir -p "$dir"; \
    cp "{}" "$dir"; \
' \;

# Restaurar
RUN dotnet restore "SMJRegisterAPI.sln"


COPY . .
WORKDIR "/SMJRegisterAPI/SMJRegisterAPI"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]