FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY SMJRegisterAPI.csproj SMJRegisterAPI/
RUN dotnet restore SMJRegisterAPI/SMJRegisterAPI.csproj

COPY . .
RUN dotnet publish SMJRegisterAPI/SMJRegisterAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
