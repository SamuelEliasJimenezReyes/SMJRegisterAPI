FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine
WORKDIR /src
COPY . .
RUN dotnet publish SMJRegisterAPI/SMJRegisterAPI.csproj -c Release -o /app
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet","SMJRegisterAPI.dll"]
