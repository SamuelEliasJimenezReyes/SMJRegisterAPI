# syntax=docker/dockerfile:1

########################
# Stage 1: Build (SDK)
########################
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copiar el archivo del proyecto y restaurar
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código y publicar
COPY . .
RUN dotnet publish -c Release -o /app

########################
# Stage 2: Runtime
########################
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
EXPOSE 8080
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
