# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copy solution file
COPY ["SMJRegisterAPI.sln", "."]

# 2. Create target directory structure
RUN mkdir -p SMJRegisterAPI/SMJRegisterAPI

# 3. Copy project file
COPY ["./SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj", "./SMJRegisterAPI/SMJRegisterAPI/"]

# 4. Restore dependencies
RUN dotnet restore "./SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj"

# 5. Copy remaining source code
COPY ["./SMJRegisterAPI/SMJRegisterAPI", "./SMJRegisterAPI/SMJRegisterAPI"]

# 6. Build and publish
WORKDIR "/src/SMJRegisterAPI/SMJRegisterAPI"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]