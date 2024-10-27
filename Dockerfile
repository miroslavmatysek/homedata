﻿FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release

COPY "src/" "src/"
RUN dotnet restore "HomeData.Worker/HomeData.Worker.csproj"
WORKDIR "/src/HomeData.Worker"
RUN dotnet publish "HomeData.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HomeData.Worker.dll"]