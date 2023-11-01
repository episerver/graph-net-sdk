using System.Collections.Generic;
using EPiServer.Authorization;
using EPiServer.Shell.Navigation;
using EPiServer.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlloyMvcTemplates.Business.Plugins
{
    [MenuProvider]
    public class StructuredMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            const string BaseMenuPath = MenuPaths.Global + "/cms/structuredmenu";

            // group & children
            // level 1
            var leftMenuItem = new UrlMenuItem("Structured Menu Plugin", BaseMenuPath, ResolveMenuUrl("Group"))
            {
                IsAvailable = (_) => true,
                SortIndex = 400
            };
            // level 2
            var groupMenuItem = new UrlMenuItem("Group Name", BaseMenuPath + "/group", ResolveMenuUrl("Group"))
            {
                IsAvailable = (_) => true,
                SortIndex = 410
            };
            // ... and children
            var item1 = new UrlMenuItem("Item One", BaseMenuPath + "/group/itemone", ResolveMenuUrl("ItemOne"))
            {
                IsAvailable = (_) => true,
                SortIndex = 420
            };
            var item2 = new UrlMenuItem("Item Two", BaseMenuPath + "/group/itemtwo", ResolveMenuUrl("ItemTwo"))
            {
                IsAvailable = (_) => true,
                SortIndex = 430
            };

            return new MenuItem[]
            {
                leftMenuItem,
                groupMenuItem,
                item1,
                item2
            };
        }

        public static string ResolveMenuUrl(string item)
        {
            var moduleUrl = "/StructuredMenu";
            return UriUtil.Combine(moduleUrl, item);
        }
    }


    [Authorize(Policy = CmsPolicyNames.CmsAdmin)]
    public class StructuredMenuController : Controller
    {
        public IActionResult Group()
        {
            return View("Index", new { Title = "Hello StructuredMenu > Group" });
        }

        public IActionResult ItemOne()
        {
            return View("Index", new { Title = "Hello StructuredMenu > ItemOne" });
        }

        public IActionResult ItemTwo()
        {
            return View("Index", new { Title = "Hello StructuredMenu > ItemTwo" });
        }
    }
}
