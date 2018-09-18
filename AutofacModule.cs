using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NGTest.Storage;

namespace NGTest
{
    public class AutofacModule : Module 
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new StorageHelper( c.Resolve<ILogger<StorageHelper>>(), c.Resolve<IConfiguration>() ) ).AsSelf().SingleInstance();
        }
    }
}