﻿using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;
 using AlloyTemplates.Business.Rendering;

 namespace AlloyTemplates.Models.Pages
{
    /// <summary>
    /// Used for the pages mainly consisting of manually created content such as text, images, and blocks
    /// </summary>
    [SiteContentType(GUID = "9CCC8A41-5C8C-4BE0-8E73-520FF3DE8267")]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-standard.png")]
    public class StandardPage : SitePageData
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
    }

    [SiteContentType(GUID = "3D629546-E59E-4DDC-81AF-BB2E982B38F1")]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-standard.png")]
    public class NumberEditorTest : PageData, IContainerPage
    {
        [Display(
            Name = "Map - Latitude",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 330)]
        [Range(-90, 90)]
        public virtual double Latitude { get; set; }
    }
}
