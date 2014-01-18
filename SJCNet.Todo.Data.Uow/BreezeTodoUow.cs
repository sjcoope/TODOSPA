using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Breeze.WebApi.EF;
using System.Threading.Tasks;
using SJCNet.Todo.Data.Uow.Contract;
using Newtonsoft.Json.Linq;
using Breeze.WebApi;
using SJCNet.Todo.Data.Repository.Contract;
using SJCNet.Todo.Model;
using SJCNet.Todo.Data.Repository;

namespace SJCNet.Todo.Data.Uow
{
    public class BreezeTodoUow : IBreezeTodoUow
    {
        private EFContextProvider<TodoDbContext> _contextProvider = null;
        private IRepository<TodoItem> _todoItemRepository = null;
        private IRepository<TodoList> _todoListRepository = null;
        private IRepository<TodoPriority> _todoPriorities = null;

        public BreezeTodoUow()
        {
            _contextProvider = new EFContextProvider<TodoDbContext>();
        }

        public bool Commit()
        {
            throw new NotImplementedException("Use the Commit(object changeSet) method.");
        }

        public SaveResult Commit(JObject changeSet)
        {
            return _contextProvider.SaveChanges(changeSet);
        }

        public IRepository<TodoItem> TodoItems
        {
            get 
            {
                return _todoItemRepository ?? (_todoItemRepository = new EFRepository<TodoItem>(_contextProvider.Context));
            }
        }

        public IRepository<Model.TodoList> TodoLists
        {
            get 
            {
                return _todoListRepository ?? (_todoListRepository = new EFRepository<TodoList>(_contextProvider.Context));
            }
        }

        public IRepository<TodoPriority> TodoPriorities
        {
            get
            {
                return _todoPriorities ?? (_todoPriorities = new EFRepository<TodoPriority>(_contextProvider.Context));
            }
        }

        public void Dispose()
        {
            if (_contextProvider != null && _contextProvider.Context != null)
            {
                _contextProvider.Context.Dispose();
                _contextProvider = null;
            }
        }
    }
}
