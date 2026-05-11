FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

WORKDIR /src
RUN dotnet restore Rendez_Vous-microservice.csproj
RUN dotnet build Rendez_Vous-microservice.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Rendez_Vous-microservice.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Rendez_Vous-microservice.dll"]