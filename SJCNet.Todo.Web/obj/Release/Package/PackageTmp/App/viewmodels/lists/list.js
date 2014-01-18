
define(['durandal/app', 'services/unitofwork', 'config'],
    function (app, unitofwork, config) {

        var todoLists = ko.observableArray();

        function activate() {
            return loadLists();
        }

        var attached = function (view) {
            // Subscribe to event.
            app.on(config.eventNames.reloadListsFromServer).then(loadLists);
        };

        var addList = function () {
            var initialValues = {
                name: "New List"
            };

            // Create the new list and save the changes (to generate a valid id)
            unitofwork.todoLists.create(initialValues);

            // Update the list
            return unitofwork.todoLists.allInCache(todoLists);
        };

        return {
            activate: activate,
            attached: attached,
            addList: addList,
            todoLists: todoLists
        };

        function loadLists()
        {
            return unitofwork.todoLists.allInCache(todoLists)
                        .then(function() {
                            app.isWorking(false);
                        });
        }
    });