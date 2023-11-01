using System;
using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace AlloyMvcTemplates.Business.EditorDescriptors;

// format - LAT;LON;ZOOM
// example - 50.1062751;19.8529595;14
[Serializable]
public class GeoCoordinate
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public int Zoom { get; set; }

    public GeoCoordinate(decimal latitude, decimal longitude, int zoom)
    {
        Latitude = latitude;
        Longitude = longitude;
        Zoom = zoom;
    }
}

[PropertyDefinitionTypePlugIn]
public class PropertyGeoCoordinate : PropertyData
{
    private GeoCoordinate _geoCoordinate;

    public PropertyGeoCoordinate()
    {
    }

    public PropertyGeoCoordinate(GeoCoordinate geoCoordinate)
    {
        _geoCoordinate = geoCoordinate;
    }

    protected override void SetDefaultValue()
    {
        _geoCoordinate = new GeoCoordinate(0, 0, 0);
    }

    public override void ParseToSelf(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var values = value.Split(";");
        if (values.Length != 3)
        {
            return;
        }

        _geoCoordinate = new GeoCoordinate(decimal.Parse(values[0]), decimal.Parse(values[1]), int.Parse(values[2]));
    }

    public override object Value
    {
        get => this.IsNull ? null : (object) this._geoCoordinate;
        set => this.SetPropertyValue(value, () =>
        {
            if (value is string str)
            {
                ParseToSelf(str);
            }
            else
            {
                _geoCoordinate = (GeoCoordinate)value;
            }
        });
    }

    public override PropertyDataType Type => PropertyDataType.String;
    public override Type PropertyValueType => typeof(GeoCoordinate);
}

[EditorDescriptorRegistration(TargetType = typeof(GeoCoordinate))]
public class GeoCoordinateDescriptor : EditorDescriptor
{
    public GeoCoordinateDescriptor()
    {
        ClientEditingClass = "alloy/Editors/FakeGeoCoordinateEditor";
    }
}
