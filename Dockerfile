# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files first
COPY ["SMJRegisterAPI.sln", "."]
COPY ["SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/"]

# Restore dependencies
RUN dotnet restore "SMJRegisterAPI/SMJRegisterAPI.csproj"

# Copy remaining source code
COPY ["SMJRegisterAPI", "SMJRegisterAPI"]

# Build and publish
WORKDIR "/src/SMJRegisterAPI"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]