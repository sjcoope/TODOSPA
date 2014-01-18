using SJCNet.Todo.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCNet.Todo.Data
{
    public class TodoDatabaseInitializer : DropCreateDatabaseAlways<TodoDbContext>
    {
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
            list1.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityHigh, CreatedDate = DateTime.Now });
            list1.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityLow, CreatedDate = DateTime.Now });
            list1.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityMedium, CreatedDate = DateTime.Now, Completed = true, CompletedDate = DateTime.Now.AddDays(-5) });
            list1.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityLow, CreatedDate = DateTime.Now, Notes = "Test Notes" });
            list1.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityLow, CreatedDate = DateTime.Now });
            context.TodoLists.Add(list1);

            var list2 = new TodoList { Name = "Todo List 2", Items = new List<TodoItem>() };
            list2.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityHigh, CreatedDate = DateTime.Now });
            list2.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityLow, CreatedDate = DateTime.Now });
            list2.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityMedium, CreatedDate = DateTime.Now, Completed = true, CompletedDate = DateTime.Now.AddDays(-5) });
            list2.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityLow, CreatedDate = DateTime.Now, Notes = "Test Notes" });
            list2.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityLow, CreatedDate = DateTime.Now });
            context.TodoLists.Add(list2);

            var list3 = new TodoList { Name = "Todo List 3", Items = new List<TodoItem>() };
            list3.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityHigh, CreatedDate = DateTime.Now });
            list3.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityLow, CreatedDate = DateTime.Now });
            list3.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityMedium, CreatedDate = DateTime.Now, Completed = true, CompletedDate = DateTime.Now.AddDays(-5) });
            list3.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityLow, CreatedDate = DateTime.Now, Notes = "Test Notes" });
            list3.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityLow, CreatedDate = DateTime.Now });
            context.TodoLists.Add(list3);

            var list4 = new TodoList { Name = "Todo List 4", Items = new List<TodoItem>() };
            list4.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityHigh, CreatedDate = DateTime.Now });
            list4.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityLow, CreatedDate = DateTime.Now });
            list4.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityMedium, CreatedDate = DateTime.Now, Completed = true, CompletedDate = DateTime.Now.AddDays(-5) });
            list4.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityLow, CreatedDate = DateTime.Now, Notes = "Test Notes" });
            list4.Items.Add(new TodoItem { Description = "Todo Item", Priority = priorityLow, CreatedDate = DateTime.Now });
            context.TodoLists.Add(list4);

            base.Seed(context);
        }
    }
}
