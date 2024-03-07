define([
    "dojo/_base/declare",
    "dijit/_Widget",
    "dijit/_TemplatedMixin"
], function(
    declare,
    _Widget,
    _TemplatedMixin
) {
    return declare("alloy.editors.FakeGeoCoordinateEditor", [_Widget, _TemplatedMixin], {

        templateString: "<div class=\"dijitInline\" tabindex=\"-1\" role=\"presentation\">\
                            <span>FAKE GEO COORDINATE MAPS PICKER</span>\
                        </div>"
    });
});
