using System;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Find.Api.Ids;
using EPiServer.Find.Helpers;
using EPiServer.Find.Json;
using EPiServer.Find.UnifiedSearch;
using Newtonsoft.Json.Serialization;
using EPiServer.Find.Api;
using EPiServer.Find.Api.Querying.Queries;

namespace EPiServer.Find.ClientConventions
{
    public class TypeConventionBuilder
    {
        public TypeConventionBuilder(IClientConventions clientConventions, Func<Type, bool> typeSelector)
        {
            clientConventions.ValidateNotNullArgument("clientConventions");
            typeSelector.ValidateNotNullArgument("typeSelector");

            ClientConventions = clientConventions;
            TypeSelector = typeSelector;
        }

        public Func<Type, bool> TypeSelector { get; private set; }

        protected IClientConventions ClientConventions { get; private set; }

        public TypeConventionBuilder UseJsonContract(Func<Type, JsonContract> contractMethod)
        {
            Func<Type, JsonContract> customContractMethod = type =>
                {
                    if(!TypeSelector(type))
                    {
                        return null;
                    }

                    return contractMethod(type);
                };
            ClientConventions.ContractResolver.CustomContractMethods.Add(customContractMethod);
            return this;
        }

        public TypeConventionBuilder ExcludeField(string name)
        {
            return ExcludeFieldMatching(property => property.PropertyName == name);
        }

        public TypeConventionBuilder ExcludeFieldMatching(Func<JsonProperty, bool> propertyCriteria)
        {
            ModifyContract(contract =>
            {
                var matchingProperties = contract.Properties.Where(propertyCriteria).ToList();
                foreach (var matchingProperty in matchingProperties)
                {
                    contract.Properties.Remove(matchingProperty);
                }

            });
            return this;
        }
        
        public TypeConventionBuilder CreateUsing(Func<object> creationMethod)
        {
            creationMethod.ValidateNotNullArgument("creationMethod");

            return ModifyContract(contract => contract.DefaultCreator = creationMethod);
        }

        public TypeConventionBuilder ModifyContract(Action<JsonObjectContract> modificationMethod)
        {
            modificationMethod.ValidateNotNullArgument("modificationMethod");

            var interceptor = new DelegateContractInterceptor(TypeSelector, modificationMethod);
            ClientConventions.ContractResolver.ObjectContractInterceptors.Add(interceptor);
            return this;
        }
    }

    public class TypeConventionBuilder<T> : TypeConventionBuilder where T : class
    {
        public TypeConventionBuilder(IClientConventions clientConventions, Func<Type, bool> typeSelector) : base(clientConventions, typeSelector)
        {
        }

        public TypeConventionBuilder<T> IncludeField<TProperty>(Expression<Func<T, TProperty>> fieldSelector)
        {
            return IncludeField(fieldSelector, null);
        }

        public TypeConventionBuilder<T> SetDecayParameters(TimeSpan scale, TimeSpan? offset = null, double? shape = null, double? minimum = null, DateTime? origin = null)
        {
            return SetDecayParameters(new DateDecayParameters(scale, offset, shape, minimum, origin));
        }

        public TypeConventionBuilder<T> SetDecayParameters(DateDecayParameters parameters)
        {
            ClientConventions.AutoBoostConvention.SetDecayParameters<T>(parameters);
            return this;
        }

        public TypeConventionBuilder<T> IncludeField<TProperty>(Expression<Func<T, TProperty>> fieldSelector, Action<T, TProperty> setter)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");

            
            var getter = fieldSelector.Compile();
            var jsonProperty = new JsonProperty();
            jsonProperty.ShouldSerialize = x => true;
            jsonProperty.Readable = true;
            jsonProperty.Writable = setter.IsNotNull();
            jsonProperty.PropertyType = typeof(TProperty);
            ModifyContract(contract =>
            {
                jsonProperty.PropertyName = ClientConventions.FieldNameConvention.GetFieldName(fieldSelector);
                if (!contract.Properties.Any(x => x.PropertyName == jsonProperty.PropertyName))
                {
                    jsonProperty.ValueProvider = new DelegateValueProvider<T, TProperty>(getter, setter);
                    contract.Properties.Add(jsonProperty);
                }
                else if (contract.Properties.Any(x => x.PropertyName == jsonProperty.PropertyName && x.Order == UnifiedSearchFieldsInterceptor.OverrideOrder)) 
                {
                    contract.Properties.Remove(contract.Properties.First(x => x.PropertyName == jsonProperty.PropertyName && x.Order == -1));

                    jsonProperty.ValueProvider = new DelegateValueProvider<T, TProperty>(getter, setter);
                    contract.Properties.Add(jsonProperty);
                }
            });
            
            return this;
        }

        public TypeConventionBuilder<T> ExcludeField(Expression<Func<T, object>> fieldSelector)
        {
            fieldSelector.ValidateNotNullArgument("fieldSelector");

            var fieldName = ClientConventions.FieldNameConvention.GetFieldName(fieldSelector);

            ExcludeField(fieldName);

            return this;
        }

        public FieldConventionBuilder FieldsMatching(Func<JsonProperty, bool> fieldSelector)
        {
            return new FieldConventionBuilder(this, fieldSelector);
        }

        public FieldConventionBuilder<TField> Field<TField>(Expression<Func<T, TField>> fieldSelector)
        {
            var fieldName = ClientConventions.FieldNameConvention.GetFieldName(fieldSelector);
            return new FieldConventionBuilder<TField>(this, x => x.PropertyName == fieldName);
        }

        public FieldConventionBuilder<TField> FieldsOfType<TField>()
        {
            return new FieldConventionBuilder<TField>(this, x => x.PropertyType == typeof(TField));
        }

        public TypeConventionBuilder<T> IdIs(Func<T, DocumentId> idMethod)
        {
            ClientConventions.IdConvention = new CompositeIdConvention(ClientConventions.IdConvention, x =>
                {
                    if (TypeSelector(x.GetType()))
                    {
                        return idMethod((T) x);
                    }

                    return null;
                });
            return this;
        }

        public TypeConventionBuilder<T> TimeToLiveIs(Func<T, TimeToLive> timeToLiveMethod)
        {
            ClientConventions.TimeToLiveConvention = new CompositeTimeToLiveConvention(ClientConventions.TimeToLiveConvention, x =>
            {
                if (TypeSelector(x.GetType()))
                {
                    return timeToLiveMethod((T)x);
                }

                return null;
            });
            return this;
        }

        public TypeConventionBuilder<T> LanguageRoutingIs(Func<T, LanguageRouting> languageRoutingMethod)
        {
            ClientConventions.LanguageRoutingConvention = new CompositeLanguageRoutingConvention(ClientConventions.LanguageRoutingConvention, x =>
            {
                if (TypeSelector(x.GetType()))
                {
                    return languageRoutingMethod((T)x);
                }

                return null;
            });
            return this;
        }
    }
}
