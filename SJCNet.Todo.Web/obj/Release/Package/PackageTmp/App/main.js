requirejs.config({
    paths: {
        'text': '../Scripts/text',
        'durandal': '../Scripts/durandal',
        'plugins': '../Scripts/durandal/plugins',
        'transitions': '../Scripts/durandal/transitions'
    }
});

define('jquery', [], function () { return jQuery; });
define('knockout', [], function () { return ko; });

define(['durandal/system', 'durandal/app', 'durandal/viewLocator', 'config'],
    function (system, app, viewLocator, config) {

        system.debug(true);

        // Attach the isWorking observable to the durandal app, this is used to 
        // show/hide the progress indicator.
        app.isWorking = ko.observable(false);

        app.title = config.appName;

        //specify which plugins to install and their configuration
        app.configurePlugins({
            router: true,
            dialog: true
        });

        app.start()
            .then(function () {
                viewLocator.useConvention();
                app.setRoot('viewmodels/shell');
            })
            .fail(function(error) {
                console.log(error);
            });
    });