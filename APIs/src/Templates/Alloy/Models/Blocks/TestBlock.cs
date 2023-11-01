using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AlloyTemplates.Models;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Web;

namespace AlloyMvcTemplates.Models.Blocks;

[SiteContentType(
    GUID = "55F617A4-2175-4360-975E-75ED12B924A7",
    GroupName = SystemTabNames.Content)]
[SiteImageUrl]
public class TestBlock : BlockData
{
    public virtual Url Url { get; set; }

    [UIHint(UIHint.Image)]
    public virtual Url UrlToImageCode { get; set; }

    [UIHint(UIHint.Video)]
    public virtual Url UrlToVideoCode { get; set; }

    [UIHint(UIHint.MediaFile)]
    public virtual Url UrlToMediaFileVideoCode { get; set; }

    public virtual IEnumerable<Url> ListUrl { get; set; }

    [UIHint(UIHint.Image)]
    public virtual IEnumerable<Url> ListUrlToImageCode { get; set; }

    [UIHint(UIHint.Video)]
    public virtual IEnumerable<Url> ListUrlToVideoCode { get; set; }

    [UIHint(UIHint.MediaFile)]
    public virtual IEnumerable<Url> ListUrlToMediaFileVideoCode { get; set; }
}
