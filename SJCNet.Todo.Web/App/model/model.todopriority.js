define([],
    function () {

    	function constructor()
    	{ }

    	function initializer(todoList) {
    	};

    	var todoPriorities = {
    		None: 1,
    		High: 2,
    		Low: 3,
			Medium: 4
		};

    	return {
    		constructor: constructor,
    		initializer: initializer,
    		todoPriorities: todoPriorities
    	};
    });