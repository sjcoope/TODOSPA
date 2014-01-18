define(['durandal/app', 'services/unitofwork', 'plugins/router', 'config', 'helper'],
    function (app, unitofwork, router, config, helper) {
        var todoList = ko.observable();
        var todoPriorities = ko.observableArray();
        var entitiesInEditMode = new Array();

        function activate(id) {
            return unitofwork.todoLists.withId(todoList, id)
                        .then(function () {
                            return unitofwork.todoPriorities.allInCache(todoPriorities);
                        });
        }

        var completeTodoItem = function () {
            var observable = helper.getObservable(this);

            // Update the completed value.
            if (observable.completed()) {
                observable.completed(false);
                observable.completedDate(null);
            } else {
                observable.completed(true);
                observable.completedDate(new Date(Date.now()));
            }
        }

        var removeList = function () {
            app.isWorking(true);

            var observable = helper.getObservable(this);
            var msg = "Are you sure you want to delete list '" + observable.name() + "'?";

            app.showMessage(msg, 'Confirmation Required', ['Yes', 'No']).then(function (dialogResult) {
                if (dialogResult == "Yes")
                {
                    app.isWorking(true);

                    // Remove the selected child list items
                    $(observable.items()).each(function () {
                        unitofwork.todoItems.remove(this);
                    });

                    // Remove the list
                    unitofwork.todoLists.remove(observable);

                    // Reload the todo lists
                    app.trigger(config.eventNames.reloadListsFromServer);

                    // Return to the dashboard view.
                    router.navigate('#');
                }
            });

            app.isWorking(false);
        };

        var addItem = function () {
            app.isWorking(true);

            var list = helper.getObservable(this);
            var defaultPriority = helper.getObservable(todoPriorities()[0]);
            
            var initialValues = {
                description: "New Todo Item"
            };

            // Create the new list and save the changes
            var newEntity = unitofwork.todoItems.create(initialValues);
            newEntity.list(list);
            newEntity.priorityText(defaultPriority);

            app.isWorking(false);
        };

        var removeItem = function () {
            app.isWorking(true);

            var todoItem = helper.getObservable(this);
            var msg = "Are you sure you want to delete the item '" + todoItem.description() + "'?";

            app.showMessage(msg, 'Confirmation Required', ['Yes', 'No']).then(function (dialogResult) {
                if (dialogResult == "Yes") {
                    unitofwork.todoItems.remove(todoItem);
                }
            });

            app.isWorking(false);
        }

        return {
            activate: activate,
            todoList: todoList,
            todoPriorities: todoPriorities,
            completeTodoItem: completeTodoItem,
            removeList: removeList,
            addItem: addItem,
            removeItem: removeItem
        };

    });