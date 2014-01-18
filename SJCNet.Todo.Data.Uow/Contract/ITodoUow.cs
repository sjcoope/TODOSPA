using SJCNet.Todo.Data.Repository.Contract;
using SJCNet.Todo.Model;
using System;
namespace SJCNet.Todo.Data.Uow
{
    public interface ITodoUow : IDisposable
    {
        bool Commit();

        IRepository<TodoItem> TodoItems { get; }

        IRepository<TodoList> TodoLists { get; }

        IRepository<TodoPriority> TodoPriorities { get; }
    }
}
