define(['services/unitofwork'],
    function (unitofwork) {

        var todoItems = ko.observableArray();

        function activate() {
            return unitofwork.todoItems.all(todoItems);
        }

        return {
            activate: activate,
            todoItems: todoItems
        };
    });