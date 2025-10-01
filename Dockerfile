# Base image for .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj and restore dependencies
COPY ticketing-api/TicketingService_API/TicketingSystem/TicketingSystem.csproj \
     TicketingService_API/TicketingSystem/
RUN dotnet restore TicketingService_API/TicketingSystem/TicketingSystem.csproj

# Copy all source files
COPY . .

WORKDIR /src/TicketingService_API/TicketingSystem

# Publish the project to /app/publish
RUN dotnet publish TicketingSystem.csproj -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Make Kestrel listen on Railway assigned port
ENV ASPNETCORE_URLS=http://+:$PORT

# Run the API
ENTRYPOINT ["dotnet", "TicketingSystem.dll"]
