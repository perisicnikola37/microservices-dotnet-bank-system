FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiGateways/OcelotApiGateway/OcelotApiGateway.csproj", "ApiGateways/OcelotApiGateway/"]
RUN dotnet restore "./ApiGateways/OcelotApiGateway/./OcelotApiGateway.csproj"
COPY . .
WORKDIR "/src/ApiGateways/OcelotApiGateway"
RUN dotnet build "./OcelotApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OcelotApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

USER root

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

USER app

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OcelotApiGateway.dll"]