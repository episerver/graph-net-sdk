using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using EPiServer.Shell.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace EPiServer.ContentGraph.IntegrationTests
{
    [InitializableModule]
    [ModuleDependency(typeof(ShellInitialization))]
    public class InitializationModule : IConfigurableModule
    {
        private const string ContentGraphCmsModuleName = "EPiServer.ContentGraph.IntegrationTests";

        public void ConfigureContainer(ServiceConfigurationContext context) =>
            context.Services.Configure<ProtectedModuleOptions>(options =>
            {
                if (!options.Items.Any(x => x.Name.Equals(ContentGraphCmsModuleName)))
                {
                    var module = new ModuleDetails
                    {
                        Name = ContentGraphCmsModuleName,
                    };
                    options.Items.Add(module);
                }
            });

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}
