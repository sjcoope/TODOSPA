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
    public class TodoListConfiguration : EntityTypeConfiguration<TodoList>
    {
        public TodoListConfiguration()
        {
            HasKey(i => i.Id);

            Property(i => i.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasMany(c => c.Items)
                .WithOptional(c => c.List)
                .HasForeignKey(c => c.TodoListId)
                .WillCascadeOnDelete(true);
        }
    }
}
