# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy entire repository
COPY . .

# Restore and publish using relative paths
RUN dotnet restore "SMJRegisterAPI.sln"
RUN dotnet publish "SMJRegisterAPI/SMJRegisterAPI.csproj" -c Release -o /app/publish \
    --runtime linux-x64 \
    --self-contained false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]