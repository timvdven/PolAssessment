#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Build the image with the following command
# docker build -t containerId .

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

# Install Filebrowser | Only necessary for Windows hosts
RUN apt-get update && apt-get install -y wget && \
    wget -O /tmp/filebrowser.tar.gz https://github.com/filebrowser/filebrowser/releases/download/v2.23.0/linux-amd64-filebrowser.tar.gz && \
    tar -xzf /tmp/filebrowser.tar.gz -C /usr/local/bin && \
    rm /tmp/filebrowser.tar.gz

# Start the application | not working on Windows hosts
#ENTRYPOINT ["dotnet", "PolAssessment.AnprEnricher.dll"]

# Run the application with bind mount | not working on Windows hosts
# docker run -v /path/on/your/host:/app/HotFolders containerId


# Start Filebrowser and the application | Only for Windows hosts
CMD ["sh", "-c", "filebrowser -r /app/HotFolders --port 8080 --address 0.0.0.0 & dotnet PolAssessment.AnprEnricher.dll"]

# Run the application with filebrowser | Only for Windows hosts
# docker run -d -p 8080:8080 containerId
