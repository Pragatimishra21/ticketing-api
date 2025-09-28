# Base image for .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the correct csproj and restore
COPY TicketingService_API/TicketingSystem/TicketingSystem.csproj TicketingService_API/TicketingSystem/
RUN dotnet restore TicketingService_API/TicketingSystem/TicketingSystem.csproj

# Copy all files
COPY . .
WORKDIR /src/TicketingService_API/TicketingSystem

# Publish output
RUN dotnet publish TicketingSystem.csproj -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TicketingSystem.dll"]
