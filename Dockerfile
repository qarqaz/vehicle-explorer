FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/VehicleExplorer.Api/VehicleExplorer.Api.csproj", "src/VehicleExplorer.Api/"]
RUN dotnet restore "src/VehicleExplorer.Api/VehicleExplorer.Api.csproj"

COPY . .
WORKDIR "/src/src/VehicleExplorer.Api"
RUN dotnet publish "VehicleExplorer.Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "VehicleExplorer.Api.dll"]