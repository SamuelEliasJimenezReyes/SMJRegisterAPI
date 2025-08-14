FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar la solución si existe
COPY *.sln ./

# Copiar todos los proyectos exactamente como en el .sln
COPY SMJRegisterAPI/*.csproj SMJRegisterAPI/
# Si tienes más proyectos, añadirlos aquí con la misma lógica

# Restaurar dependencias (usando sln o csproj)
RUN if [ -f *.sln ]; then \
        dotnet restore *.sln; \
    else \
        proj=$(find . -name "*.csproj" | head -n 1); \
        dotnet restore "$proj"; \
    fi

# Copiar el resto del código
COPY . .

# Construir
RUN proj=$(find . -name "*.csproj" | head -n 1) && \
    dotnet build "$proj" -c Release -o /app/build

FROM build AS publish
RUN proj=$(find . -name "*.csproj" | head -n 1) && \
    dotnet publish "$proj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]
