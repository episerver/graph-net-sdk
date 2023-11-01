using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.SpecializedProperties;

namespace AlloyTemplates.Models.Pages
{
    /// <summary>
    /// Used for the pages mainly consisting of manually created content such as text, images, and blocks
    /// </summary>
    [SiteContentType(GUID = "AACC8A33-5C8C-4BE0-8E73-520FF3DE8267")]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-standard.png")]
    public class TagHelperPage : SitePageData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 310)]
        [CultureSpecific]
        public virtual XhtmlString MainBody { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 320)]
        public virtual ContentArea MainContentArea { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 330)]
        public virtual LinkItem Link { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 340)]
        public virtual Url Url { get; set; }
    }
}
