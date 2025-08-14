
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
EXPOSE 8080
COPY . /source

WORKDIR /source/SMJRegisterAPI

ARG TARGETARCH


RUN dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app


WORKDIR /app

COPY --from=build /app .

USER $APP_UID

ENTRYPOINT ["dotnet", "SMJRegisterAPI.dll"]