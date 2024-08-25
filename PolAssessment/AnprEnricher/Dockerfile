# Use official .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish "AnprEnricher.csproj" -c Release -o /app

# Use official .NET runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app .

# Set overruling environment variables
ENV HotFolderTgzPath="HotFolders/HotFolderTgz"
ENV HotFolderDataPath="HotFolders/HotFolderData"

# Start the application | not working on Windows hosts
ENTRYPOINT ["dotnet", "PolAssessment.AnprEnricher.dll"]
