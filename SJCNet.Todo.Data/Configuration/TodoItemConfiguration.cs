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
    public class TodoItemConfiguration : EntityTypeConfiguration<TodoItem>
    {
        public TodoItemConfiguration()
        {
            HasKey(i => i.Id);
            
            Property(i => i.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(c => c.Priority)
                .WithMany()
                .HasForeignKey(c => c.TodoPriorityId)
                .WillCascadeOnDelete(true);
        }
    }
}
