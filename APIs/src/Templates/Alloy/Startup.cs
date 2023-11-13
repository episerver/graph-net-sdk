using AlloyMvcTemplates.Extensions;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Data;
using EPiServer.DependencyInjection;
using EPiServer.Framework.Web.Resources;
using EPiServer.Shell.Telemetry;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using EPiServer.Scheduler;
using EPiServer.Cms.TinyMce;
using EPiServer.Web.Mvc.Html;
using EPiServer.Cms.Shell;
using EPiServer.Cms.UI.Admin;
using EPiServer.Cms.UI.VisitorGroups;
using EPiServer.Cms.Shell.UI;
using EPiServer.Web.Mvc;
using EPiServer.Cms.Shell.UI.Rest.Projects;
using EPiServer.Cms.TinyMce.PropertySettings.Internal;
using EPiServer.ContentGraph.Extensions;
using Optimizely.ContentGraph.Cms.Configuration;
using AlloyMvcTemplates;
using EPiServer.ContentGraph.Api.Filters;

namespace EPiServer.Templates.Alloy.Mvc
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostingEnvironment;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment webHostingEnvironment, IConfiguration configuration)
        {
            _webHostingEnvironment = webHostingEnvironment;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var dbPath = Path.Combine(_webHostingEnvironment.ContentRootPath, "App_Data\\Alloy.mdf");
            var connectionstring = _configuration.GetConnectionString("EPiServerDB") ?? $"Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename={dbPath};Initial Catalog=alloy_mvc_netcore;Integrated Security=True;Encrypt=False;Connect Timeout=30;MultipleActiveResultSets=True";

            services.Configure<SchedulerOptions>(o =>
            {
                o.Enabled = false;
            });

            services.Configure<DataAccessOptions>(o =>
            {
                o.SetConnectionString(connectionstring);
            });

            services.Configure<TelemetryOptions>(o => {
                o.Enabled = false;
            });

            services.Configure<TinyMcePropertySettingsOptions>(o =>
            {
                o.Enabled = true;
            });

            services.AddCmsAspNetIdentity<ApplicationUser>();

            if (_webHostingEnvironment.IsDevelopment())
            {
                
                services.Configure<ClientResourceOptions>(uiOptions =>
                {
                    uiOptions.Debug = true;
                });

                services.Configure<ProjectUIOptions>(projectUIOptions =>
                {
                    // projectUIOptions.ProjectModeEnabled = false;
                });
            }

            
            services.AddMvc();
            services.AddAlloy();
            services.AddCmsHost()
                .AddCmsHtmlHelpers()
                .AddCmsUI()
                .AddAdmin()
                .AddVisitorGroupsUI()
                .AddTinyMce()
                .AddAdminUserRegistration(options => options.Behavior = RegisterAdminUserBehaviors.Enabled |
                                                                        RegisterAdminUserBehaviors.LocalRequestsOnly);
            services.AddScoped<IFilterForVisitor,FilterForModified>();

            services.ConfigureContentApiOptions(o =>
            {
                o.IncludeInternalContentRoots = true;
                o.IncludeSiteHosts = true;
                //o.EnablePreviewFeatures = true;// optional
            });
            services.AddContentDeliveryApi(); // required, for further configurations, see https://docs.developers.optimizely.com/content-cloud/v1.5.0-content-delivery-api/docs/configuration
            
            services.AddContentGraph();
            services.AddContentGraphCore(options=>
            {
                //options.UseHmacKey = false;
            });
            //the following is obsolete and is kept for compatibility for now
            //services.AddContentGraph(_configuration);
            services.AddCmsTagHelpers();
            services.AddEmbeddedLocalization<Startup>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapContent();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
