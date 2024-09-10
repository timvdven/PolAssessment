FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY PolAssessment/Common.Lib/Common.Lib.csproj Common.Lib/
COPY PolAssessment/AnprEnricher.App/AnprEnricher.App.csproj AnprEnricher.App/

RUN dotnet restore Common.Lib/Common.Lib.csproj
RUN dotnet restore AnprEnricher.App/AnprEnricher.App.csproj

COPY PolAssessment/Common.Lib/ Common.Lib/
COPY PolAssessment/AnprEnricher.App/ AnprEnricher.App/

RUN dotnet build AnprEnricher.App/AnprEnricher.App.csproj -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/build .

ENV HotFolder__TgzPath="HotFolders/HotFolderTgz"
ENV HotFolder__DataPath="HotFolders/HotFolderData"

ENTRYPOINT ["dotnet", "PolAssessment.AnprEnricher.App.dll"]

# docker build  -f Dockerfile.AnprEnricher.App -t anpr-enricher-app-image .
