# A minimal chat application to showcase ASP.Net Core, SignalR & Azure services

## Versions
- Build with Visual Studio Code 1.27.2 on MacOS and Ubuntu 16.04
- ASP.NET Core 2.1
- Azure SignalR 1.0.0-*
- Azure Storage 9.3.2
- Docker-CE for ASP.NET Core - microsoft/dotnet:2.1-sdk image

## Design/Architectural Notes

- The IoC container framework showcased is Autofac, for the following reasons
    - Clear, up-to-date documentation (For example, compared to Unity)
    - Obvious support for ASP.NET Core with examples.
    - Integration with Service Fabric (if needed)
    - Looked at Ninject / Unity and the conclusions here :
    [Report](http://www.codinginstinct.com/2008/05/ioc-container-benchmark-rerevisted.html)
    - Autofac authored by Blumhardt (Seq, Serilog)

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

## Known Issues
- There is a client disconnect issue if the server is idle for a while, need to tinker with keepAlive settings etc. (or perhaps its something else - Azure SignalR is still not GA)
- The SF cluster is unable to instantiate a container - because of image OS discrepancies. Can be fixed.
- There seems to be an open issue provisioning containers onto an SF Linux cluster on Azure with SFCTL. Tracking this on Github [here](https://github.com/Microsoft/service-fabric-cli/issues/107)
