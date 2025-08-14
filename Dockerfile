FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["SMJRegisterAPI.sln", "."]
# Usar MAYÚSCULAS
COPY ["SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/"]  
COPY ["global.json", "."]

# Verificar archivo (con mayúsculas)
RUN ls -la SMJRegisterAPI/SMJRegisterAPI.csproj

# Restaurar dependencias
RUN dotnet restore "SMJRegisterAPI.sln"

COPY . .

WORKDIR "/src/SMJRegisterAPI"
RUN dotnet build "SMJRegisterAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SMJRegisterAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]