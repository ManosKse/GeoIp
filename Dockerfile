# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /code

# Copy solution and all project files
COPY ["GeoIpProject.sln", "./"]
COPY ["GeoIpProject.csproj", "./"]
COPY ["code/GeoIpProject.Api.Controllers/GeoIpProject.Api.Controllers.csproj", "code/GeoIpProject.Api.Controllers/"]
COPY ["code/GeoIpProject.Api.Services/GeoIpProject.Api.Services.csproj", "code/GeoIpProject.Api.Services/"]
COPY ["code/GeoIpProject.Api.Services.Interfaces/GeoIpProject.Api.Services.Interfaces.csproj", "code/GeoIpProject.Api.Services.Interfaces/"]
COPY ["code/GeoIpProject.Clients/GeoIpProject.Clients.csproj", "code/GeoIpProject.Clients/"]
COPY ["code/GeoIpProject.Clients.Interfaces/GeoIpProject.Clients.Interfaces.csproj", "code/GeoIpProject.Clients.Interfaces/"]
COPY ["code/GeoIpProject.Services/GeoIpProject.Services.csproj", "code/GeoIpProject.Services/"]
COPY ["code/GeoIpProject.Services.Intefaces/GeoIpProject.Services.Intefaces.csproj", "code/GeoIpProject.Services.Intefaces/"]

# Restore all dependencies
RUN dotnet restore "GeoIpProject.sln"

# Copy all source files
COPY . .

# Build and publish
RUN dotnet build "GeoIpProject.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "GeoIpProject.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Expose ports
EXPOSE 5000

# Entry point
ENTRYPOINT ["dotnet", "GeoIpProject.dll"]
