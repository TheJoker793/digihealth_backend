FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Rendez_vous-microservice/Rendez_vous-microservice.csproj", "Rendez_vous-microservice/"]
RUN dotnet restore "./Rendez_vous-microservice/Rendez_vous-microservice.csproj"
COPY . .
WORKDIR "/src/Rendez_vous-microservice"
RUN dotnet build "./Rendez_vous-microservice.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Rendez_vous-microservice.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rendez_vous-microservice.dll"]
