# fa.clients.components
## Description
FrostAura client framework for Blazor server. This includes a components catelog.
## Status
## Status
| Project | Build | Nuget | Test Coverage
| --- | --- | --- | --- |
| FrostAura.Standard.Components | ![TravisCI](https://travis-ci.org/faGH/fa.standard.components.svg?branch=master) | [![NuGet](https://img.shields.io/nuget/v/FrostAura.Standard.Components.Razor.svg?style=for-the-badge)](https://www.nuget.org/packages/FrostAura.Standard.Components.Razor/)[![NuGet](https://img.shields.io/nuget/dt/FrostAura.Standard.Components.Razor.svg?style=for-the-badge)](https://www.nuget.org/packages/FrostAura.Standard.Components.Razor/) | PENDING |
## Database Migrations (EF Core)
### Overview
For migrations, we need to add them initially and update or re-add them each time the context changes. The actual execution of migrations happen on application start and is autonomous.
### Lessons Learnt
- In order to create migrations, the DB context has to either
    - Have a default constructor.
    - Have a IDesignTimeDbContextFactory implementation. - This allows for providing the args to the overloaded constructor during design-time as there is no DI available during that time.
    - Migration library has to be a core library and not a standard library.
    - Migration project has to reference 'Microsoft.EntityFrameworkCore.Tools'.
    - In order to make websockets operational via NGINX, certain proxy headers are required. See: https://docs.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/server?view=aspnetcore-3.1

### Re-create Migrations (Package-Manager Console)
> dotnet tool install --global dotnet-ef

> dotnet ef migrations add InitialDbMigration -c ApplicationDbContext -o Migrations/ApplicationDb --project FrostAura.Clients.Components.Data
## Docker Support
### Local
The project supports being run as a container and is in fact indended to. In order to run this service locally, simply run `docker-compose up` in the directory where the `docker-compose.yml` file resides. The service will now run on port 8083:HTTPS, 8082:HTTP.
### Docker Hub
Automated builds are set up for Docker Hub. To use this service without the source code running, use 
- `docker pull frostauraconsolidated/components` or 
- Visit https://hub.docker.com/repository/docker/frostauraconsolidated/components.

After the service is running, simply confirm that the service is running correctly by navigating to https://localhost:8083/

NOTE: For the container to work (HTTPS) by itself, we will require a valid SSL cert. You can simply create a dev self-signed cert using dotnet CLI, after which we can map it to the container. See below for local / development. For production you should have a valid cert. See https://docs.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-3.1 for more.

    Add a docker file share location for '%USERPROFILE%\.aspnet\https\'

    dotnet dev-certs https --clean
    dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p Password1234
    dotnet dev-certs https --trust
#### Docker Compose Example
    version: "3"
        services:
            db:
                image: "mcr.microsoft.com/mssql/server"
                environment:
                    - SA_PASSWORD=Password1234
                    - ACCEPT_EULA=Y
                ports:
                    - 1433:1433
            web:
                image: "frostauraconsolidated/components"
                ports:
                    - 8082:80
                    - 8083:443
                depends_on:
                    - db
                environment:
                    - ASPNETCORE_URLS=https://+:443;http://+:80
                    - ASPNETCORE_Kestrel__Certificates__Default__Password=Password1234
                    - ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
                volumes:
                    - ~/.aspnet/https:/https:ro

## How To
### Getting Familiar
To get context on how to consume the service, see the unit tests and integration tests for context on total usage.

## Contribute
In order to contribute, simply fork the repository, make changes and create a pull request.

## Support
For any queries, contact dean.martin@frostaura.net.
