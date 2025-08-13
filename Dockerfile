# Add this at the very top of your Dockerfile to ensure clean builds
# syntax=docker/dockerfile:1.4

# Build stage - with retry for network issues
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file
COPY ["SMJRegisterAPI.sln", "./"]

# Copy project file
COPY ["SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/"]

# Restore with retry
RUN --network=host dotnet restore "SMJRegisterAPI/SMJRegisterAPI.csproj" --disable-parallel

# Copy remaining files
COPY ["SMJRegisterAPI/", "SMJRegisterAPI/"]

# Build and publish
WORKDIR "/src/SMJRegisterAPI"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]