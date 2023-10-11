using EPiServer.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EPiServer.ContentGraph.IntegrationTests.TestSupport
{
    public class ApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public Action<IWebHostBuilder> HostConfig { get; init; }
        protected override void ConfigureWebHost(IWebHostBuilder builder) => HostConfig?.Invoke(builder);
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .UseEnvironment("Development")
                .ConfigureCmsDefaults()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<TStartup>());
        }
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            return base.CreateHost(builder);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
