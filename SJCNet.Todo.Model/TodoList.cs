using SJCNet.Todo.Model.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SJCNet.Todo.Model
{
    public class TodoList : IEntity
    {
        public TodoList()
        {
            this.Items = new List<TodoItem>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<TodoItem> Items { get; set; }
    }
}
