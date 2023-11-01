using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell;

namespace AlloyTemplates.Models.Blocks
{
    /// <summary>
    /// Used to insert editorial content edited using a rich-text editor
    /// </summary>
    [SiteContentType(
        GUID = "6AA617A4-2175-4360-975E-75EDF2B92411",
        GroupName = SystemTabNames.Content)]
    [SiteImageUrl]
    public class NestedBlock : SiteBlockData
    {
        [Display(GroupName = SystemTabNames.Content)]
        [CultureSpecific]
        public virtual XhtmlString MainBody { get; set; }

        [Display(GroupName = SystemTabNames.Content)]
        public virtual ContentArea ContentAreaItems { get; set; }
    }

    [UIDescriptorRegistration]
    public class NestedBlockUIDescriptor : UIDescriptor<NestedBlock>
    {
        public NestedBlockUIDescriptor()
            : base("editorial-block-icon")
        {
        }
    }
}
