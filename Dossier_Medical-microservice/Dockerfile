FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# 👉 Port standard pour microservices .NET
EXPOSE 80

# 👉 Important : force l'écoute sur le bon port
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Dossier_Medical-microservice.csproj", "./"]
RUN dotnet restore

COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Dossier_Medical-microservice.dll"]