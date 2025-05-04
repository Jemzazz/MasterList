# Build stage (Updated to .NET 8.0)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build  # Changed from 6.0 to 8.0
WORKDIR /src
COPY . .
RUN dotnet publish "MasterList.csproj" -c Release -o /app/publish

# Runtime stage (Updated to .NET 8.0)
FROM mcr.microsoft.com/dotnet/aspnet:8.0  # Changed from 6.0 to 8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MasterList.dll"]
