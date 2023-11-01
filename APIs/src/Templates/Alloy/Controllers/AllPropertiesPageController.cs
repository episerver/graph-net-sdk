using AlloyTemplates.Models.Pages;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace AlloyMvcTemplates.Controllers;

public class AllPropertiesTestPageController : PageController<AllPropertiesTestPage>
{
    public IActionResult Index(AllPropertiesTestPage currentPage)
    {
        return View(currentPage);
    }
}
