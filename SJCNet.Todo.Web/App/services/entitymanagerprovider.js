define(['durandal/app', 'model/model.base', 'config'],
    function(app, model, config) {
        var hasChanges = ko.observable(false);

        // Get the master manager
        var masterManager = configureBreezeManager();

        // Build the entity manager provider 
        var EntityManagerProvider = (function () {

            var entityManagerProvider = function () {
                var manager;

                this.manager = function () {
                    if (!manager) {
                        manager = masterManager.createEmptyCopy();

                        // Populate with lookup data
                        manager.importEntities(masterManager.exportEntities());
                    }

                    return manager;
                };
            };

            return entityManagerProvider;
        })();

        var self = {
            initialize: initialize,
            create: create,
            hasChanges: hasChanges,
        };

        return self;

        function create() {
            return new EntityManagerProvider();
        }

        function initialize() {
            // Fetch the metadata from the servers
            return masterManager.fetchMetadata()
                .then(function () {
                    if (self.modelBuilder) {
                        self.modelBuilder(masterManager.metadataStore);
                    }

                    // Initialize the lookup data.
                    return initializeLookups()
                        .then(initializeTodoItems)
                        .then(initializeTodoLists);
                });
        }

        function configureBreezeManager() {
            
            breeze.NamingConvention.camelCase.setAsDefault();

            // configure to resist CSRF attack
            var antiForgeryToken = $("#antiForgeryToken").val();
            if (antiForgeryToken) {
                // get the current default Breeze AJAX adapter & add header
                var ajaxAdapter = breeze.config.getAdapterInstance("ajax");
                ajaxAdapter.defaultSettings = {
                    headers: {
                        'RequestVerificationToken': antiForgeryToken
                    },
                };
            }

            // Change how Breeze handles dates, currently it applies a timezone offset when the dates
            // are being read but not when they're being written and re-read.  Meaning the first time
            // the dates are read the date is incorrect because of the timezone offset.
            breeze.DataType.parseDateFromServer = function (source) {
                var date = moment(source);
                return date.toDate();
            };
                
            // Configure the breeze manager and metadata store.
            var dataService = new breeze.DataService({
                serviceName: config.remoteServiceName
            });
            var mgr = new breeze.EntityManager(dataService);
            model.configureMetadataStore(mgr.metadataStore);

            // Set the manager validation options
            var validationOptions = new breeze.ValidationOptions({
                validateOnAttach: false,
                validateOnPropertyChange: false,
                validateOnSave: true
            });

            mgr.setProperties({
                validationOptions: validationOptions
            });

            return mgr;
        }

        function initializeLookups()
        {
            var query = new breeze.EntityQuery()
                                .from(config.resourceNames.todoPriorities);

            return masterManager.executeQuery(query);
        }

        function initializeTodoItems()
        {
            var now = new Date();
            var dateLimit = new Date();
            dateLimit.setDate(now.getDate() - config.dataToShowInDays)

            // Get todoItems to display
            var p1 = breeze.Predicate.create("createdDate", ">=", dateLimit);
            var p2 = breeze.Predicate.create("createdDate", "<", dateLimit)
                            .and("completed", "==", true);

            var predicate = breeze.Predicate.or([p1, p2]);
            var query = new breeze.EntityQuery()
                                .from(config.resourceNames.todoItems)
                                .where(predicate);

            return masterManager.executeQuery(query);
        }

        function initializeTodoLists()
        {
            var query = new breeze.EntityQuery()
                                .from(config.resourceNames.todoLists)

            return masterManager.executeQuery(query);
        }
    });

