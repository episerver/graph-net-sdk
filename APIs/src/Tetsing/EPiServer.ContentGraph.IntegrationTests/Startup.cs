using EPiServer.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EPiServer.ContentGraph.IntegrationTests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureContentApiOptions(o =>
            {
                o.IncludeInternalContentRoots = true;
                o.IncludeSiteHosts = true;
                //o.EnablePreviewFeatures = true;// optional
            });
            services.AddContentDeliveryApi(); // required, for further configurations, see https://docs.developers.optimizely.com/content-cloud/v1.5.0-content-delivery-api/docs/configuration
            services.AddContentGraph();
        }
    }
}