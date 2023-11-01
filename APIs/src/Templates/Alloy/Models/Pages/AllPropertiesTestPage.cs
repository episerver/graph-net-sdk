using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AlloyMvcTemplates.Business.EditorDescriptors;
using AlloyTemplates.Models.Blocks;
using EPiServer;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.Serialization;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace AlloyTemplates.Models.Pages
{
    [ContentType(
        GUID = "A7D46007-43E5-4401-9204-127040E79E09",
        GroupName = Global.GroupNames.Specialized)]
    [AvailableContentTypes(
        Availability.Specific,
        IncludeOn = new[] { typeof(StartPage) })
    ]
    public class AllPropertiesTestPage : PageData
    {
        private const string Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus tortor turpis, ultrices nec placerat vel, scelerisque lobortis est.";

        [Display(Name = "Content Area", Description = Description, GroupName = SystemTabNames.Content, Order = 10)]
        public virtual ContentArea ContentArea1 { get; set; }

        [Display(Name = "Content Area [Readonly]", Description = Description, GroupName = SystemTabNames.Content, Order = 20)]
        [ReadOnly(true)]
        public virtual ContentArea ContentAreaReadonly1 { get; set; }

        [Display(Name = "Content Reference List", Description = Description, GroupName = SystemTabNames.Content, Order = 50)]
        public virtual IEnumerable<ContentReference> ContentReferenceList1 { get; set; }

        [Display(Name = "Content Reference List (Allowed Types)", Description = Description, GroupName = SystemTabNames.Content, Order = 50)]
        [AllowedTypes(typeof(EditorialBlock), typeof(TeaserBlock), typeof(JumbotronBlock))]
        public virtual IEnumerable<ContentReference> ContentReferenceListAllowedTypes { get; set; }

        [Display(Name = "Content Reference List [Readonly]", Description = Description, GroupName = SystemTabNames.Content, Order = 60)]
        [ReadOnly(true)]
        public virtual IEnumerable<ContentReference> ContentReferenceListReadonly1 { get; set; }

        [Display(Name = "Link item collection", Description = Description, GroupName = SystemTabNames.Content, Order = 70)]
        public virtual LinkItemCollection LinkItemCollection1 { get; set; }

        [Display(Name = "Link item collection [Readonly]", Description = Description, GroupName = SystemTabNames.Content, Order = 71)]
        [ReadOnly(true)]
        public virtual LinkItemCollection LinkItemCollectionReadonly1 { get; set; }

        [Display(Name = "Link item", GroupName = SystemTabNames.Content, Order = 75)]
        public virtual LinkItem LinkItem { get; set; }

        [Display(Name = "Link item [Readonly]", GroupName = SystemTabNames.Content, Order = 76)]
        [ReadOnly(true)]
        public virtual LinkItem LinkItemReadonly { get; set; }

        [Display(Name = "Url", Description = Description, GroupName = SystemTabNames.Content, Order = 81)]
        public virtual Url Url { get; set; }

        [Display(Name = "Url [Readonly]", Description = Description, GroupName = SystemTabNames.Content, Order = 82)]
        [ReadOnly(true)]
        public virtual Url UrlReadonly { get; set; }

        [UIHint(UIHint.Image)]
        [Display(Name = "Url to image", Description = Description, GroupName = SystemTabNames.Content, Order = 82)]
        public virtual Url UrlToImage { get; set; }

        [Display(Name = "Text", Description = Description, GroupName = SystemTabNames.Content, Order = 90)]
        public virtual string Text1 { get; set; }

        [Display(Name = "Text [Readonly]", Description = Description, GroupName = SystemTabNames.Content, Order = 100)]
        [ReadOnly(true)]
        public virtual string TextReadonly1 { get; set; }

        [Display(Name = "TextArea", Description = Description, GroupName = SystemTabNames.Content, Order = 110)]
        [UIHint(UIHint.Textarea)]
        public virtual string TextArea1 { get; set; }

        [Display(Name = "TextArea [Readonly]", Description = Description, GroupName = SystemTabNames.Content, Order = 120)]
        [UIHint(UIHint.Textarea)]
        [ReadOnly(true)]
        public virtual string TextAreaReadonly1 { get; set; }

        [Display(Name = "Previewable text", Description = Description, GroupName = SystemTabNames.Content, Order = 130)]
        [UIHint(UIHint.PreviewableText)]
        public virtual string PreviewableText1 { get; set; }

        [Display(Name = "Previewable text [Readonly]", Description = Description, GroupName = SystemTabNames.Content, Order = 140)]
        [UIHint(UIHint.PreviewableText)]
        [ReadOnly(true)]
        public virtual string PreviewableTextReadonly1 { get; set; }

        [Display(Name = "Date", Description = Description, GroupName = SystemTabNames.Content, Order = 150)]
        public virtual DateTime Date1 { get; set; }

        [Display(Name = "Date [Readonly]", Description = Description, GroupName = SystemTabNames.Content, Order = 160)]
        [ReadOnly(true)]
        public virtual DateTime DateReadonly1 { get; set; }

        [Display(Name = "Integer", Description = Description, GroupName = SystemTabNames.Content, Order = 170)]
        public virtual int Integer1 { get; set; }

        [Display(Name = "Integer [Readonly]", Description = Description, GroupName = SystemTabNames.Content, Order = 180)]
        [ReadOnly(true)]
        public virtual int IntegerReadonly1 { get; set; }

        [Display(Name = "Integer - range (0-10)", Description = Description, GroupName = SystemTabNames.Content, Order = 190)]
        [Range(0, 10)]
        public virtual int IntegerRange1 { get; set; }

        [Display(Name = "Boolean", Description = Description, GroupName = SystemTabNames.Content, Order = 200)]
        public virtual bool Bool1 { get; set; }

        [Display(Name = "Boolean [Readonly]", Description = Description, GroupName = SystemTabNames.Content, Order = 210)]
        [ReadOnly(true)]
        public virtual bool BoolReadonly1 { get; set; }

        [Display(Name = "GUID", Description = Description, GroupName = SystemTabNames.Content, Order = 220)]
        public virtual Guid Guid { get; set; }

        [Display(Name = "Integer List [Readonly]", Description = Description, GroupName = Tabs.List, Order = 230)]
        [ReadOnly(true)]
        public virtual IEnumerable<int> IntegerListReadonly1 { get; set; }

        [Display(Name = "String List [Only letters]", Description = Description, GroupName = Tabs.List, Order = 231)]
        [ItemRegularExpression("[a-zA-Z]*")]
        public virtual IList<string> StringListWithRegexValidation { get; set; }

        [Display(Name = "String List [Max 10 characters]", Description = Description, GroupName = Tabs.List, Order = 232)]
        [ItemStringLength(10)]
        public virtual IList<string> StringListWithMax10Characters { get; set; }

        [Display(Name = "Single select", Description = Description, GroupName = SystemTabNames.Content, Order = 250)]
        [SelectOne(SelectionFactoryType = typeof(TestSelectionFactory))]
        public virtual string SingleSelect1 { get; set; }

        [Display(Name = "Single select [Readonly]", Description = Description, GroupName = SystemTabNames.Content, Order = 260)]
        [SelectOne(SelectionFactoryType = typeof(TestSelectionFactory))]
        [ReadOnly(true)]
        public virtual string SingleSelectReadonly1 { get; set; }

        [Display(Name = "Multi select", Description = Description, GroupName = SystemTabNames.Content, Order = 270)]
        [SelectMany(SelectionFactoryType = typeof(TestSelectionFactory))]
        public virtual string MultiSelect1 { get; set; }

        [Display(Name = "Multi select [Readonly]", Description = Description, GroupName = SystemTabNames.Content, Order = 280)]
        [SelectMany(SelectionFactoryType = typeof(TestSelectionFactory))]
        [ReadOnly(true)]
        public virtual string MultiSelectReadonly1 { get; set; }

        [Display(Name = "List property", Description = Description, GroupName = Tabs.PropertyList, Order = 290)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<Person>))]
        public virtual IList<Person> Persons { get; set; }

        [Display(Name = "List property [Readonly]", Description = Description, GroupName = Tabs.PropertyList, Order = 300)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<Person>))]
        [ReadOnly(true)]
        public virtual IList<Person> PersonsReadonly { get; set; }

        [AutoSuggestSelection(typeof(TestSelectionQuery))]
        [Display(Name = "Auto suggest selection editor", Description = Description, GroupName = SystemTabNames.Content, Order = 310)]
        public virtual string SelectionEditor1 { get; set; }

        [AutoSuggestSelection(typeof(TestSelectionQuery))]
        [Display(Name = "Auto suggest selection editor [Readonly]", Description = Description, GroupName = SystemTabNames.Content, Order = 330)]
        [ReadOnly(true)]
        public virtual string SelectionEditorReadonly1 { get; set; }

        [AutoSuggestSelection(typeof(TestSelectionQuery), AllowCustomValues = true)]
        [Display(Name = "Auto suggest selection editor with custom values", Description = Description, GroupName = SystemTabNames.Content, Order = 320)]
        public virtual string SelectionEditor2 { get; set; }

        #region List<T>

        [ListItems(0, 3)]
        [Display(Name = "List of EditorialBlock (0-3 items)", Description = Description, GroupName = Tabs.List, Order = 10)]
        public virtual IEnumerable<EditorialBlock> EditorialBlocks { get; set; }

        [ListItems(0, 2)]
        [Display(Name = "List of XhtmlString (0-2 items)", Description = Description, GroupName = Tabs.List, Order = 20)]
        public virtual IEnumerable<XhtmlString> XhtmlStrings { get; set; }

        [Display(Name = "Single ContactBlock", Description = Description, GroupName = Tabs.List, Order = 25)]
        public virtual ContactBlock ContactBlock { get; set; }

        [Display(Name = "List of ContactBlock", Description = Description, GroupName = Tabs.List, Order = 30)]
        [ListItemHeaderProperty(nameof(Blocks.ContactBlock.Heading))]
        public virtual IEnumerable<ContactBlock> ContactBlocks { get; set; }

        [Display(Name = "Enumerable of int", Description = Description, GroupName = Tabs.List, Order = 40)]
        public virtual IEnumerable<int> EnumerableInts { get; set; }

        [Display(Name = "Enumerable of int (with explicit PropertyCollection to avoid storing as JSON in the db)", Description = Description, GroupName = Tabs.List, Order = 45)]
        [BackingType(typeof(PropertyCollection))]
        public virtual IEnumerable<int> EnumerableIntsPropertyList { get; set; }

        [Display(Name = "Enumerable of PageReference (with explicit PropertyCollection to avoid storing as JSON in the db)", Description = Description, GroupName = Tabs.List, Order = 45)]
        [BackingType(typeof(PropertyCollection))]
        public virtual IEnumerable<PageReference> EnumerablePageReferencesPropertyList { get; set; }

        [Display(Name = "Enumerable of ContentReference (with explicit PropertyCollection to avoid storing as JSON in the db)", Description = Description, GroupName = Tabs.List, Order = 45)]
        [BackingType(typeof(PropertyCollection))]
        public virtual IEnumerable<ContentReference> EnumerableContentReferencesPropertyList { get; set; }

        [ItemRange(0, 10)]
        [Display(Name = "List of int (each number between 0 and 10)", Description = Description, GroupName = Tabs.List, Order = 50)]
        public virtual IList<int> ListInts { get; set; }

        [Display(Name = "List of string", Description = Description, GroupName = Tabs.List, Order = 60)]
        [ItemStringLength(10)]
        public virtual IList<string> ListStrings { get; set; }

        [Display(Name = "List of DateTime", Description = Description, GroupName = Tabs.List, Order = 70)]
        public virtual IList<DateTime> Dates { get; set; }

        [Display(Name = "List of Guid", Description = Description, GroupName = Tabs.List, Order = 80)]
        public virtual IList<Guid> Guids { get; set; }

        [Display(Name = "List of LinkItem", Description = Description, GroupName = Tabs.List, Order = 90)]
        public virtual IList<LinkItem> LinksList { get; set; }

        [Display(Name = "Enumerable of LinkItem", Description = Description, GroupName = Tabs.List, Order = 100)]
        public virtual IEnumerable<LinkItem> LinksEnumerable { get; set; }

        [Display(Name = "Enumerable of PageReference", Description = Description, GroupName = Tabs.List, Order = 110)]
        public virtual IEnumerable<PageReference> Pages { get; set; }

        [Display(Name = "Enumerable of Url", Description = Description, GroupName = Tabs.List, Order = 120)]
        public virtual IEnumerable<Url> Urls { get; set; }

        [Display(Name = "Enumerable of ContentReference (with Image UIHint)", Description = Description, GroupName = Tabs.List, Order = 130)]
        [ListItemUIHint(UIHint.Image)]
        public virtual IEnumerable<ContentReference> Images { get; set; }

        [Display(Name = "Enumerable of ContentReference (with Video UIHint)", Description = Description, GroupName = Tabs.List, Order = 140)]
        [ListItemUIHint(UIHint.Video)]
        public virtual IEnumerable<ContentReference> Videos { get; set; }

        [Display(Name = "Enumerable of LongString (with TextArea UIHint)", Description = Description, GroupName = Tabs.List, Order = 150)]
        [ListItemUIHint(UIHint.Textarea)]
        public virtual IEnumerable<string> TextAreas { get; set; }

        [Display(Name = "Enumerable of ContentReference", Description = Description, GroupName = Tabs.List, Order = 160)]
        public virtual IEnumerable<ContentReference> References { get; set; }

        [Display(Name = "Inline block with a List<T> inside", Description = Description, GroupName = Tabs.List, Order = 170)]
        public virtual ListBlock ListBlock { get; set; }

        #endregion

        #region ContentReference

        [Display(Name = "Content Reference", Description = Description, GroupName = "Content Reference", Order = 1)]
        public virtual ContentReference ContentReference1 { get; set; }

        [Display(Name = "Content Reference [Readonly]", Description = Description, GroupName = "Content Reference", Order = 2)]
        [ReadOnly(true)]
        public virtual ContentReference ContentReferenceReadonly1 { get; set; }

        [Display(Name = "Page Reference (AllowedTypes)", Description = Description, GroupName = "Content Reference", Order = 31)]
        [AllowedTypes(typeof(PageData))]
        public virtual ContentReference PageReference1 { get; set; }

        [Display(Name = "Page Reference (Model type)", Description = Description, GroupName = "Content Reference", Order = 32)]
        public virtual PageReference PageReference2 { get; set; }

        [Display(Name = "Content Reference (AllowedTypes `MediaData`)", Description = Description, GroupName = "Content Reference", Order = 33)]
        [AllowedTypes(typeof(MediaData))]
        public virtual ContentReference MediaReference1 { get; set; }

        [Display(Name = "Content Reference (AllowedTypes `ImageData`)", Description = Description, GroupName = "Content Reference", Order = 34)]
        [AllowedTypes(typeof(ImageData))]
        public virtual ContentReference MediaReference2 { get; set; }

        [Display(Name = "Content Reference (UIHint `Media`)", Description = Description, GroupName = "Content Reference", Order = 35)]
        [UIHint(UIHint.MediaFile)]
        public virtual ContentReference MediaReference3 { get; set; }

        [Display(Name = "Content Reference (UIHint `Video`)", Description = Description, GroupName = "Content Reference", Order = 36)]
        [UIHint(UIHint.Video)]
        public virtual ContentReference MediaReference4 { get; set; }

        [Display(Name = "Content Reference (UIHint `Folder`)", Description = Description, GroupName = "Content Reference", Order = 36)]
        [UIHint(UIHint.AssetsFolder)]
        public virtual ContentReference FolderReference { get; set; }

        [Display(Name = "Content Reference (UIHint `Image`)", Description = Description, GroupName = "Content Reference", Order = 37)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference MediaReference5 { get; set; }

        [Display(Name = "Content Reference (AllowedTypes `ImageData & VideoData`)", Description = Description, GroupName = "Content Reference", Order = 38)]
        [AllowedTypes(typeof(ImageData), typeof(VideoData))]
        public virtual ContentReference MediaReference6 { get; set; }

        [Display(Name = "Block Reference (AllowedTypes)", Description = Description, GroupName = "Content Reference", Order = 40)]
        [AllowedTypes(AllowedTypes = new[] { typeof(BlockData) }, RestrictedTypes = new[] { typeof(ButtonBlock), typeof(EditorialBlock) })]
        public virtual ContentReference BlockReference1 { get; set; }

        [Display(Name = "Block Reference (UIHint `Block`)", Description = Description, GroupName = "Content Reference", Order = 41)]
        [UIHint(UIHint.Block)]
        public virtual ContentReference BlockReference2 { get; set; }

        [Display(Name = "Block Reference (AllowedTypes - 3 block types)", Description = Description, GroupName = "Content Reference", Order = 42)]
        [AllowedTypes(typeof(EditorialBlock), typeof(TeaserBlock), typeof(JumbotronBlock))]
        public virtual ContentReference BlockReference3 { get; set; }

        [Display(Name = "Image", Description = Description, GroupName = "Content Reference", Order = 230)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image1 { get; set; }

        [Display(Name = "Image [Readonly]", Description = Description, GroupName = "Content Reference", Order = 240)]
        [UIHint(UIHint.Image)]
        [ReadOnly(true)]
        public virtual ContentReference ImageReadonly1 { get; set; }

        #endregion

        #region Various tests

        [Display(Name = "Money List", Description = "List of custom types without a specific UIHint but a property backing type",
            GroupName = Tabs.CustomEditorDescriptors, Order = 400)]
        public virtual IList<Money> Amounts { get; set; }

        [Display(Name = "Custom IList<string>", Description = "List<string> with a custom UIHint. Users should be able to override the default editor",
            GroupName = Tabs.CustomEditorDescriptors, Order = 410)]
        [UIHint("CustomStringList")]
        public virtual IList<string> CustomStringList { get; set; }

        [Display(Name = "Legacy List<int> with old PropertyIntegerList backing type", Description = "List<int> now has an automatic backing type PropertyList but it should still be possible to use old one",
            GroupName = Tabs.CustomEditorDescriptors, Order = 420)]
        [CultureSpecific]
        [BackingType(typeof(PropertyIntegerList))]
        public virtual IList<int> IntList { get; set; }

        [Display(Name = "Single geo coordinate", GroupName = Tabs.CustomEditorDescriptors, Order = 430)]
        public virtual GeoCoordinate GeoCoordinate { get; set; }

        [Display(Name = "Geo coordinate list", Description = "Example of List<T> property with custom 3rd party type & editor",
            GroupName = Tabs.CustomEditorDescriptors, Order = 440)]
        public virtual IList<GeoCoordinate> ListGeoCoordinate { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 450)]
        public virtual ContentAreaItem ContentAreaItem { get; set; }

        [Display(
            Name = "ContentAreaItem with allowed types",
            GroupName = SystemTabNames.Content,
            Order = 450)]
        [AllowedTypes(typeof(EditorialBlock), typeof(TeaserBlock), typeof(JumbotronBlock))]
        public virtual ContentAreaItem ContentAreaItemWithAllowedTypes { get; set; }

        [Display(
            Name = "ContentAreaItem only blocks",
            GroupName = SystemTabNames.Content,
            Order = 450)]
        [AllowedTypes(typeof(BlockData))]
        public virtual ContentAreaItem ContentAreaItemOnlyBlocks { get; set; }

        public virtual string StringNoValidation { get; set; }

        [StringLength(10)]
        public virtual string StringLength { get; set; }

        [RegularExpression("^[a-z]$")]
        public virtual string StringRegex { get; set; }

        public virtual string IntNoValidation { get; set; }

        [Range(1, 1000)]
        public virtual string Int { get; set; }

        public virtual double FloatNoValidation { get; set; }

        [Range(1, 1000)]
        public virtual double Float { get; set; }

        #endregion

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            ContentReferenceReadonly1 = ContentReference.StartPage;
        }

        private static class Tabs
        {
            public const string List = "List of T";
            public const string PropertyList = "PropertyList (Legacy)";
            public const string CustomEditorDescriptors = "Custom Editor Descriptors";
        }
    }

    public class TestSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new[]
            {
                new SelectItem
                {
                    Text = "aaaaaa",
                    Value = "1"
                },
                new SelectItem
                {
                    Text = "bbbbb",
                    Value = "2"
                },
                new SelectItem
                {
                    Text = "ccccc",
                    Value = "3"
                },
                new SelectItem
                {
                    Text = "ddddd",
                    Value = "4"
                },
                new SelectItem
                {
                    Text = "eeeee",
                    Value = "5"
                },
            };
        }
    }

    public class Person
    {
        [DisplayName("/admin/secedit/firstname")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        public int Age { get; set; }

        [Display(Order = 10, Name = "DoB", Description = "Date of birth")]
        public DateTime DoB { get; set;  }

        [ClientEditor(ClientEditingClass = "epi-cms/form/EmailValidationTextBox")]
        public string Email { get; set; }
    }

    [PropertyDefinitionTypePlugIn]
    public class PersonListProperty : PropertyList<Person>
    {
        public PersonListProperty()
        {
            _objectSerializer = _objectSerializerFactory.Service.GetSerializer(KnownContentTypes.Json);
        }

        private Injected<IObjectSerializerFactory> _objectSerializerFactory;

        private IObjectSerializer _objectSerializer;

        protected override Person ParseItem(string value)
        {
            return _objectSerializer.Deserialize<Person>(value);
        }
    }

    // Sample SelectionQuery for auto-suggestion editor
    // https://world.episerver.com/documentation/developer-guides/CMS/Content/Properties/built-in-property-types/Built-in-auto-suggestion-editor/
    [ServiceConfiguration(typeof(ISelectionQuery))]
    public class TestSelectionQuery : ISelectionQuery
    {
        readonly SelectItem[] _items;
        public TestSelectionQuery()
        {
            _items = new[] {
                new SelectItem() { Text = string.Empty, Value = string.Empty },
                new SelectItem() { Text = "Alternative1", Value = "1" },
                new SelectItem() { Text = "Alternative 2", Value = "2" } };
        }
        //Will be called when the editor types something in the selection editor.
        public IEnumerable<ISelectItem> GetItems(string query)
        {
            return _items.Where(i => i.Text.StartsWith(query, StringComparison.OrdinalIgnoreCase));
        }
        //Will be called when initializing an editor with an existing value to get the corresponding text representation.
        public ISelectItem GetItemByValue(string value)
        {
            return _items.FirstOrDefault(i => i.Value.Equals(value));
        }
    }
}
