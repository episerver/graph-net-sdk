using EPiServer;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using Optimizely.ContentGraph.Cms.Core.ContentApiModelProperties.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation
{
        [ServiceConfiguration(typeof(IContentApiModelProperty), Lifecycle = ServiceInstanceScope.Singleton)]
        public class CustomContentTypeContentApiModelProperty : IContentApiModelProperty
        {
            private readonly IContentTypeRepository _contentTypeRepository;
            private readonly IContentLoader _contentLoader;

            public CustomContentTypeContentApiModelProperty()
                : this(ServiceLocator.Current.GetInstance<IContentTypeRepository>(),
                        ServiceLocator.Current.GetInstance<IContentLoader>())
            {
            }

            public CustomContentTypeContentApiModelProperty(
                IContentTypeRepository contentTypeRepository,
                IContentLoader contentLoader)
            {
                _contentLoader = contentLoader;
                _contentTypeRepository = contentTypeRepository;
            }

            public string Name => "ContentType";

            public object GetValue(ContentApiModel contentApiModel)
            {
                var contentType = GetContentType(contentApiModel);
                if (contentType == null)
                {
                    return contentApiModel.ContentType;
                }

                var abstractTypes = new List<Type>();
                AddBaseTypes(contentType.ModelType, ref abstractTypes);
                contentApiModel.ContentType.AddRange(abstractTypes.Select(x => x.Name));
                contentApiModel.ContentType.Add("Content");

                return contentApiModel.ContentType.Distinct().ToList();
            }

            private void AddBaseTypes(Type type, ref List<Type> types)
            {
                if (type?.BaseType != null && type.BaseType != type && type.BaseType != typeof(IContent) && type.BaseType != typeof(Object))
                {
                    types.Add(type.BaseType);
                    AddBaseTypes(type.BaseType, ref types);
                }
            }

            private ContentType GetContentType(ContentApiModel contentApiModel)
            {
                if (contentApiModel?.ContentLink?.Id is null or 0)
                {
                    return null;
                }

                var contentReference = new ContentReference(contentApiModel.ContentLink.Id.Value, contentApiModel.ContentLink?.WorkId.GetValueOrDefault() ?? 0, contentApiModel.ContentLink?.ProviderName);
                if (_contentLoader.TryGet<IContent>(contentReference, out var content))
                {
                    var contentType = _contentTypeRepository.Load(content.ContentTypeID);
                    if (contentType != null)
                    {
                        return contentType;
                    }
                }

                return null;
            }
        }
}
