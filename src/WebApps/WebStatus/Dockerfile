FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebApps/WebStatus/WebStatus.csproj", "WebApps/WebStatus/"]
RUN dotnet restore "./WebApps/WebStatus/./WebStatus.csproj"
COPY . .
WORKDIR "/src/WebApps/WebStatus"
RUN dotnet build "./WebStatus.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebStatus.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/WebApps/WebStatus/style.css /app/style.css
ENTRYPOINT ["dotnet", "WebStatus.dll"]