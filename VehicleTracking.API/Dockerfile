FROM microsoft/dotnet:2.2-aspnetcore-runtime AS Base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build

# Restore dotnet before build to allow for caching
WORKDIR /
COPY VehicleTracking.Application/VehicleTracking.Application.csproj /src/VehicleTracking.Application/
COPY VehicleTracking.Common/VehicleTracking.Common.csproj /src/VehicleTracking.Common/
COPY VehicleTracking.Domain/VehicleTracking.Domain.csproj /src/VehicleTracking.Domain/
COPY VehicleTracking.Infrastructure/VehicleTracking.Infrastructure.csproj /src/VehicleTracking.Infrastructure/
COPY VehicleTracking.Persistence/VehicleTracking.Persistence.csproj /src/VehicleTracking.Persistence/
COPY VehicleTracking.API/VehicleTracking.API.csproj /src/VehicleTracking.API/

RUN dotnet restore /src/VehicleTracking.API/VehicleTracking.API.csproj

# Copy source files and build
COPY . /src

RUN dotnet build /src/VehicleTracking.API/VehicleTracking.API.csproj --no-restore -c Release
RUN dotnet publish /src/VehicleTracking.API/VehicleTracking.API.csproj --no-restore -c Release -o /app

# Copy compiled app to runtime container
FROM base AS final
COPY --from=build /app .
CMD ["dotnet", "VehicleTracking.API.dll"]