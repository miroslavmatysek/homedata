FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

COPY "src/" "src/"
WORKDIR "/src/HomeData.Worker"
RUN dotnet restore "HomeData.Worker.csproj"
RUN dotnet publish "HomeData.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HomeData.Worker.dll"]