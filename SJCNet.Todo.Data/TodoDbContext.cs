using SJCNet.Todo.Data.Configuration;
using SJCNet.Todo.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCNet.Todo.Data
{
    public class TodoDbContext : DbContext
    {
        #region Constructors

        public TodoDbContext() : base(ConfigurationManager.ConnectionStrings["TodoDb"].ConnectionString)
        {
            // Recreate the database and initialize the data.
            Database.SetInitializer<TodoDbContext>(new TodoDatabaseInitializer());

            // Set-up config.
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
        }

        #endregion

        #region Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Configure entity configurations (navigation properties, etc).
            modelBuilder.Configurations.Add(new TodoListConfiguration());            
            modelBuilder.Configurations.Add(new TodoItemConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Properties

        public DbSet<TodoList> TodoLists { get; set; }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<TodoPriority> TodoPriorities { get; set; }

        #endregion
    }
}