FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only solution file first
COPY ["SMJRegisterAPI.sln", "."]

# Copy project files using relative paths
COPY ["SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/"]

# Verify files exist before restore
RUN ls -la && ls -la SMJRegisterAPI/

# Restore dependencies
RUN dotnet restore "SMJRegisterAPI.sln"

# Copy remaining source code
COPY . .

# Build and publish
WORKDIR "/src/SMJRegisterAPI"
RUN dotnet build "SMJRegisterAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SMJRegisterAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]