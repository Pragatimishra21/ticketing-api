# Base image for .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY TicketingService_API/TicketingSystem/TicketingService_API.csproj TicketingService_API/TicketingSystem/
RUN dotnet restore TicketingService_API/TicketingSystem/TicketingService_API.csproj

# Copy everything else
COPY . .
WORKDIR /src/TicketingService_API/TicketingSystem

# Publish output
RUN dotnet publish TicketingService_API.csproj -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TicketingService_API.dll"]
