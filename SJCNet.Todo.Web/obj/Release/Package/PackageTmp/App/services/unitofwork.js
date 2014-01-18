define(['durandal/system', 'services/entitymanagerprovider', 'config', 'durandal/app', 'services/logger',
    'repository/repository.todoitem', 'repository/repository.todolist', 'repository/repository.todopriority'],
    function (system, entityManagerProvider, config, app, logger, todoItemRepository, todoListRepository, todoPriorityRepository) {
        var hasChanges = ko.observable(false);
        var isSaving = ko.observable(false);
        var provider = entityManagerProvider.create();

        // Create the repositories.
        var todoItems = new todoItemRepository(provider);
        var todoLists = new todoListRepository(provider);
        var todoPriorities = new todoPriorityRepository(provider);

        // Link the hasChanges property to the breeze entity manager hasChangesChanged event.
        provider.manager().hasChangesChanged.subscribe(function (eventArgs) {
            hasChanges(eventArgs.hasChanges);
        });

        var commit = function () {
            isSaving(true);

            var saveOptions = new breeze.SaveOptions({ resourceName: 'data/savechanges' });

            return provider.manager().saveChanges(null, saveOptions)
                .then(saveSucceeded)
                .fail(saveFailed);

            function saveSucceeded(saveResult) {
                logger.logSuccess('Changes saved...', saveResult, system.getModuleId(unitofwork), saveResult, true);
                isSaving(false);
                return true;
            }

            function saveFailed(error) {
                var msg = 'Save Failed: </br>' + getErrorMessages(error);
                logger.logError(msg, error, system.getModuleId(unitofwork), true);
                isSaving(false);
                return false;
            }
        };

        var revert = function () {
            provider.manager().rejectChanges();
            logger.log("Changes cancelled...", null, system.getModuleId(unitofwork), true);
            return Q.resolve();
        };

        var getChangesByType = function (entityName) {
            return provider.manager().getChanges(entityName);
        };

        var unitofwork = {
            getChangesByType: getChangesByType,
            hasChanges: hasChanges,
            isSaving: isSaving,
            commit: commit,
            revert: revert,
            todoItems: todoItems,
            todoLists: todoLists,
            todoPriorities: todoPriorities
        };
        return unitofwork;

        function getErrorMessages(error) {
            var msg = error.message;

            // Check if we have a vlaidation error or "general" error.
            if (msg.match(/validation error/i)) return getValidationMessages(error);
            return msg;
        }

        function getValidationMessages(error) {
            try {
                return error.entityErrors.map(function (valError) {
                    return valError.entity.entityType.shortName + ": " + valError.errorMessage;
                }).join('; <br /><br />');
            }
            catch (e) {
                logger.logError(e.message, e, system.getModuleId(unitofwork), false);
                return 'Validation Error<br /><br />Please refer to console for more details.';
            }
        }
    });