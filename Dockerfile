# Usa etiquetas oficiales de Microsoft
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia primero el archivo de solución
COPY ["SMJRegisterAPI.sln", "."]
COPY ["SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/"]

# Restaura dependencias
RUN dotnet restore "SMJRegisterAPI.sln"

# Copia todo y publica
COPY . .
RUN dotnet publish "SMJRegisterAPI/SMJRegisterAPI.csproj" -c Release -o /app/publish

# Runtime final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]