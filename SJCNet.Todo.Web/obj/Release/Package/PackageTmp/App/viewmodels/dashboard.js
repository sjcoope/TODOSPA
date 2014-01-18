define(['services/unitofwork', 'config', 'model/model.todopriority'],
    function (unitofwork, config, todoPriorityModel) {

        var overdueCompletedTasks = ko.observableArray();
        var overdueOutstandingTasks = ko.observableArray();
        var todaysCompletedTasks = ko.observableArray();
        var todaysOutstandingTasks = ko.observableArray();
        var tomorrowsCompletedTasks = ko.observableArray();
        var tomorrowsOutstandingTasks = ko.observableArray();
        var weeksCompletedTasks = ko.observableArray();
        var weeksOutstandingTasks = ko.observableArray();
        var highPriorityCompletedTasks = ko.observableArray();
        var highPriorityOutstandingTasks = ko.observableArray();
        var lowPriorityCompletedTasks = ko.observableArray();
        var lowPriorityOutstandingTasks = ko.observableArray();
        var noPriorityCompletedTasks = ko.observableArray();
        var noPriorityOutstandingTasks = ko.observableArray();
        var mediumPriorityCompletedTasks = ko.observableArray();
        var mediumPriorityOutstandingTasks = ko.observableArray();

        function activate() {
            var now = new Date();
            var today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
            var tomorrow = new Date(now.getFullYear(), now.getMonth(), now.getDate() + 1);
            var tomorrowPlus1 = new Date(now.getFullYear(), now.getMonth(), now.getDate() + 2);
            var yesterday = new Date(now.getFullYear(), now.getMonth(), now.getDate() - 1);
            var todayPlus7Days = new Date(now.getFullYear(), now.getMonth(), now.getDate() + 7);

            unitofwork.todoItems.findInCache(overdueCompletedTasks, new breeze.Predicate.create("dueDate", "<", today)
                                                                        .and("completed", "==", true));

            unitofwork.todoItems.findInCache(overdueOutstandingTasks, new breeze.Predicate.create("dueDate", "<", today)
                                                                        .and("completed", "==", false));

            unitofwork.todoItems.findInCache(todaysCompletedTasks, new breeze.Predicate.create("dueDate", ">", yesterday)
                                                                        .and("dueDate", "<", tomorrow)
                                                                        .and("completed", "==", true));

            unitofwork.todoItems.findInCache(todaysOutstandingTasks, new breeze.Predicate.create("dueDate", ">", yesterday)
                                                                        .and("dueDate", "<", tomorrow)
                                                                        .and("completed", "==", false));

            unitofwork.todoItems.findInCache(tomorrowsCompletedTasks, new breeze.Predicate.create("dueDate", ">", today)
                                                                            .and("dueDate", "<", tomorrowPlus1)
                                                                            .and("completed", "==", true));

            unitofwork.todoItems.findInCache(tomorrowsOutstandingTasks, new breeze.Predicate.create("dueDate", ">", today)
                                                                            .and("dueDate", "<", tomorrowPlus1)
                                                                            .and("completed", "==", false));

            unitofwork.todoItems.findInCache(weeksCompletedTasks, new breeze.Predicate.create("dueDate", ">", yesterday)
                                                                        .and("dueDate", "<", todayPlus7Days)
                                                                        .and("completed", "==", true));

            unitofwork.todoItems.findInCache(weeksOutstandingTasks, new breeze.Predicate.create("dueDate", ">", yesterday)
                                                                        .and("dueDate", "<", todayPlus7Days)
                                                                        .and("completed", "==", false));

            unitofwork.todoItems.findInCache(noPriorityCompletedTasks, new breeze.Predicate.create("todoPriorityId", "==", todoPriorityModel.todoPriorities.None)
                                                                        .and("completed", "==", true));

            unitofwork.todoItems.findInCache(noPriorityOutstandingTasks, new breeze.Predicate.create("todoPriorityId", "==", todoPriorityModel.todoPriorities.None)
                                                                        .and("completed", "==", false));

            unitofwork.todoItems.findInCache(lowPriorityCompletedTasks, new breeze.Predicate.create("todoPriorityId", "==", todoPriorityModel.todoPriorities.Low)
                                                                        .and("completed", "==", true));

            unitofwork.todoItems.findInCache(lowPriorityOutstandingTasks, new breeze.Predicate.create("todoPriorityId", "==", todoPriorityModel.todoPriorities.Low)
                                                                        .and("completed", "==", false));
            
            unitofwork.todoItems.findInCache(highPriorityCompletedTasks, new breeze.Predicate.create("todoPriorityId", "==", todoPriorityModel.todoPriorities.High)
                                                                        .and("completed", "==", true));
            
            unitofwork.todoItems.findInCache(highPriorityOutstandingTasks, new breeze.Predicate.create("todoPriorityId", "==", todoPriorityModel.todoPriorities.High)
                                                                        .and("completed", "==", false));

            unitofwork.todoItems.findInCache(mediumPriorityCompletedTasks, new breeze.Predicate.create("todoPriorityId", "==", todoPriorityModel.todoPriorities.Medium)
                                                                        .and("completed", "==", true));

            unitofwork.todoItems.findInCache(mediumPriorityOutstandingTasks, new breeze.Predicate.create("todoPriorityId", "==", todoPriorityModel.todoPriorities.Medium)
                                                                        .and("completed", "==", false));
        }

        return {
            activate: activate,
            overdueCompletedTasks: overdueCompletedTasks,
            overdueOutstandingTasks: overdueOutstandingTasks,
            todaysCompletedTasks: todaysCompletedTasks,
            todaysOutstandingTasks: todaysOutstandingTasks,
            tomorrowsCompletedTasks: tomorrowsCompletedTasks,
            tomorrowsOutstandingTasks: tomorrowsOutstandingTasks,
            weeksCompletedTasks: weeksCompletedTasks,
            weeksOutstandingTasks: weeksOutstandingTasks,
            highPriorityCompletedTasks: highPriorityCompletedTasks,
            highPriorityOutstandingTasks: highPriorityOutstandingTasks,
            lowPriorityCompletedTasks: lowPriorityCompletedTasks,
            lowPriorityOutstandingTasks: lowPriorityOutstandingTasks,
            noPriorityCompletedTasks: noPriorityCompletedTasks,
            noPriorityOutstandingTasks: noPriorityOutstandingTasks,
            mediumPriorityCompletedTasks: mediumPriorityCompletedTasks,
            mediumPriorityOutstandingTasks: mediumPriorityOutstandingTasks
        };
    });