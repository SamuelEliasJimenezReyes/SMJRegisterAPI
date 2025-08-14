FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine

WORKDIR /src
COPY . .
RUN dotnet build SMJRegisterAPI/SMJRegisterAPI.csproj -c Release


EXPOSE 8080
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
