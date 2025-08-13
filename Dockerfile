# Add retry logic for dotnet restore
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SMJRegisterAPI.sln", "."]
COPY ["SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/"]
RUN for i in {1..3}; do dotnet restore "SMJRegisterAPI/SMJRegisterAPI.csproj" --disable-parallel && break || sleep 5; done
COPY ["SMJRegisterAPI/", "SMJRegisterAPI/"]
WORKDIR "/src/SMJRegisterAPI"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]