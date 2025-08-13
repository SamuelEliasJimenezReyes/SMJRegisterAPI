FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Try these variations one at a time:
# Option 1 (most common structure):
COPY ["SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/"]
RUN dotnet restore "SMJRegisterAPI/SMJRegisterAPI.csproj"

# Option 2 (if your file is in a subfolder):
# COPY ["SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/SMJRegisterAPI/"]
# RUN dotnet restore "SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj"

COPY . .
WORKDIR "/src/SMJRegisterAPI"
RUN dotnet build "SMJRegisterAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SMJRegisterAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]