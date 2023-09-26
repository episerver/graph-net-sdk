using System;
using System.Collections.Generic;
using EPiServer.Find.Api;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Find.Api.Querying.Queries;
using EPiServer.Find.Helpers;
using EPiServer.Find.Json;
using EPiServer.Find.UnifiedSearch;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EPiServer.Find.ClientConventions
{
    public class DefaultConventions : IClientConventions
    {
        private IClient client;

        public DefaultConventions() : this(null)
        {

        }

        public DefaultConventions(IClient client)
        {
            this.client = client;
            NestedConventions = new NestedConventions(client);
            ContractResolver = new JsonContractResolver();
            ContractResolver.ObjectContractInterceptors.Add(new TypeHierarchyInterceptor());
            ContractResolver.ObjectContractInterceptors.Add(new IncludeTypeNameInGeoLocationFieldNamesInterceptor());
            ContractResolver.ObjectContractInterceptors.Add(new IncludeTypeNameInAttachmentFieldNamesInterceptor());
            ContractResolver.ObjectContractInterceptors.Add(new IncludeTypeNameInNestedFieldNamesInterceptor(NestedConventions));
            ContractResolver.IgnoreSerializableAttribute = true;
            ContractResolver.IgnoreSerializableInterface = true;
            TypeNameConvention = new DefaultTypeNameConvention();
            var includeTypeInNativeFieldNamesInterceptor = new IncludeTypeInNativeFieldNamesInterceptor();
            ContractResolver.ObjectContractInterceptors.Add(includeTypeInNativeFieldNamesInterceptor);
            ContractResolver.ContractInterceptors.Add(includeTypeInNativeFieldNamesInterceptor);
            UnifiedSearchRegistry = new UnifiedSearchRegistry();
            ContractResolver.ObjectContractInterceptors.Add(new UnifiedSearchFieldsInterceptor(UnifiedSearchRegistry));
            TypeValidationConventions = new TypeValidationConventions();
        }
        public NestedConventions NestedConventions { get; set; }
        public ITypeValidationConventions TypeValidationConventions { get; set; }
        IIdConvention idConvention;
        public virtual IIdConvention IdConvention
        {
            get
            {
                if (idConvention.IsNull())
                {
                    idConvention = new DefaultIdConvention();
                }

                return idConvention;
            }
            set
            {
                value.ValidateNotNullArgument("value");
                idConvention = value;
            }
        }

        ITimeToLiveConvention timeToLiveConvention;
        public virtual ITimeToLiveConvention TimeToLiveConvention
        {
            get
            {
                if (timeToLiveConvention.IsNull())
                {
                    timeToLiveConvention = new DefaultTimeToLiveConvention();
                }

                return timeToLiveConvention;
            }
            set
            {
                value.ValidateNotNullArgument("value");
                timeToLiveConvention = value;
            }
        }

        ILanguageRoutingConvention languageRoutingConvention;
        public virtual ILanguageRoutingConvention LanguageRoutingConvention
        {
            get
            {
                if (languageRoutingConvention.IsNull())
                {
                    languageRoutingConvention = new DefaultLanguageRoutingConvention();
                }
                return languageRoutingConvention;
            }
            set
            {
                value.ValidateNotNullArgument("value");
                languageRoutingConvention = value;
            }
        }

        ITypeNameConvention typeNameConvention;
        public ITypeNameConvention TypeNameConvention
        {
            get
            {
                return typeNameConvention;
            }

            set
            {
                if (value.IsNull())
                {
                    throw new ArgumentNullException();
                }

                typeNameConvention = value;
            }
        }

        public JsonContractResolver ContractResolver { get; protected set; }

        public virtual Action<JsonSerializer> CustomizeSerializer { get; set; }

        public Action<IList<IProjectionProcessor>> CustomizeProjectionHandling { get; set; }

        FieldNameConvention fieldNameConvention;
        public virtual FieldNameConvention FieldNameConvention
        {
            get
            {
                if (fieldNameConvention.IsNull())
                {
                    fieldNameConvention = new TypeSuffixFieldNameConvention(NestedConventions);
                }

                return fieldNameConvention;
            }
            set
            {
                value.ValidateNotNullArgument("value");
                fieldNameConvention = value;
            }
        }

        Action<ITypeFilterable, IEnumerable<Type>> searchTypeFilter;
        public virtual Action<ITypeFilterable, IEnumerable<Type>> SearchTypeFilter
        {
            get
            {
                if (searchTypeFilter.IsNull())
                {
                    searchTypeFilter = ApplyTypeHierarchyFilter;
                }
                return searchTypeFilter;
            }
            set
            {
                value.ValidateNotNullArgument("value");
                searchTypeFilter = value;
            }
        }

        public IUnifiedSearchRegistry UnifiedSearchRegistry { get; set; }

        static void ApplyTypeHierarchyFilter(ITypeFilterable command, IEnumerable<Type> searchedTypes)
        {
            searchedTypes.ValidateNotNullArgument("searchedTypes");

            Filter typeFilter = null;
            foreach (var searchedType in searchedTypes)
            {
                var filter = new TermFilter(TypeHierarchyInterceptor.TypeHierarchyJsonPropertyName, searchedType.FullName);
                if (typeFilter == null)
                {
                    typeFilter = filter;
                }
                else
                {
                    typeFilter = typeFilter | filter;
                }
            }
            var existingQuery = command.Query;
            IQuery newQuery;
            if (existingQuery.IsNotNull())
            {
                newQuery = new FilteredQuery(existingQuery, typeFilter);
            }
            else
            {
                newQuery = new ConstantScoreQuery(typeFilter);
            }
            command.Query = newQuery;
        }

        AutoBoostConvention typeSpecificConvention;
        public virtual AutoBoostConvention AutoBoostConvention
        {
            get
            {
                return typeSpecificConvention ?? (typeSpecificConvention = new AutoBoostConvention());
            }

            set
            {
                value.ValidateNotNullArgument("value");
                typeSpecificConvention = value;
            }
        }
    }

    public class IncludeTypeNameInGeoLocationFieldNamesInterceptor : IInterceptObjectContract
    {
        public JsonObjectContract ModifyContract(JsonObjectContract contract)
        {
            foreach (var property in contract.Properties)
            {
                if (typeof(GeoLocation).IsAssignableFrom(property.PropertyType))
                {
                    property.NullValueHandling = NullValueHandling.Ignore; 
                    property.PropertyName += TypeSuffix.GeoLocation;
                }
            }
            return contract;
        }
    }

    public class IncludeTypeNameInAttachmentFieldNamesInterceptor : IInterceptObjectContract
    {
        public JsonObjectContract ModifyContract(JsonObjectContract contract)
        {
            foreach (var property in contract.Properties)
            {
                if (typeof(Attachment).IsAssignableFrom(property.PropertyType))
                {
                    property.NullValueHandling = NullValueHandling.Ignore;
                    property.PropertyName += TypeSuffix.Attachment;
                }
            }
            return contract;
        }
    }

    public class IncludeTypeNameInNestedFieldNamesInterceptor : IInterceptObjectContract
    {
        private NestedConventions nestedConventions;

        public IncludeTypeNameInNestedFieldNamesInterceptor(NestedConventions nestedConventions)
        {
            this.nestedConventions = nestedConventions;
        }

        public JsonObjectContract ModifyContract(JsonObjectContract contract)
        {
            foreach (var property in contract.Properties)
            {
                if (nestedConventions.IsNotNull() && nestedConventions.Contains(property))
                {
                    property.NullValueHandling = NullValueHandling.Ignore;
                    property.PropertyName += TypeSuffix.Nested;
                    continue;
                }
            }
            return contract;
        }
    }
}
