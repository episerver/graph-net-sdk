using System.ComponentModel.DataAnnotations;
using AlloyTemplates.Business.Rendering;
using AlloyTemplates.Models;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace AlloyMvcTemplates.Models.HeadlessContent
{
    [SiteContentType(GUID = "22F617A4-2175-4360-975E-75EDF2B924A7", GroupName = "Headless", DisplayName = "[Headless] Ingredient")]
    [SiteImageUrl]
    public class IngredientBlock : BlockData, IContainerPage, IHeadlessContent
    {
        [Display(GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        public virtual XhtmlString MainBody { get; set; }
    }
}
