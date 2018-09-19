# A minimal chat application to showcase ASP.Net Core, SignalR & Azure services

## Versions
- ASP.NET Core 2.1
- Azure SignalR 1.0.0-*
- Azure Storage 9.3.2
- Docker for ASP.NET Core - microsoft/dotnet:2.1-sdk image

## Design/Architectural Notes

- The IoC container framework showcased is Autofac, for the following reasons
    - Clear, up-to-date documentation (For example, compared to Unity)
    - Obvious support for ASP.NET Core with examples.
    - Integration with Service Fabric (if needed)

- Usage of strongly-type Hubs in SignalR
    - Clearer what interface the client supports
    - Easier maintenance/readability of code!

- Choice of React JS Framework for the client SPA
    - Compartmentalizing concerns
    - Nice flow of state/props from parent --> child makes understanding/maintenance of the software easier        

- Choice of Azure Table Storage for message persistance
    - Most straightforward demonstration of message persistance
    - Easy to test/debug message persistance!   

- Choice of container-based deployment to Service Fabric Cluster
    - Easier to package ASP.NET Core 2.0 + React SPA + Deps
    - The user secrets for development (e.g connection strings), are part of the image!
    
## Implementation Notes

- Scaffolding based on the ASP.NET Core 2.1 React template (created with dotnet new)
- Service Fabric service created with yeoman generators for container-based service