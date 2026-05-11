FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY API_Gateway-microservice.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish API_Gateway-microservice.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Copier Ocelot.json seulement s'il existe (sans commande shell)
COPY Ocelot.json* ./

EXPOSE 80
ENTRYPOINT ["dotnet", "API_Gateway-microservice.dll"]