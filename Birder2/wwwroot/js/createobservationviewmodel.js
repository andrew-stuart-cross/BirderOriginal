﻿CreateObservationViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, observedSpeciesMapping, self);

    ko.bindingHandlers.selectPicker = {
        init: function (element, valueAccessor, allBindings) {
            $(element).selectpicker('render');
        }
    };

    ko.bindingHandlers.datepicker = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            //initialize datepicker with some optional options
            var options = allBindingsAccessor().datepickerOptions || {};
            $(element).datepicker(options);

            //when a user changes the date, update the view model
            ko.utils.registerEventHandler(element, "changeDate", function (event) {
                var value = valueAccessor();
                if (ko.isObservable(value)) {
                    value(event.date);
                }
            });
        },
        update: function (element, valueAccessor) {
            var widget = $(element).data("datepicker");
            //when the view model is updated, update the widget
            if (widget) {
                widget.date = ko.utils.unwrapObservable(valueAccessor());
                if (widget.date) {
                    widget.setValue();
                }
            }
        }
    };

    self.addObservedSpecies = function () {
        var observedSpecies = new ObservedSpeciesViewModel({ Id: 0, BirdId: 0, Quantity: 1 });
        self.ObservedSpecies.push(observedSpecies);
    };

    self.removeObservedSpecies = function () {
        if (self.ObservedSpecies().length > 1)
            self.ObservedSpecies.pop();
    };

    self.disableSubmitButton = ko.observable(false);

    self.Total = ko.computed(function () {
        var total = 0;
        total += self.ObservedSpecies().length;
        return total;
    }),


    self.post = function () {
        self.disableSubmitButton(true);
        if (self.ObservedSpecies().length < 1) {
            // ToDo: Implement proper client-side validation of the Observed Species collection
            alert("You must choose at least one observed bird species");
            self.MessageToClient("You must choose at least one observed bird species...");
            self.disableSubmitButton(false);
            return;
        }
        $.ajax({
            url: "/Observation/Post/",
            type: "POST",
            data: ko.toJSON(self),
            headers:
            {
                "content-type": "application/json; charset=utf-8"
            },
            success: function (data) {
                var obj = JSON.parse(data);
                if (obj.IsModelStateValid === false) {
                    self.MessageToClient(obj.MessageToClient);
                }
                else {
                    window.location.replace("./Index/");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                self.disableSubmitButton(false);
                if (XMLHttpRequest.status === 400) {
                    $('#MessageToClient').text(XMLHttpRequest.responseText);
                }
                else {
                    $('#MessageToClient').text('The web server had an error.  The issue has been logged for investigation by the developer.');
                }
            }
        });
    };
};

var observedSpeciesMapping = {
    'ObservedSpecies': {
        key: function (obsevedSpecies) {
            return ko.utils.unwrapObservable(obsevedSpecies.Id);
        },
        create: function (options) {
            return new CreateObservationViewModel(options.data);
        }
    }
};

ObservedSpeciesViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, observedSpeciesMapping, self);  
};


$("#form").validate({
    submitHandler: function () {
        createObservationViewModel.post();
    },

    rules: {
        //ObservedSpecies: {
        //    min: 1
        //},
        Quantity: {
            required: true,
            digits: true,
            min: 1
        },
        "Observation.BirdId": {
            required: true
        },
        "Observation.ObservationDateTime": {
            date: true
        }
    },

    messages: {
        "Observation.BirdId": {
            required: "You must choose a bird species."//,
            //maxlength: "Customer names must be 30 characters or shorter."
        },
        Quantity: {
            required: "Quantity must be digits only",
            digitsonly: "k"
        }
    },

    tooltip_options: {
        Quantity: {
            placement: 'bottom'
        }
        //PONumber: {
        //    placement: 'right'
        //}
    }
});


// example of custom validation
$.validator.addMethod("alphaonly",
    function (value) {
        return /^[A-Za-z]+$/.test(value);
    }
);




