define(['config', 'model/model.todolist', 'model/model.todoitem'],
    function (config, todoListModel, todoItemModel) {

        return {
            configureMetadataStore: configureMetadataStore
        };

        // Internal Methods
        function configureMetadataStore(metadataStore) {
            metadataStore.registerEntityTypeCtor(config.entityNames.todoList, todoListModel.constructor, todoListModel.initializer);
            metadataStore.registerEntityTypeCtor(config.entityNames.todoItem, todoItemModel.constructor, todoItemModel.initializer);
        };
    });