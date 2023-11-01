using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.DataAbstraction;

namespace AlloyTemplates.Models.Blocks
{
    /// <summary>
    /// Used to insert editorial content edited using a rich-text editor
    /// </summary>
    [SiteContentType(
        GUID = "AAF617A4-2175-4360-975E-75EDF2B924A7",
        GroupName = SystemTabNames.Content, AvailableInEditMode = false)]
    [SiteImageUrl]
    public class ListBlock : BlockData
    {
        public virtual IList<string> Tags { get; set; }
    }
}
