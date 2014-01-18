using SJCNet.Todo.Model.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCNet.Todo.Model
{
    public class TodoPriority : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
