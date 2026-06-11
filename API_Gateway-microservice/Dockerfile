FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore API_Gateway-microservice/API_Gateway-microservice.csproj

RUN dotnet publish API_Gateway-microservice/API_Gateway-microservice.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

COPY API_Gateway-microservice/Ocelot.json .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "API_Gateway-microservice.dll"]