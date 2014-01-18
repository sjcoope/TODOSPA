using SJCNet.Todo.Data.Repository;
using SJCNet.Todo.Data.Repository.Contract;
using SJCNet.Todo.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCNet.Todo.Data.Uow
{
    public class TodoUow : ITodoUow
    {
        #region Variables

        private EFRepository<TodoList> _todoLists = null;
        private EFRepository<TodoItem> _todoItems = null;
        private EFRepository<TodoPriority> _todoPriorities = null;
    
        #endregion

        #region Constructors

        public TodoUow()
        {
            InitialiseDbContext(null);
        }

        public TodoUow(TodoDbContext context)
        {
            InitialiseDbContext(context);
        }

        #region Properties

        private DbContext Context { get; set; }

        #endregion

        #endregion

        #region Private Methods

        protected void InitialiseDbContext(DbContext context)
        {
            Context = context == null ? new TodoDbContext() : context;
            Context.Configuration.LazyLoadingEnabled = false;
            Context.Configuration.ProxyCreationEnabled = false;
            Context.Configuration.ValidateOnSaveEnabled = false;
        }

        #endregion

        #region Public Methods

        public bool Commit()
        {
            return this.Context.SaveChanges() > 0;
        }

        #endregion

        #region ITodoUow Implementation

        public IRepository<TodoList> TodoLists
        {
            get { return _todoLists ?? (_todoLists = new EFRepository<TodoList>(this.Context)); }
        }

        public IRepository<TodoItem> TodoItems
        {
            get { return _todoItems ?? (_todoItems = new EFRepository<TodoItem>(this.Context)); }
        }


        public IRepository<TodoPriority> TodoPriorities
        {
            get { return _todoPriorities ?? (_todoPriorities = new EFRepository<TodoPriority>(this.Context)); }
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Clear managed resources
                if (this.Context != null)
                {
                    this.Context.Dispose();
                    this.Context = null;
                }
            }
        }

        #endregion
    }
}
