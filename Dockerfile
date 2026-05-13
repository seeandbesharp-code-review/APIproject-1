# =============================================================
# Stage 1 — BUILD
# Restores all projects in the solution and publishes Release
# =============================================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy ALL .csproj files first — needed because solution restore
# walks every project including TestProject1 (which references Servers/Repositories/Entities)
COPY WebAPIShop.sln                           ./
COPY WebAPIShop/WebAPIShop.csproj             WebAPIShop/
COPY Servers/Services.csproj                  Servers/
COPY Repositories/Repositories.csproj         Repositories/
COPY Entities/Entities.csproj                 Entities/
COPY DTOs/DTOs.csproj                         DTOs/
COPY TestProject1/TestProject1.csproj         TestProject1/

# Restore once — cached unless a .csproj changes
RUN dotnet restore WebAPIShop.sln

# Copy all source code
COPY . .

# Publish only the web project in Release mode
RUN dotnet publish WebAPIShop/WebAPIShop.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# =============================================================
# Stage 2 — RUNTIME
# Lean ASP.NET runtime image — no SDK, no test projects
# =============================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Create a non-root user for security
RUN addgroup --system appgroup \
    && adduser --system --ingroup appgroup appuser

# Create the logs directory and give the app user ownership
# NLog will write here instead of the hardcoded Windows path
RUN mkdir -p /app/logs && chown appuser:appgroup /app/logs

USER appuser

# Copy published output from build stage
COPY --from=build /app/publish .

# Copy the container-safe NLog config, overwriting the one published from source
COPY --chown=appuser:appgroup WebAPIShop/nlog.docker.config ./nlog.config

EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
# Tell NLog to write to /app/logs inside the container
ENV NLOG_LOG_DIRECTORY=/app/logs

ENTRYPOINT ["dotnet", "WebAPIShop.dll"]
