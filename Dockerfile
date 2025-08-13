# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file from root
COPY ["SMJRegisterAPI.sln", "."]

# Copy project file from SMJRegisterAPI subdirectory
COPY ["SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/"]

# Restore dependencies
RUN dotnet restore "SMJRegisterAPI/SMJRegisterAPI.csproj"

# Copy entire project directory
COPY ["SMJRegisterAPI", "SMJRegisterAPI"]

# Build and publish
WORKDIR "/src/SMJRegisterAPI"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]