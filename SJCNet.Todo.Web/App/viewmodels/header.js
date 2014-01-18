define(['services/unitofwork', 'durandal/app'],
    function (unitofwork, app) {

        function activate()
        {}

        var cancelChanges = function () {
            app.showMessage('Are you sure you want to undo all changes?', 'Confirmation Required', ['Yes', 'No'])
                .then(function (dialogResult) {
                    if (dialogResult == "Yes") {
                        if (unitofwork.hasChanges())
                        {
                            app.isWorking(true);
                            unitofwork.revert()
                                .then(function() {
                                    app.isWorking(false);
                                });
                        }
                    }
                }
            );
        };

        var saveChanges = function () {
            if (unitofwork.hasChanges()) {
                app.isWorking(true);
                unitofwork.commit()
                    .then(function() {
                        app.isWorking(false);
                    });
            }
        };

        return {
            unitofwork: unitofwork,
            cancelChanges: cancelChanges,
            saveChanges: saveChanges
        }
    });