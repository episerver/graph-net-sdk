using System.Collections.Generic;
using EPiServer.Authorization;
using EPiServer.Shell.Navigation;
using EPiServer.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlloyMvcTemplates.Business.Plugins
{
    /*
     * An example of client side menu
     */
    [MenuProvider]
    public class ClientSideMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            const string ImportToolsMenuPath = MenuPaths.Global + "/cms/clientsideapp";

            // parent
            var emailsSection = new UrlMenuItem("Client side", ImportToolsMenuPath, ResolveMenuUrl(""))
            {
                IsAvailable = (_) => true
            };

            var page1 = new UrlMenuItem("Page 1", ImportToolsMenuPath + "/page1", ResolveMenuUrl("page1"))
            {
                IsAvailable = (_) => true,
                SortIndex = 100
            };
            var page2 = new UrlMenuItem("Page 2", ImportToolsMenuPath + "/page2", ResolveMenuUrl("page2"))
            {
                IsAvailable = (_) => true,
                SortIndex = 200
            };
            var page3 = new UrlMenuItem("Page 3", ImportToolsMenuPath + "/page3", ResolveMenuUrl("page3"))
            {
                IsAvailable = (_) => true,
                SortIndex = 300
            };

            return new MenuItem[]
            {
                emailsSection,
                page1,
                page2,
                page3
            };
        }

        public static string ResolveMenuUrl(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                item = "#" + item;
            }
            var moduleUrl = "/ClientMenu";
            return UriUtil.Combine(moduleUrl, "Index" + item);
        }
    }


    [Authorize(Policy = CmsPolicyNames.CmsAdmin)]
    public class ClientMenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
