FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY . /src
RUN dotnet publish "/src/SMJRegisterAPI/SMJRegisterAPI.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY --from=build /app /app
WORKDIR /app
ENV ASPNETCORE_URLS=http://*:$PORT
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]