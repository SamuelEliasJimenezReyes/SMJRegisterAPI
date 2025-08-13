FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar solo el csproj para optimizar cache
COPY SMJRegisterAPI.csproj ./

RUN dotnet restore SMJRegisterAPI.csproj

# Copiar todo el código fuente
COPY . .

RUN dotnet publish SMJRegisterAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
