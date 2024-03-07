define([
    "dojo/_base/declare",
    "dijit/_Widget",
    "dijit/_TemplatedMixin"
], function(
    declare,
    _Widget,
    _TemplatedMixin
) {
    return declare("alloy.editors.MoneyFakeEditor", [_Widget, _TemplatedMixin], {

        templateString: "<div class=\"dijitInline\" tabindex=\"-1\" role=\"presentation\">\
                            <span>MONEY FAKE EDITOR</span>\
                        </div>"
    });
});
