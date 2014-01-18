using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Breeze.WebApi.EF;
using SJCNet.Todo.Data;
using SJCNet.Todo.Model;
using SJCNet.Todo.Data.Uow;
using Breeze.WebApi;
using Newtonsoft.Json.Linq;
using SJCNet.Todo.Data.Uow.Contract;

namespace SJCNet.Todo.Web.Api
{
    [BreezeController]
    public class DataController : ApiController
    {
        private IBreezeTodoUow _uow = null;

        #region Constructors

        public DataController(IBreezeTodoUow uow)
        {
            _uow = uow;
        }

        #endregion

        #region Put Methods

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _uow.Commit(saveBundle);
        }

        #endregion

        #region Get Methods

        [Queryable]
        [HttpGet]
        public IQueryable<TodoItem> TodoItems()
        {
            _uow.TodoItems.EagerLoad(i => i.Priority);
            return _uow.TodoItems.GetAll();
        }

        [Queryable]
        [HttpGet]
        public IQueryable<TodoList> TodoLists()
        {
            return _uow.TodoLists.GetAll();
        }

        [Queryable]
        [HttpGet]
        public IQueryable<TodoPriority> TodoPriorities()
        {
            return _uow.TodoPriorities.GetAll();


        }

        #endregion
    }
}