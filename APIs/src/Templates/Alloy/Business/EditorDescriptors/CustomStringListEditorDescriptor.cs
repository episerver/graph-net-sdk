using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace AlloyMvcTemplates.Business.EditorDescriptors;

[EditorDescriptorRegistration(TargetType = typeof(IList<string>), UIHint = "CustomStringList")]
public class CustomStringListEditorDescriptor : EditorDescriptor
{
    public CustomStringListEditorDescriptor()
    {
        ClientEditingClass = "alloy/Editors/CustomStringList";
    }
}
