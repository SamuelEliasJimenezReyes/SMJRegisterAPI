

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["/SMJRegisterAPI.csproj", "SMJRegisterAPI/"]
RUN dotnet restore "SMJRegisterAPI/SMJRegisterAPI.csproj"
COPY . .
WORKDIR "/src/SMJRegisterAPI"
RUN dotnet build "./SMJRegisterAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SMJRegisterAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["./SMJRegisterAPI"]
