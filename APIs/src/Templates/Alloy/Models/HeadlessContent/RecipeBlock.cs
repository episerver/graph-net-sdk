using System.ComponentModel.DataAnnotations;
using AlloyTemplates.Business.Rendering;
using AlloyTemplates.Models;
using AlloyTemplates.Models.Blocks;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace AlloyMvcTemplates.Models.HeadlessContent
{
    [SiteContentType(GUID = "11F617A4-2175-4360-975E-75EDF2B924A7", GroupName = "Headless", DisplayName = "[Headless] Recipe")]
    [SiteImageUrl]
    [AvailableContentTypes(Availability.Specific, Include = new [] {typeof(IngredientBlock)})]
    public class RecipeBlock : BlockData, IContainerPage, IHeadlessContent
    {
        [Display(GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        public virtual XhtmlString MainBody { get; set; }
    }
}
