using SJCNet.Todo.Data;
using SJCNet.Todo.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCNet.Todo.Test.Common.Database
{
    public class TestDatabaseInitializer : DropCreateDatabaseAlways<TodoDbContext>
    {
        public TestDatabaseInitializer()
        {
            this.SourceDate = DateTime.Now.AddDays(-5);
        }

        protected override void Seed(TodoDbContext context)
        {
            // Add Priorities
            var priorityNone = new TodoPriority { Id = (int)TodoPriorities.None, Name = TodoPriorities.None.ToString() };
            var priorityLow = new TodoPriority { Id = (int)TodoPriorities.Low, Name = TodoPriorities.Low.ToString() };
            var priorityMedium = new TodoPriority { Id = (int)TodoPriorities.Medium, Name = TodoPriorities.Medium.ToString() };
            var priorityHigh = new TodoPriority { Id = (int)TodoPriorities.High, Name = TodoPriorities.High.ToString() };
            context.TodoPriorities.Add(priorityNone);
            context.TodoPriorities.Add(priorityLow);
            context.TodoPriorities.Add(priorityMedium);
            context.TodoPriorities.Add(priorityHigh);

            // Add Lists and Items
            var list1 = new TodoList { Name = "Todo List 1", Items = new List<TodoItem>() };
            list1.Items.Add(new TodoItem { Description = "Todo Item 1 1", Priority = priorityHigh, CreatedDate = SourceDate, DueDate = SourceDate.AddDays(3) });
            list1.Items.Add(new TodoItem { Description = "Todo Item 1 2", Priority = priorityLow, CreatedDate = SourceDate });
            list1.Items.Add(new TodoItem { Description = "Todo Item 1 3", Priority = priorityMedium, CreatedDate = SourceDate, Completed = true, CompletedDate = SourceDate.AddDays(2) });
            list1.Items.Add(new TodoItem { Description = "Todo Item 1 4", Priority = priorityLow, CreatedDate = SourceDate, Notes = "Test Notes" });
            list1.Items.Add(new TodoItem { Description = "Todo Item 1 5", Priority = priorityLow, CreatedDate = SourceDate });
            context.TodoLists.Add(list1);

            var list2 = new TodoList { Name = "Todo List 2", Items = new List<TodoItem>() };
            list2.Items.Add(new TodoItem { Description = "Todo Item 2 1", Priority = priorityHigh, CreatedDate = SourceDate, DueDate = SourceDate.AddDays(5) });
            list2.Items.Add(new TodoItem { Description = "Todo Item 2 2", Priority = priorityLow, CreatedDate = SourceDate });
            list2.Items.Add(new TodoItem { Description = "Todo Item 2 3", Priority = priorityMedium, CreatedDate = SourceDate, Completed = true, CompletedDate = SourceDate.AddDays(2) });
            list2.Items.Add(new TodoItem { Description = "Todo Item 2 4", Priority = priorityLow, CreatedDate = SourceDate, Notes = "Test Notes" });
            list2.Items.Add(new TodoItem { Description = "Todo Item 2 5", Priority = priorityLow, CreatedDate = SourceDate });
            context.TodoLists.Add(list2);

            var list3 = new TodoList { Name = "Todo List 3", Items = new List<TodoItem>() };
            list3.Items.Add(new TodoItem { Description = "Todo Item 3 1", Priority = priorityHigh, CreatedDate = SourceDate, DueDate = SourceDate.AddDays(5) });
            list3.Items.Add(new TodoItem { Description = "Todo Item 3 2", Priority = priorityLow, CreatedDate = SourceDate });
            list3.Items.Add(new TodoItem { Description = "Todo Item 3 3", Priority = priorityMedium, CreatedDate = SourceDate, Completed = true, CompletedDate = SourceDate.AddDays(2) });
            list3.Items.Add(new TodoItem { Description = "Todo Item 3 4", Priority = priorityLow, CreatedDate = SourceDate, Notes = "Test Notes" });
            list3.Items.Add(new TodoItem { Description = "Todo Item 3 5", Priority = priorityLow, CreatedDate = SourceDate });
            context.TodoLists.Add(list3);

            var list4 = new TodoList { Name = "Todo List 4", Items = new List<TodoItem>() };
            list4.Items.Add(new TodoItem { Description = "Todo Item 4 1", Priority = priorityHigh, CreatedDate = SourceDate, DueDate = SourceDate.AddDays(5) });
            list4.Items.Add(new TodoItem { Description = "Todo Item 4 2", Priority = priorityLow, CreatedDate = SourceDate });
            list4.Items.Add(new TodoItem { Description = "Todo Item 4 3", Priority = priorityMedium, CreatedDate = SourceDate, Completed = true, CompletedDate = SourceDate.AddDays(2) });
            list4.Items.Add(new TodoItem { Description = "Todo Item 4 4", Priority = priorityLow, CreatedDate = SourceDate, Notes = "Test Notes" });
            list4.Items.Add(new TodoItem { Description = "Todo Item 4 5", Priority = priorityLow, CreatedDate = SourceDate });
            context.TodoLists.Add(list4);

            base.Seed(context);
        }

        public DateTime SourceDate { get; set; }
    }
}
