# Running migrations

## Generate new migration
`dotnet ef migrations add <MigrationName> --project CovidTracker.Infrastructure --startup-project CovidTracker.Web`

## Update database
`dotnet ef database update --project CovidTracker.Infrastructure --startup-project CovidTracker.Web`