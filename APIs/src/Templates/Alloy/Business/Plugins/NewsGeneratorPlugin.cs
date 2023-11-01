using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AlloyTemplates.Models.Blocks;
using AlloyTemplates.Models.Media;
using AlloyTemplates.Models.Pages;
using EPiServer;
using EPiServer.Authorization;
using EPiServer.Cms.Shell;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.DataAccess.Internal;
using EPiServer.Framework.Blobs;
using EPiServer.Notification.Internal;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Navigation;
using EPiServer.Shell.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlloyMvcTemplates.Business.Plugins
{
    [MenuProvider]
    public class NewGeneratorAdminMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            var urlMenuItem1 = new UrlMenuItem("Generate content", MenuPaths.Global + "/cms/admin/newsGenerator",
                "/NewsGeneratorPlugin/Index")
            {
                IsAvailable = context => true,
                SortIndex = 100,
            };

            return new List<MenuItem>(1)
            {
                urlMenuItem1
            };
        }
    }


    [Authorize(Policy = CmsPolicyNames.CmsAdmin)]
    public class NewsGeneratorPluginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Generate([FromForm] GenerateContentPostDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.root))
            {
                return new JsonDataResult("Root not set");
            }

            if (!ContentReference.TryParse(dto.root, out var rootId))
            {
                return new JsonDataResult("Cannot parse root page");
            }

            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var blobFactory = ServiceLocator.Current.GetInstance<IBlobFactory>();
            var contentAssetHelper = ServiceLocator.Current.GetInstance<ContentAssetHelper>();
            var notificationRepository = ServiceLocator.Current.GetInstance<INotificationRepository>();

            var alloyMeetFolder = contentRepository.GetChildren<ContentFolder>(ContentReference.SiteBlockFolder)
                .FirstOrDefault(x => x.Name == "Alloy Meet");
            var newsListBlock = contentRepository.GetChildren<PageListBlock>(alloyMeetFolder.ContentLink)
                .FirstOrDefault();

            var root = contentRepository.Get<IContent>(rootId);
            var isPage = root is PageData;

            if (dto.truncateParent)
            {
                contentRepository.DeleteChildren(rootId, true, AccessLevel.NoAccess);
            }

            for (var i = 0; i < dto.count; i++)
            {
                IContentData newContentItem;

                if (isPage)
                {
                    newContentItem = contentRepository.GetDefault<NewsPage>(rootId);
                    (newContentItem as IContent).Name = LoremIpsum(1, 2, 1, 1, 1);
                    (newContentItem as NewsPage).MetaDescription = LoremIpsum(1, 3, 1, 1, 1);
                    (newContentItem as NewsPage).NewsList = newsListBlock;
                }
                else
                {
                    newContentItem = contentRepository.GetDefault<TeaserBlock>(rootId);
                    (newContentItem as IContent).Name = LoremIpsum(1, 2, 1, 1, 1);
                    (newContentItem as TeaserBlock).Text = LoremIpsum(1, 3, 1, 1, 1);
                }

                var reference = contentRepository.Save(newContentItem as IContent,
                    SaveAction.Publish | SaveAction.SkipValidation);

                if (dto.createNotification)
                {
                    notificationRepository.SaveAsync(new InternalNotificationMessage
                    {
                        Sender = "abbie",
                        Recipient = "cmsadmin",
                        Subject = $"Generated {(newContentItem as IContent).Name}({reference})",
                        TypeName = "approve",
                        ChannelName = "Immediately",
                        Content = LoremIpsum(1, 3, 1, 1, 1),
                        Category = (newContentItem as IContent).GetUri()
                    }).ConfigureAwait(false).GetAwaiter().GetResult();
                }

                if (!dto.generateImages)
                {
                    continue;
                }

                byte[] data = null;

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("http://source.unsplash.com/random/277x155?nature,water&sig=" + i);
                    if (response.IsSuccessStatusCode)
                    {
                        data = await response.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                        return new JsonDataResult("Couldnt download image.");
                    }
                }

                var imageFile =
                    contentRepository.GetDefault<ImageFile>(contentAssetHelper.GetOrCreateAssetFolder(reference)
                        .ContentLink);
                imageFile.Name = "newsimage_" + i;

                var blob = blobFactory.CreateBlob(imageFile.BinaryDataContainer, ".jpg");
                using (var s = blob.OpenWrite())
                {
                    var w = new StreamWriter(s);
                    w.BaseStream.Write(data, 0, data.Length);
                    w.Flush();
                }

                imageFile.BinaryData = blob;
                var imageContentRef = contentRepository.Save(imageFile, SaveAction.Publish);

                if (isPage)
                {
                    (newContentItem as NewsPage).PageImage = imageContentRef;
                    var changeTrackable = (IChangeTrackable)newContentItem;
                    var random = new Random();
                    var dateTime = new DateTime(random.Next(2018, 2020), random.Next(1, 13), random.Next(1, 29));
                    changeTrackable.Created = dateTime;
                    changeTrackable.Changed = dateTime;
                    changeTrackable.Saved = dateTime;
                    changeTrackable.SetChangedOnPublish = true;
                    (newContentItem as NewsPage).StartPublish = dateTime;
                    EPiServer.Framework.ContextCache.Current[ContentSaveDB.UseIChangeTrackingSavedKey] =
                        new object();
                }
                else
                {
                    (newContentItem as TeaserBlock).Image = imageContentRef;
                }

                contentRepository.Save(newContentItem as IContent, SaveAction.Publish | SaveAction.SkipValidation);
            }

            return new JsonDataResult($"Successfully created {dto.count} children under {rootId}");
        }

        private static string LoremIpsum(int minWords, int maxWords,
            int minSentences, int maxSentences,
            int numParagraphs)
        {

            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
                "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
                "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            var rand = new Random();
            int numSentences = rand.Next(maxSentences - minSentences)
                               + minSentences + 1;
            int numWords = rand.Next(maxWords - minWords) + minWords + 1;

            StringBuilder result = new StringBuilder();

            for (var p = 0; p < numParagraphs; p++)
            {
                for (var s = 0; s < numSentences; s++)
                {
                    for (var w = 0; w < numWords; w++)
                    {
                        if (w > 0)
                        {
                            result.Append(' ');
                        }
                        result.Append(words[rand.Next(words.Length)]);
                    }
                    result.Append(". ");
                }
            }

            return result.ToString();
        }
    }

    public class GenerateContentPostDto
    {
        public string root { get; set; }
        public int count { get; set; }
        public bool truncateParent { get; set; }
        public bool generateImages { get; set; }

        public bool createNotification { get; set; }
    }
}
