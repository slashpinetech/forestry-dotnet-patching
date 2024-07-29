[![MIT License](https://img.shields.io/github/license/slashpinetech/forestry-dotnet-patching?color=1F3B2B&style=flat-square)](https://github.com/slashpinetech/forestry-dotnet-lifecycle/blob/main/LICENSE)

# Forestry.NET -- Lifecycle

Forestry .NET is a set of open-source libraries for building modern web
applications using ASP.NET Core.

The patching package adds support for executing code based database updates, either once or on every application start.

## Usage

```c#
services.AddPatching(options => {
    options.ConnectionString = Configuration["Database:ConnectionString"];
})
    .AddBootstrapPatch<ApplicationBootstrapPatch>()
    .AddPatch<OneTimePatch>();
```

## TODO
- [ ] Clean up code for Open Source release.
- [ ] Update README as needed.
- [ ] Add tests.
- [ ] Create github actions for CI/CD.