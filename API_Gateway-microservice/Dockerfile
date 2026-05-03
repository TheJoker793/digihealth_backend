# Phase de base - Linux
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Phase de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API_Gateway-microservice/API_Gateway-microservice.csproj", "API_Gateway-microservice/"]
RUN dotnet restore "./API_Gateway-microservice/API_Gateway-microservice.csproj"
COPY . .
WORKDIR "/src/API_Gateway-microservice"
RUN dotnet build "./API_Gateway-microservice.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Phase de publication
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API_Gateway-microservice.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Phase finale
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API_Gateway-microservice.dll"]