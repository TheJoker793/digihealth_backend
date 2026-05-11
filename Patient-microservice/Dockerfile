FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Patient-microservice.csproj ./
RUN dotnet restore Patient-microservice.csproj

COPY . .
RUN dotnet build Patient-microservice.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Patient-microservice.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Patient-microservice.dll"]