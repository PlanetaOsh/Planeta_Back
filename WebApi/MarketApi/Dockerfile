FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "WebApi/MarketApi/MarketApi.csproj"
COPY . .
WORKDIR "/src/WebApi/MarketApi"
RUN dotnet build "./MarketApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./MarketApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarketApi.dll"]
