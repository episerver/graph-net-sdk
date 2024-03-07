define([
    "dojo/_base/declare",
    "dijit/_Widget",
    "dijit/_TemplatedMixin"
], function(
    declare,
    _Widget,
    _TemplatedMixin
) {
    return declare("alloy.editors.CustomStringList", [_Widget, _TemplatedMixin], {

        templateString: "<div class=\"dijitInline\" tabindex=\"-1\" role=\"presentation\">\
                            <span>CustomStringList - This should be displayed for this property!</span>\
                        </div>"
    });
});
