define(['durandal/app', 'services/unitofwork', 'model/model.todopriority', 'config', 'helper'],
    function (app, unitofwork, todopriority, config, helper) {

        var todoItems = ko.observableArray();
        var title = ko.observable();
        var entitiesInEditMode = new Array();
        var todoPriorities = ko.observableArray();
        var mode;

        var hasItems = ko.computed(function () {
            return (todoItems().length > 0);
        });

        function activate(viewName) {
            mode = viewName;

            // Get any lookups
            unitofwork.todoPriorities.allInCache(todoPriorities);

            getTodoItems();
        }

        var completeTodoItem = function () {
            app.isWorking(true);

            var observable = helper.getObservable(this);

            // Update the completed value.
            if (observable.completed()) {
                observable.completed(false);
                observable.completedDate(null);
            } else {
                observable.completed(true);
                observable.completedDate(new Date(Date.now()));
            }

            app.isWorking(false);
        }

        var removeItem = function () {
            app.isWorking(true);

            var todoItem = helper.getObservable(this);
            console.log('removeItem', this);
            var msg = "Are you sure you want to delete the item '" + todoItem.description() + "' from the list '" + todoItem.listName() + "'?";

            app.showMessage(msg, 'Confirmation Required', ['Yes', 'No'])
                .then(function (dialogResult) {
                    if (dialogResult == "Yes") {
                        console.log('todoItem', todoItem);
                        unitofwork.todoItems.remove(todoItem);
                        getTodoItems();
                    }
                });

           app.isWorking(false);
        }

        return {
            activate: activate,
            title: title,
            hasItems: hasItems,
            todoItems: todoItems,
            todoPriorities: todoPriorities,
            completeTodoItem: completeTodoItem,
            removeItem: removeItem
        };

        function getTodoItems(viewName)
        {
            var now = new Date();
            var today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
            var tomorrow = new Date(now.getFullYear(), now.getMonth(), now.getDate() + 1);
            var tomorrowPlus1 = new Date(now.getFullYear(), now.getMonth(), now.getDate() + 2);
            var yesterday = new Date(now.getFullYear(), now.getMonth(), now.getDate() - 1);
            var todayPlus7Days = new Date(now.getFullYear(), now.getMonth(), now.getDate() + 7);

            // Load the correct view of todoitems.
            switch (mode.toLowerCase()) {
                case "overdue":
                    title("Overdue Tasks");
                    unitofwork.todoItems.findInCache(todoItems, new breeze.Predicate.create("dueDate", "<", today));
                    break;
                case "tomorrow":
                    title("Tomorrows Tasks");
                    unitofwork.todoItems.findInCache(todoItems, new breeze.Predicate.create("dueDate", ">", today)
                                                                            .and("dueDate", "<", tomorrowPlus1));
                    break;
                case "week":
                    title("Weeks Tasks");
                    unitofwork.todoItems.findInCache(todoItems, new breeze.Predicate.create("dueDate", ">", yesterday)
                                                                        .and("dueDate", "<", todayPlus7Days));
                    break;
                case "high":
                    title("High Priority Tasks");
                    unitofwork.todoItems.findInCache(todoItems, new breeze.Predicate.create("todoPriorityId", "==", todopriority.todoPriorities.High));
                    break;
                case "medium":
                    title("Medium Priority Tasks");
                    unitofwork.todoItems.findInCache(todoItems, new breeze.Predicate.create("todoPriorityId", "==", todopriority.todoPriorities.Medium));
                    break;
                case "low":
                    title("Low Priority Tasks");
                    unitofwork.todoItems.findInCache(todoItems, new breeze.Predicate.create("todoPriorityId", "==", todopriority.todoPriorities.Low));
                    break;
                case "none":
                    title("No Priority Tasks");
                    unitofwork.todoItems.findInCache(todoItems, new breeze.Predicate.create("todoPriorityId", "==", todopriority.todoPriorities.None));
                    break;
                default:
                    // Today is the default.
                    title("Todays Tasks");
                    unitofwork.todoItems.findInCache(todoItems, new breeze.Predicate.create("dueDate", ">", yesterday)
                                                                        .and("dueDate", "<", tomorrow));
                    break;
            }

            console.log('getTodoItems', todoItems);
        }
    });