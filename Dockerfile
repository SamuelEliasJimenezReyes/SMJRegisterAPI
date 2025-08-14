# Etapa base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar todo el código al contenedor
COPY . .

# Buscar y restaurar usando .sln si existe, de lo contrario usar .csproj
RUN if [ -f *.sln ]; then \
        echo "Restaurando usando solución..."; \
        dotnet restore *.sln; \
    else \
        echo "No se encontró .sln, buscando .csproj..."; \
        proj=$(find . -name "*.csproj" | head -n 1); \
        echo "Restaurando proyecto $proj"; \
        dotnet restore "$proj"; \
    fi

# Compilar usando el .csproj encontrado
RUN proj=$(find . -name "*.csproj" | head -n 1) && \
    echo "Compilando $proj" && \
    dotnet build "$proj" -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN proj=$(find . -name "*.csproj" | head -n 1) && \
    echo "Publicando $proj" && \
    dotnet publish "$proj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final (runtime)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080 DOTNET_RUNNING_IN_CONTAINER=true
ENTRYPOINT ["dotnet", "$(find . -name '*.dll' | head -n 1)"]
