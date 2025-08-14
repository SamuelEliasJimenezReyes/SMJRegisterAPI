# syntax=docker/dockerfile:1

# Build stage
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG TARGETARCH

WORKDIR /source

# Copy solution and project files first
COPY *.sln .
COPY SMJRegisterAPI/*.csproj ./SMJRegisterAPI/

# Restore with cache - using project-specific prefix
RUN --mount=type=cache,id=smjregisterapi-nuget,target=/root/.nuget/packages \
    dotnet restore

# Copy remaining source code
COPY . .
WORKDIR /source/SMJRegisterAPI

# Publish with cache
RUN --mount=type=cache,id=smjregisterapi-nuget,target=/root/.nuget/packages \
    dotnet publish -a ${TARGETARCH/amd64/x64} -c Release --use-current-runtime --self-contained false -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app .

USER $APP_UID
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]