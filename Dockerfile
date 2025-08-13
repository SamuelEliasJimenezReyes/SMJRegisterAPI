# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file first
COPY ["SMJRegisterAPI.sln", "."]

# Copy the main project file
COPY ["SMJRegisterAPI/SMJRegisterAPI/SMJRegisterAPI.csproj", "SMJRegisterAPI/"]

# Restore dependencies
RUN dotnet restore "SMJRegisterAPI/SMJRegisterAPI.csproj"

# Copy everything else
COPY . .

# Build the project
WORKDIR "/src/SMJRegisterAPI"
RUN dotnet build "SMJRegisterAPI.csproj" -c Release -o /app/build

# Publish the project
FROM build AS publish
RUN dotnet publish "SMJRegisterAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]