define([],
function () {

    function getObservable(input) {
        var observable = input;
        if (typeof (input) == "function") observable = input();
        return observable;
    }

    return {
        getObservable: getObservable
    };

});

