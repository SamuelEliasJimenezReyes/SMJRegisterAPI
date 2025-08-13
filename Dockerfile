FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia la solución
COPY ["SMJRegisterAPI.sln", "./"]

# Copia el proyecto
COPY ["SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/"]

# Restaura dependencias
RUN dotnet restore "SMJRegisterAPI.sln"

# Copia todo el código
COPY . .

# Publica
RUN dotnet publish "SMJRegisterAPI/SMJRegisterAPI.csproj" -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
