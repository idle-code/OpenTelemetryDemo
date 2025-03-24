FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Install primary dependencies and clean up cache
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
      curl \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS publish
WORKDIR /src
COPY . .
RUN dotnet restore "./WebAPI.csproj"
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final

COPY --from=publish --chown="$APP_UID":"$APP_UID" /app/publish .
RUN chown "$APP_UID":"$APP_UID" /app

USER $APP_UID
ENTRYPOINT ["dotnet", "WebAPI.dll"]
