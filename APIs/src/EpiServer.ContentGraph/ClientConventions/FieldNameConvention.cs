using System.Collections.Generic;
using System.Linq.Expressions;
using EPiServer.Find.Helpers;
using EPiServer.Find.Helpers.Reflection;
using System;
using EPiServer.Find.Helpers.Linq;

namespace EPiServer.Find.ClientConventions
{
    public class FieldNameConvention
    {
        private NestedConventions nestedConventions;
        public const string GeoLocationTypeSuffix = TypeSuffix.GeoLocation;
        public const string AttachmentTypeSuffix =  TypeSuffix.Attachment;
        public const string NestedTypeSuffix = TypeSuffix.Nested;

        public FieldNameConvention() : this(null) { }

        public FieldNameConvention(NestedConventions nestedConventions)
        {
            this.nestedConventions = nestedConventions;
        }

        public virtual string GetFieldName(Expression fieldSelector)
        {
            var name = fieldSelector.GetFieldPath();

            if (typeof(GeoLocation).IsAssignableFrom(fieldSelector.GetReturnType()))
            {
                name = name + GeoLocationTypeSuffix;
            }
            if (typeof(Attachment).IsAssignableFrom(fieldSelector.GetReturnType()))
            {
                name = name + AttachmentTypeSuffix;
            }
            if (nestedConventions.IsNotNull() && nestedConventions.Contains(fieldSelector))
            {
                return name + NestedTypeSuffix;
            }

            return name;
        }

        public virtual string GetNestedFieldPath(Expression fieldSelector)
        {
            if (nestedConventions.IsNull() || !nestedConventions.Contains(fieldSelector))
            {
                throw new ArgumentException(string.Format("{0}.{1} is not a nested object. Ensure you have added a nested convention for it.", fieldSelector.GetDeclaringType().FullName, fieldSelector.GetFieldPath()));
            }
            return fieldSelector.GetFieldPath() + TypeSuffix.Nested;
        }

        public virtual bool IsObjectField(Expression fieldSelector)
        {
            return !GetFieldName(fieldSelector).Contains(TypeSuffix.Separator) && !IsSupportedType(fieldSelector.GetReturnType()) && !IsSupportedEnumerableType(fieldSelector.GetReturnType());
        }

        private bool IsSupportedType(Type type)
        {
            if (Nullable.GetUnderlyingType(type) != null)
            {
                type = Nullable.GetUnderlyingType(type);
            }
            if (type.IsCustomStruct()) return false;
            return  IsTreatedAsValueType(type);
        }

        private static bool IsTreatedAsValueType(Type type)
        {
            return type.IsValueType || type == typeof (string) || type == typeof (GeoLocation);
        }


        private bool IsSupportedEnumerableType(Type type)
        {
            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(IEnumerable<>) || genericTypeDefinition == typeof(List<>))
                {
                    return IsTreatedAsValueType(type.GetGenericArguments()[0]);
                }
            }
            return false;
        }

        public virtual string GetFieldNameForSearch(Expression fieldSelector, Language lang)
        {
            var filterFieldName = GetFieldName(fieldSelector);
            var type = fieldSelector.GetReturnType();
            if (type != typeof(string) && !typeof(IEnumerable<string>).IsAssignableFrom(type))
            {
                return filterFieldName;
            }
            
            if (lang is Language.NoLanguage)
            {
                return string.Format("{0}.standard", filterFieldName);
            }

            return string.Format("{0}.{1}", filterFieldName, lang.FieldSuffix);
        }

        public virtual string GetFieldNameForAnalyzed(Expression fieldSelector)
        {
            var fieldName = GetFieldName(fieldSelector);
            return string.Format("{0}.standard", fieldName);
        }

        public virtual string GetFieldNameForLowercase(Expression fieldSelector)
        {
            var fieldName = GetFieldName(fieldSelector);
            return string.Format("{0}.lowercase", fieldName);
        }

        public virtual string GetFieldNameForSort(Expression fieldSelector)
        {
            var fieldName = GetFieldName(fieldSelector);
            var type = fieldSelector.GetReturnType();
            if (type != typeof(string))
            {
                return fieldName;
            }
            return string.Format("{0}.sort", fieldName);
        }
    }
}
