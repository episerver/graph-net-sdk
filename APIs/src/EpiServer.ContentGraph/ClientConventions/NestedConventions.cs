using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using EPiServer.Find.Api;
using EPiServer.Find.Api.Mapping;
using EPiServer.Find.Helpers;
using EPiServer.Find.Helpers.Reflection;
using EPiServer.Find.UnifiedSearch;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EPiServer.Find.ClientConventions
{
    public class NestedConventions
    {
        private IClient client;
        private readonly List<string> nestedItems = new List<string>();
        private static string _format = "{0}.{1}";
        private const string NestedDummyObjectType = "NestedDummyObject" + TypeSuffix.Nested;

        public NestedConventions(IClient client)
        {
            this.client = client;
        }

        public bool IsEnabled { get { return nestedItems.Any(); } }

        private bool Contains(Type type, string name)
        {
            return nestedItems.Contains(string.Format(_format, type, name));
        }

        public bool Contains(JsonProperty property)
        {
            if (!IsEnabled)
            {
                return false;
            }

            return
                Contains(
                    property.DeclaringType.IsDynamicTypeWithBaseType() ? property.DeclaringType.BaseType : property.DeclaringType,
                    property.PropertyName);
        }

        public bool Contains(Expression fieldSelector)
        {
            if (!IsEnabled)
            {
                return false;
            }

            var name = fieldSelector.GetFieldPath();
            var member = fieldSelector.GetDeclaringType();
            return ((member.IsNotNull() && Contains(member, name)));
        }

        public void ForType<TSource, TListItem>(Expression<Func<TSource, IEnumerable<TListItem>>> exp)
            where TSource : class where TListItem : class
        {
            if (typeof(TSource).IsInterface)
            {
                var errorMessage = ConventionsExtensions.GetNonConcreteTypeErrorMessage(typeof(TSource).Name, "interface");

                throw new ArgumentException(errorMessage);
            }

            if (typeof(TSource).IsAbstract)
            {
                var errorMessage = ConventionsExtensions.GetNonConcreteTypeErrorMessage(typeof(TSource).Name, "abstract class");

                throw new ArgumentException(errorMessage);
            }

            var name = exp.GetFieldPath();
            AddNestedType(new[] { typeof(TSource) }, name);
        }

        public void ForInstancesOf<TSource, TListItem>(Expression<Func<TSource, IEnumerable<TListItem>>> exp)
            where TSource : class where TListItem : class
        {
            var declaringType = exp.GetDeclaringType();
            var name = exp.GetFieldPath();
            MarkAllImplementationsAsNested(declaringType, name);
        }

        [Obsolete("IEnumerable<string> cannot be a used to setup a nested convention", true)]
        public void Add<TSource>(Expression<Func<TSource, IEnumerable<string>>> exp)
        {
            throw new ArgumentException("IEnumerable<string> cannot be a used to setup a nested convention");
        }

        [Obsolete("Use NestedConventions.ForType<TSource>().Add<TListItem>() instead")]
        public void Add<TSource>(Expression<Func<TSource, IEnumerable<object>>> exp) where TSource : class
        {
            new NestedConventionItemInstanceWrapper<TSource>(this).Add(exp);
        }

        public INestedConventionItemWrapper<TSource> ForInstancesOf<TSource>() where TSource : class
        {
            return new NestedConventionItemInstanceWrapper<TSource>(this);
        }

        public INestedConventionItemWrapper<TSource> ForType<TSource>() where TSource : class
        {
            if (typeof(TSource).IsInterface)
            {
                var errorMessage = ConventionsExtensions.GetNonConcreteTypeErrorMessage(typeof(TSource).Name, "interface");

                throw new ArgumentException(errorMessage);
            }

            if (typeof(TSource).IsAbstract)
            {
                var errorMessage = ConventionsExtensions.GetNonConcreteTypeErrorMessage(typeof(TSource).Name, "abstract class");

                throw new ArgumentException(errorMessage);
            }

            return new NestedConventionItemTypeWrapper<TSource>(this);
        }

        private void AddNestedType(IEnumerable<Type> declaringTypes, string name)
        {
            foreach (var declaringType in declaringTypes)
            {
                if (!nestedItems.Contains(string.Format(_format, declaringType, name)))
                {
                    nestedItems.Add(string.Format(_format, declaringType, name));
                }
            }

            if (client.IsNotNull())
            {
                var nestedTypeMapping = new TypeMapping(NestedDummyObjectType);
                var getCommand = client.NewCommand(context => new GetMappingCommand(context, nestedTypeMapping));
                getCommand.Indexes.Add(client.DefaultIndex);
                GetMappingResult getResult = null;
                try
                {
                    getResult = getCommand.Execute();
                }
                catch (ServiceException serviceException)
                {
                    var webException = serviceException.InnerException as WebException;
                    var webResponse = webException?.Response as HttpWebResponse;

                    var status = webResponse?.StatusCode;

                    if (!status.HasValue || status != HttpStatusCode.NotFound)
                    {
                        throw;
                    }
                }

                var propertyMappingName = name + TypeSuffix.Nested;
                if (getResult?.Items != null && getResult.Items.ContainsKey(client.DefaultIndex))
                {
                    var jTokenMappings = getResult.Items[client.DefaultIndex];

                    // Slight difference between mapping response in ES 0.9 and 1.7
                    // 0.9 : { "v1_index": { "NestedDummyObject$$nested": {
                    // 1.7 : { "v3_index": { "mappings": { "NestedDummyObject$$nested": {
                    var mappings = jTokenMappings["mappings"];
                    if (mappings.IsNull())
                    {
                        mappings = jTokenMappings;
                    }

                    var typeMapping = JsonConvert.DeserializeObject<TypeMapping>(mappings.ToString());
                    if (typeMapping.Properties.Select(x => x.Name).Contains(propertyMappingName))
                    {
                        return;
                    }
                }

                nestedTypeMapping.Properties.Add(new PropertyMapping(propertyMappingName, new NestedMapping()));
                var command = client.NewCommand(context => new PutMappingCommand(context, nestedTypeMapping));
                command.Indexes.Add(client.DefaultIndex);
                command.Execute();
            }
        }

        private void MarkAllImplementationsAsNested(Type declaringType, string name)
        {
            List<Type> types = new List<Type>();
            types.AddRange(AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.IsDynamicAssembly() && !IsNetFrameworkAssembly(x) && !(IsExcludedAssembly(x)))
                .Types()
                .Where(declaringType.IsAssignableFrom));
            AddNestedType(types, name);
        }

        private static bool IsExcludedAssembly(Assembly assembly)
        {
            return ExcludedAssemblies.Contains(assembly.GetName().Name);
        }

        private static bool IsNetFrameworkAssembly(Assembly assembly)
        {
            var displayName = assembly.GetName().Name.ToUpperInvariant();
            return displayName == "MSCORLIB" ||
                   displayName == "NETSTANDARD" ||
                   displayName == "SYSTEM" ||
                   displayName.StartsWith("SYSTEM.", StringComparison.Ordinal) ||
                   displayName.StartsWith("MICROSOFT.", StringComparison.Ordinal);
        }

        private static ISet<string> ExcludedAssemblies { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Castle.Core",
            "Castle.Windsor",
            "EPiServer.Logging.Compatibility",
            "Lucene.Net",
            "Newtonsoft.Json",
            "NuGet.Core",
            "StructureMap",
            "StructureMap.Net4",
            "StructureMap.Web",
            "SMDiagnostics",
            "ICSharpCode.SharpZipLib",
            "EntityFramework",
            "EntityFramework.SqlServer",
            "elmah",
            "Ninject",
            "AjaxControlToolkit",
            "ComponentArt.Charting.WebChart",
            "ComponentArt.Web.Visualization.Charting",
            "RadEditor.Net2",
            "DotNetOpenAuth",
            "NHibernate",
            "SMTP.Net",
            "IMAP4.Net",
            "Parse.Net",
            "ComponentArt.Silverlight.Server",
            "Common.Logging",
            "ComponentArt.Web.UI",
            "CuteEditor",
            "EPiServer.Common.ComponentArt.Web.UI",
            "nsoftware.IBizPay",
            "WebGrease",
            "Ionic.Zip",
            "Antlr3.Runtime",
            "Machine.TestAdapter",
            "Machine.Specifications",
            "Machine.Specifications.Should",
            "Moq",
            "xunit.runner.visualstudio.testadapter",
            "xunit.runner.reporters.net452"
        };
    }
}