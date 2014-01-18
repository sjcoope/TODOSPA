ko.bindingHandlers.hidden = {
    update: function (element, valueAccessor) {
        ko.bindingHandlers.visible.update(element, function () { return !ko.utils.unwrapObservable(valueAccessor()); });
    }
};

ko.bindingHandlers.datePicker = {
    init: function (element, valueAccessor) {
        $(element).datepicker({
            format: 'dd/mm/yyyy',
            todayHighlight: true
        });
    }
};

ko.bindingHandlers.inlineEditor = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var observable = valueAccessor();
        observable.editing = ko.observable(false);

        // Check for value and input elements
        var editorValue = element.getElementsByClassName("editorValue");
        var editorInput = element.getElementsByClassName("editorInput");

        // If no value and input elements specified create the defaults
        if (editorInput.length == 0 && editorValue.length == 0) {
            $("<span class=\"editorValue\" />").appendTo(element);
            $("<input type=\"text\" class=\"editorInput\" />").appendTo(element);
        };
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var editorValue = element.getElementsByClassName("editorValue")[0];
        var editorInput = element.getElementsByClassName("editorInput")[0];
        if (editorValue == null) throw Error("The parent element with inlineEditor binding must have a child with a class of 'editorValue'.");
        if (editorInput == null) throw Error("The parent element with inlineEditor binding must have a child with a class of 'editorInput'.");

        // Now apply the bindings
        var observable = valueAccessor();
        observable.editing = ko.observable(false);

        // Set-up the editor container.
        ko.applyBindingsToNode(element, {
            click: function () {
                // If the input control is a textarea then set the height as otherwise it resizes.
                var parentWidth = $(element).width();
                $(editorInput).width(parentWidth);

                if ($(editorInput).is("textarea")) {
                    var parentHeight = $(element).height();
                    $(editorInput).height(parentHeight);
                }

                // Update the binding
                observable.editing(true);
            }
        });

        // Set the editor value settings
        ko.cleanNode(editorValue);
        ko.applyBindingsToNode(editorValue, {
            text: observable,
            hidden: observable.editing
        });

        // Set the editor input default settings.
        ko.cleanNode(editorInput);
        ko.applyBindingsToNode(editorInput, {
            visible: observable.editing,
            value: observable,
            hasfocus: observable.editing
        });

        // Set the editor input specific settings
        if ($(editorInput).is("[data-type]")) {
            ko.applyBindingsToNode(editorInput, {
                datePicker: observable
            })
        }

        // Only apply the on enter press event to an input, not a textarea
        if ($(editorInput).is("input")) {
            ko.applyBindingsToNode(editorInput, {
                event: {
                    keyup: function (data, event) {
                        // Enter key
                        if (event.keyCode == 13) {
                            observable.editing(false);
                            return false;
                        }
                    }
                }
            });
        }
    }
};