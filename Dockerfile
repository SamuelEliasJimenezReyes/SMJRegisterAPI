# Use multi-stage build for smaller final image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copy solution file
COPY ["SMJRegisterAPI.sln", "."]

# 2. Copy project file
COPY ["SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/"]

# 3. Restore dependencies (without --network=host)
RUN dotnet restore "SMJRegisterAPI/SMJRegisterAPI.csproj"

# 4. Copy remaining source code
COPY ["SMJRegisterAPI/", "SMJRegisterAPI/"]

# 5. Build and publish
WORKDIR "/src/SMJRegisterAPI"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]