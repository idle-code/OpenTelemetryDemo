FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS publish
WORKDIR /src
COPY . .
RUN dotnet restore "./FunctionsWorker.csproj"
RUN dotnet publish "FunctionsWorker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final

COPY --from=publish --chown="$APP_UID":"$APP_UID" /app/publish .

RUN chown "$APP_UID":"$APP_UID" /app /azure-functions-host /app /run /etc/profile
RUN chmod 755 /azure-functions-host /app /run /etc/profile

USER $APP_UID

ENV AzureWebJobsScriptRoot=/app AzureFunctionsJobHost__Logging__Console__IsEnabled=true
