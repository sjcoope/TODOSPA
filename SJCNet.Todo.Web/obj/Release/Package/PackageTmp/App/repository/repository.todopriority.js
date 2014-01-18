define(['repository/repository.base', 'config'],
	function (repositoryBase, config) {

	    var repository = function (entityManagerProvider) {
	        // Construct the base object
	        repositoryBase.call(this, entityManagerProvider, config.entityNames.todoPriority, config.resourceNames.todoPriorities);
	    }

	    // Set the inheritance.  We constructed the base object in the constructor above, so we don't need to pass in parameters.  
	    // This makes the inheritance cleaner without having to call this.prototype.blah when using this object
	    repository.prototype = new repositoryBase();
	    repository.prototype.constructor = repository;

	    return repository;
	});