using AlloyTemplates.Business;
using AlloyTemplates.Models.Pages;
using AlloyTemplates.Models.ViewModels;
using EPiServer.Web.Mvc;
using EPiServer.Shell.Security;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EPiServer.Web.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.Options;
using EPiServer.Shell.Security.Internal;

namespace AlloyTemplates.Controllers
{
    /// <summary>
    /// All controllers that renders pages should inherit from this class so that we can
    /// apply action filters, such as for output caching site wide, should we want to.
    /// </summary>
    public abstract class PageControllerBase<T> : PageController<T>, IModifyLayout
        where T : SitePageData
    {
        protected Injected<SecurityConfiguration> securityConfiguration;
        protected Injected<IHttpContextAccessor> contextAccessor;
        protected Injected<IOptions<AuthenticationOptions>> authenticationOptions;

        /// <summary>
        /// Signs out the current user and redirects to the Index action of the same controller.
        /// </summary>
        /// <remarks>
        /// There's a log out link in the footer which should redirect the user to the same page.
        /// As we don't have a specific user/account/login controller but rely on the login URL for
        /// forms authentication for login functionality we add an action for logging out to all
        /// controllers inheriting from this class.
        /// </remarks>
        public async Task<IActionResult> Logout()
        {
            await SignOutAsync();
            return Redirect(HttpContext.RequestServices.GetService<UrlResolver>().GetUrl(PageContext.ContentLink, PageContext.LanguageID));
        }

        public virtual void ModifyLayout(LayoutModel layoutModel)
        {
            var page = PageContext.Page as SitePageData;
            if (page != null)
            {
                layoutModel.HideHeader = page.HideSiteHeader;
                layoutModel.HideFooter = page.HideSiteFooter;
            }
        }

        private async Task SignOutAsync()
        {
            var uiSignInManager = securityConfiguration.Service?.UiSignInManager;
            if (uiSignInManager != null)
            {
                await uiSignInManager.SignOutAsync();
                return;
            }

            if (authenticationOptions.Service != null && contextAccessor.Service?.HttpContext != null)
            {
                foreach (var scheme in authenticationOptions.Service.Value.Schemes)
                {
                    await contextAccessor.Service.HttpContext.SignOutAsync(scheme.Name);
                }
            }
        }
    }
}
