define([],
    function () {

        function constructor()
        {}

        function initializer(todoItem) {

            todoItem.dueDateText = ko.computed({
                read: function () {
                    var dueDate = todoItem.dueDate();
                    if (dueDate) return moment(dueDate).format("DD/MM/YYYY");
                    return "";
                },
                write: function (value) {
                    if (value) {
                        var newDate = moment(value, "DD-MM-YYYY").toDate();
                        todoItem.dueDate(newDate);
                    }
                }
            });

            todoItem.priorityText = ko.computed({
                read: function () {
                    var priority = todoItem.priority();
                    if (priority)  return priority.name();
                },
                write: function (value) {
                    if(value) todoItem.priority(value);
                }
            });

            todoItem.listName = ko.computed({
                read: function () {
                    var list = todoItem.list();
                    if (list) return list.name();
                }
            });
        };

        return {
            constructor: constructor,
            initializer: initializer
        };
    });