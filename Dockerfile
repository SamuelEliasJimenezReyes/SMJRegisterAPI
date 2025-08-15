FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Copiar todo el repositorio
COPY . /src

# Publicar desde la subcarpeta
RUN dotnet publish "/src/SMJRegisterAPI/SMJRegisterAPI.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY --from=build /app /app
WORKDIR /app
ENV ASPNETCORE_URLS=http://*:$PORT
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]