using System;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace AlloyMvcTemplates.Business.EditorDescriptors;

[PropertyDefinitionTypePlugIn]
[Serializable]
public class PropertyMoneyList : PropertyList<Money>
{
    public PropertyMoneyList()
    {
    }

    public PropertyMoneyList(IEnumerable<Money> list)
        : base(list)
    {
    }
}

public class Money
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }

    public Money(decimal amount)
    {
        Amount = amount;
    }
}

[EditorDescriptorRegistration(TargetType = typeof(IList<Money>))]
public class MoneyListDescriptor : EditorDescriptor
{
    public MoneyListDescriptor()
    {
        ClientEditingClass = "alloy/Editors/MoneyFakeEditor";
    }
}
