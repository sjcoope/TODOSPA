using SJCNet.Todo.Model.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SJCNet.Todo.Model
{
    public class TodoItem : IEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Notes { get; set; }

        public DateTime? DueDate { get; set; }

        public TodoList List { get; set; }

        public int? TodoListId { get; set; }

        public TodoPriority Priority { get; set; }

        public int? TodoPriorityId { get; set; }

        public bool Completed { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
