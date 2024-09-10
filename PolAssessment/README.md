# .NET Solution of PolAssessment

[Main README](../README.md)

The .NET solution contains of:

- AnprDataProcessor.Test
  - NUnit test project
- [AnprDataProcessor.WebApi](./AnprDataProcessor.WebApi/README.md)
  - .NET 8 Web API
  - Responsible for:
    - Communication with the database
    - Processing valid data into the database
    - Exposing Web API (using Swagger)
    - Security:
        - Only authorized entities are allowed to upload data
        - Trace (log) uploads
- [AnprEnricher.App](./AnprEnricher.App/README.md)
  - .NET 8 Console App
  - Responsible for:
    - Unpacking tgz files
    - Reading unpacked JSON files to ANPR data
    - Enriching ANPR data (multiple enrichers)
    - Sending enriched ANPR data to Data Processor
- AnprEnricher.Test
  - NUnit test project
- AnprFrontEnd.Test
  - NUnit test project
- [AnprFrontEnd.WebApi](./AnprFrontEnd.WebApi/README.md)
  - .NET 8 Web API
  - Responsible for:
    - Communication with the database
    - Processing authorized requests using database
    - Exposing Web API (using Swagger)
    - Security:
        - Only authorized entities are allowed to use this API
- [Common.Lib](./Common.Lib/README.md)
  - .NET 8 Library
  - Responsible for:
    - Preventing code duplication
- Common.Test
  - NUnit test project

## Clean, Restore & Build
In order to clean, restore & build the .NET solution, invoke:
```sh
dotnet clean
dotnet restore
dotnet build
```

## Test
In order to run all test projects, invoke:
```sh
dotnet test
```

## Run

Please consult the prerequisites and requirements prior to trying to run any project.

From this level, you could either run each project separately, like:
```sh
dotnet AnprEnricher.App/bin/Debug/net8.0/PolAssessment.AnprEnricher.App.dll
dotnet AnprDataProcessor.WebApi/bin/Debug/net8.0/PolAssessment.AnprDataProcessor.WebApi.dll
dotnet AnprFrontEnd.WebApi/bin/Debug/net8.0/PolAssessment.AnprFrontEnd.WebApi.dll
```

or you could configure a webserver like IIS to host the two WebApis,
or you could run the whole solution plus the static files and database in an Docker Compose project, which is the recommended way to go.

### Docker
Simply run this script at the root:
```sh
./rebuild_docker_images.sh
```
