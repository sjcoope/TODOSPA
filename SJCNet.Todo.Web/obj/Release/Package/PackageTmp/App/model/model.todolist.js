define([],
    function () {

        function constructor()
        { }

        function initializer(todoList) {
            todoList.hasItems = ko.computed({
                read: function () {
                    return (todoList.items().length > 0);
                }
            });
        };

        return {
            constructor: constructor,
            initializer: initializer
        };
    });