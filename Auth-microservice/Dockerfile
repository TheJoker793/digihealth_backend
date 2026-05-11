FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copier UNIQUEMENT le .csproj en premier
COPY Auth-microservice.csproj .

# 2. Restore sans --no-cache → Docker met cette couche en cache
#    Si .csproj n'a pas changé, cette étape est skippée au prochain build
RUN dotnet restore Auth-microservice.csproj

# 3. Copier tout le reste du code source
COPY . .

# 4. Build
RUN dotnet build Auth-microservice.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Auth-microservice.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth-microservice.dll"]