define(['durandal/system', 'services/logger'],
    function (system, logger) {

    	var repositoryBase = function (entityManagerProvider, entityTypeName, resourceName, fetchStrategy) {
    	    // Set properties
    		this.fetchStrategy = fetchStrategy;
    		this.resourceName = resourceName;
    		this.entityTypeName = entityTypeName;
    		this.entityType = null;

    		// Ensure resourceName is registered
    		if (entityTypeName) {
    		    this.entityType = getMetastore().getEntityType(entityTypeName);
    		    this.entityType.setProperties({ defaultResourceName: resourceName });
    		    getMetastore().setEntityTypeForResourceName(resourceName, entityTypeName);
    		}

    		this.manager = function () {
    			return entityManagerProvider.manager();
    		}

    		function getMetastore() {
    			return entityManagerProvider.manager().metadataStore;
    		}
    	};

    	repositoryBase.prototype.create = function (initialValues) {
    	    var newEntity = this.entityType.createEntity(initialValues);
    	    this.manager().addEntity(newEntity);
    	    return newEntity;
    	};

    	repositoryBase.prototype.remove = function (entity) {
    	    return entity.entityAspect.setDeleted();
    	};

    	repositoryBase.prototype.executeQuery = function (query) {
    		return this.manager()
                .executeQuery(query)
                .then(function (data) {
                    return data.results;
                });
    	}

    	repositoryBase.prototype.executeCacheQuery = function (query) {
    		return this.manager().executeQueryLocally(query);
    	};

    	repositoryBase.prototype.queryFailed = function (error) {
    		logger.logError('Query failed', error, system.getModuleId(repositoryBase), false);
    	    throw error;
    	};

    	repositoryBase.prototype.withId = function (observable, key) {
    		if (!this.entityTypeName) throw new Error("Repository must be created with an entity type specified");

            // Checks the local cache of entities first, if the entity can't be found then we query the server.
    		return this.manager().fetchEntityByKey(this.entityTypeName, key, true)
                .then(function (data) {
                	if (!data.entity) throw new Error("Entity not found!");
                	if (observable) observable(data.entity);
                })
                .fail(this.queryFailed);
    	}

    	repositoryBase.prototype.find = function (observable, predicate) {
    		var query = breeze.EntityQuery
                .from(this.resourceName)
                .where(predicate);

    		return this.executeQuery(query)
                    .then(function (result) {
                    	if (observable) observable(result);
                    })
                    .fail(this.queryFailed);
    	};

    	repositoryBase.prototype.findInCache = function (observable, predicate) {
    		var query = breeze.EntityQuery
                .from(this.resourceName)
                .where(predicate);

    		var result = this.executeCacheQuery(query);
    		if (result) {
    			if (observable) {
    				observable(result);
    				return Q.resolve();
    			}
    		}
    	}

    	repositoryBase.prototype.all = function (observableArray) {
    		// Clear any existing values in the array
    		if (observableArray) observableArray([]);

    		var query = breeze.EntityQuery
                .from(this.resourceName);

    		return this.executeQuery(query)
                .then(function (results) {
                	if (observableArray) observableArray(results);
                })
                .fail(this.queryFailed);
    	};

    	repositoryBase.prototype.allInCache = function (observableArray) {
    		// Clear any existing values in the array
    		if (observableArray) observableArray([]);

    		var query = new breeze.EntityQuery()
                                .from(this.resourceName);

    		var results = this.executeCacheQuery(query);
    		if (results.length > 0) {
    			observableArray(results);
    		}

    		return Q.resolve();
    	};

    	return repositoryBase;
    });