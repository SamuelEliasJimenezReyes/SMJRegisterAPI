FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia el archivo de solución
COPY ["SMJRegisterAPI.sln", "."]

# Copia el proyecto (nota: el proyecto está en un subdirectorio)
COPY ["SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/SMJRegisterAPI/"]

# Restaura dependencias
RUN dotnet restore "SMJRegisterAPI.sln"

# Copia todo
COPY . .

# Publica
RUN dotnet publish "SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj" -c Release -o /app/publish

# Runtime final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]