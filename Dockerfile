FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine

WORKDIR /app
COPY . .

RUN dotnet build -c Release

EXPOSE 8080
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
