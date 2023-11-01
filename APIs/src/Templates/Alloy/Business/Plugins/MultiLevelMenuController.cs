using System.Collections.Generic;
using EPiServer.Authorization;
using EPiServer.Shell.Navigation;
using EPiServer.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlloyMvcTemplates.Business.Plugins
{
    /*
     * An example of multilevel menu
     * Multilevel plugin
     *     - Overview
     *     - Create email
     *         - Resend email
     *     - Reports
     */
    [MenuProvider]
    public class CustomMultiLevelMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            const string ImportToolsMenuPath = MenuPaths.Global + "/cms/custommultilevel";

            // parent
            var emailsSection = new UrlMenuItem("Multilevel plugin", ImportToolsMenuPath, ResolveMenuUrl("Overview"))
            {
                IsAvailable = (_) => true
            };

            var overview = new UrlMenuItem("Overview", ImportToolsMenuPath + "/overview", ResolveMenuUrl("Overview"))
            {
                IsAvailable = (_) => true,
                SortIndex = 100
            };
            var create = new UrlMenuItem("Create email", ImportToolsMenuPath + "/create", ResolveMenuUrl("Create"))
            {
                IsAvailable = (_) => true,
                SortIndex = 200
            };
            var reports = new UrlMenuItem("Reports", ImportToolsMenuPath + "/reports", ResolveMenuUrl("Reports"))
            {
                IsAvailable = (_) => true,
                SortIndex = 300
            };
            var mailSent = new UrlMenuItem(string.Empty, ImportToolsMenuPath + "/create/mailsent", ResolveMenuUrl("ResendMessage"))
            {
                IsAvailable = (_) => false,
                SortIndex = 400
            };

            return new MenuItem[]
            {
                emailsSection,
                overview,
                create,
                reports,
                mailSent
            };
        }

        public static string ResolveMenuUrl(string item)
        {
            var moduleUrl = "/MultiLevelMenu";//Paths.ToResource(typeof(EmailsMenuProvider), "emails");
            return UriUtil.Combine(moduleUrl, item);
        }
    }


    [Authorize(Policy = CmsPolicyNames.CmsAdmin)]
    public class MultiLevelMenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Overview()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Reports()
        {
            return View();
        }
        public IActionResult ResendMessage()
        {
            return View();
        }
    }
}
