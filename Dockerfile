FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Copia todo y lista el contenido para diagnóstico
COPY . /src
RUN ls -la /src
RUN ls -la /src/SMJRegisterAPI

# Publica el proyecto
RUN dotnet publish "/src/SMJRegisterAPI/SMJRegisterAPI.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY --from=build /app /app
WORKDIR /app
ENV ASPNETCORE_URLS=http://*:$PORT
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]