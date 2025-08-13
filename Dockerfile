# Simple single-stage build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar todo el código fuente
COPY . .

# Encontrar el archivo csproj y usarlo para restore y publish
RUN dotnet restore **/*.csproj
RUN dotnet publish **/*.csproj -c Release -o /out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .

# Configuración para Railway
ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT

ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]