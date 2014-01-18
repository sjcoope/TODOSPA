using SJCNet.Todo.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCNet.Todo.Data.Configuration
{
    public class TodoPriorityConfiguration : EntityTypeConfiguration<TodoPriority>
    {
        public TodoPriorityConfiguration()
        {
            HasKey(i => i.Id);
        }
    }
}
