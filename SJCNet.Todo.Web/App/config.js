define(function () {

    var startRoute = 'home';
    var appName = 'Todo';
    var remoteServiceName = '/api/';

    // Output message timeouts
    var infoDisplayTimeout = 1000;
    var errorDisplayTimeout = 3000;
    var warningDisplayTimeout = 2000;
    var successDisplayTimeout = 1000;

    var entityNames = {
        todoList: 'TodoList',
        todoItem: 'TodoItem',
        todoPriority: 'TodoPriority'
    };

    var resourceNames = {
        todoLists: 'data/todoLists',
        todoItems: 'data/todoItems',
        todoPriorities: 'data/todoPriorities'
    };

    var eventNames = {
        reloadListsFromServer: 'reloadListsFromServer'
    };

    var dataToShowInDays = 20;

    return {
        startRoute: startRoute,
        remoteServiceName: remoteServiceName,
        appName: appName,
        entityNames: entityNames,
        eventNames: eventNames,
        resourceNames: resourceNames,
        errorDisplayTimeout: errorDisplayTimeout,
        infoDisplayTimeout: infoDisplayTimeout,
        warningDisplayTimeout: warningDisplayTimeout,
        dataToShowInDays: dataToShowInDays
    };
});