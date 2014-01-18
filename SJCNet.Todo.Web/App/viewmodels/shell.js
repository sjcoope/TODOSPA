define(['durandal/app', 'plugins/router', 'services/entitymanagerprovider', 'services/logger'],
    function (app, router, entityManagerProvider, logger) {

        function activate() {

            return entityManagerProvider
                    .initialize()
                    .then(boot)
                    .fail(function (e) {
                        return failedInitialization(e);
                    });
        }

        var showProgress = ko.computed(function () {
            if (!router.isNavigating() && !app.isWorking()) return false;
            else return true;
        });

        return {
            activate: activate,
            router: router,
            showProgress: showProgress,
            entityManagerProvider: entityManagerProvider
        };

        function boot() {
            router.makeRelative({ moduleId: 'viewmodels' });
            router.map([
                    { route: '', moduleId: 'dashboard' },
                    { route: 'lists', moduleId: 'lists/lists' },
                    { route: 'lists/:id', moduleId: 'lists/item' },
                    { route: 'dataview/:viewName', moduleId: 'dataview' }
            ])

            // Builds an observable model from the mapping to bind your UI to
            router.buildNavigationModel();

            // Sets up conventional mapping for unrecognized routes
            router.mapUnknownRoutes();

            return router.activate();
        }

        function failedInitialization(error) {
            logger.logError('Initialization failed.', error, null);
            return Q.defer().reject(error);
        }
    });