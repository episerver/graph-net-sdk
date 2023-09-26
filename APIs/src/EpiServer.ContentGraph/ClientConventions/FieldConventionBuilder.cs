using System;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace EPiServer.Find.ClientConventions
{
    public class FieldConventionBuilder
    {
        TypeConventionBuilder typeConventionBuilder;
        Func<JsonProperty, bool> propertyMatcher;

        public FieldConventionBuilder(TypeConventionBuilder typeConventionBuilder, Func<JsonProperty, bool> propertyMatcher)
        {
            this.typeConventionBuilder = typeConventionBuilder;
            this.propertyMatcher = propertyMatcher;
        }

        public FieldConventionBuilder Modify(Action<JsonProperty> modification)
        {
            typeConventionBuilder.ModifyContract(contract =>
                {
                    var matchingProperties = contract.Properties.Where(propertyMatcher).ToList();
                    foreach (var matchingProperty in matchingProperties)
                    {
                        modification(matchingProperty);
                    }
                });
            return this;
        }
    }

    public class FieldConventionBuilder<TField> : FieldConventionBuilder
    {
        public FieldConventionBuilder(TypeConventionBuilder typeConventionBuilder, Func<JsonProperty, bool> propertyMatcher)
            : base(typeConventionBuilder, propertyMatcher)
        {
        }

        public FieldConventionBuilder<TField> ConvertBeforeSerializing(Func<TField, TField> conversionDelegate)
        {
            Modify(property =>
            {
                var originalValueProvider = property.ValueProvider;
                property.ValueProvider = new DelegateValueProvider(instance => conversionDelegate((TField)originalValueProvider.GetValue(instance)), originalValueProvider.SetValue);
            });

            return this;
        }
    }
}