# syntax=docker/dockerfile:1

# Build stage
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG TARGETARCH

WORKDIR /source

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY SMJRegisterAPI/*.csproj ./SMJRegisterAPI/

# FIX: Added project-specific prefix to cache ID
RUN --mount=type=cache,id=smjregisterapi-nuget,target=/root/.nuget/packages \
    dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /source/SMJRegisterAPI

# FIX: Same cache mount for publish step
RUN --mount=type=cache,id=smjregisterapi-nuget,target=/root/.nuget/packages \
    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app
EXPOSE 8080

# Copy from build stage
COPY --from=build /app .

USER $APP_UID
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]