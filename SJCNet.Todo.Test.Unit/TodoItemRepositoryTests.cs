using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SJCNet.Todo.Data.Uow;
using SJCNet.Todo.Test.Common.Database;
using SJCNet.Todo.Data;
using System.Data.Entity;
using SJCNet.Todo.Model;
using System.Linq;
using System.Collections.Generic;

namespace SJCNet.Todo.Test.Unit
{
    [TestClass]
    public class TodoItemRepositoryTests
    {
        private ITodoUow _uow1 = null;
        private ITodoUow _uow2 = null;
        private DateTime _sourceDate;

        #region Initialise and Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            // Create the database initializer & get any initial values
            var initializer = new TestDatabaseInitializer();
            _sourceDate = initializer.SourceDate;

            // Initialize the database
            var context = new TodoDbContext();
            Database.SetInitializer<TodoDbContext>(initializer);
            context.Database.Initialize(true);

            // Create the unit of work
            _uow1 = new TodoUow(context);
            _uow2 = new TodoUow(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (_uow1 != null)
            {
                _uow1.Dispose();
                _uow1 = null;
            }

            if (_uow2 != null)
            {
                _uow2.Dispose();
                _uow2 = null;
            }
        }

        #endregion

        [TestMethod]
        public void TodoItem_GetAll_Test()
        {
            // Arrange
            var expectedCount = 20;

            // Act
            var actualCount = _uow1.TodoItems.GetAll().Count();

            // Assert
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void TodoItem_GetById_Test()
        {
            // Arrange - Get expected by name
            var expected = _uow1.TodoItems.GetAll().SingleOrDefault(i => i.Description == "Todo Item 1 1");

            // Act
            _uow2.TodoItems.EagerLoad(i => i.Priority, i => i.List);
            var actual = _uow2.TodoItems.GetById(expected.Id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.DueDate, actual.DueDate);
            Assert.AreEqual(expected.Priority.Id, actual.Priority.Id);
            Assert.AreEqual(expected.Priority.Name, actual.Priority.Name);
            Assert.AreEqual(expected.Notes, actual.Notes);
            Assert.AreEqual(expected.Completed, actual.Completed);
            Assert.AreEqual(expected.CompletedDate, actual.CompletedDate);
        }

        [TestMethod]
        public void TodoItem_Add_Single_Test()
        {
            // Arrange
            var expected = new TodoItem
            {
                Description = "New Item 1",
                List = _uow2.TodoLists.GetById(1),
                DueDate = DateTime.Parse("2013-11-04 11:07:26"),
                Priority = _uow2.TodoPriorities.GetById((int)TodoPriorities.Medium),
                Notes = "Test Notes",
                Completed = false,
                CompletedDate = null,
                CreatedDate = DateTime.Now
            };

            // Act
            _uow1.TodoItems.Add(expected);
            _uow1.Commit();

            // Assert - Get the newly added item
            _uow2.TodoItems.EagerLoad(i => i.List, i => i.Priority);
            var actual = _uow2.TodoItems.GetById(expected.Id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.List.Id, actual.List.Id);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.DueDate, actual.DueDate);
            Assert.AreEqual(expected.Priority.Id, actual.Priority.Id);
            Assert.AreEqual(expected.Priority.Name, actual.Priority.Name);
            Assert.AreEqual(expected.Notes, actual.Notes);
            Assert.AreEqual(expected.Completed, actual.Completed);
            Assert.AreEqual(expected.CompletedDate, actual.CompletedDate);
        }

        [TestMethod]
        public void TodoItem_Add_Multiple_Test()
        {
            // Arrange
            var expected1 = new TodoItem
            {
                Description = "New Item 1",
                List = _uow2.TodoLists.GetById(1),
                DueDate = DateTime.Now.AddDays(-2),
                Priority = _uow2.TodoPriorities.GetById((int)TodoPriorities.Medium),
                Notes = "Test Notes",
                Completed = false,
                CompletedDate = null,
                CreatedDate = DateTime.Now
            };

            var expected2 = new TodoItem
            {
                Description = "New Item 2",
                List = _uow2.TodoLists.GetById(1),
                DueDate = DateTime.Now.AddDays(-2),
                Priority = _uow2.TodoPriorities.GetById((int)TodoPriorities.High),
                Notes = "Test Notes",
                Completed = true,
                CompletedDate = DateTime.Now.AddDays(-1),
                CreatedDate = DateTime.Now
            };

            var expectedList = new List<TodoItem> { expected1, expected2 };

            // Act
            _uow1.TodoItems.Add(expectedList);
            _uow1.Commit();

            // Assert - Get the newly added item
            _uow2.TodoItems.EagerLoad(i => i.List, i => i.Priority);
            var actual1 = _uow2.TodoItems.GetById(expected1.Id);
            var actual2 = _uow2.TodoItems.GetById(expected2.Id);

            // Assert - Check entity 1
            Assert.IsNotNull(actual1);
            Assert.AreEqual(expected1.Id, actual1.Id);
            Assert.AreEqual(expected1.List.Id, actual1.List.Id);
            Assert.AreEqual(expected1.Description, actual1.Description);
            Assert.AreEqual(expected1.DueDate, actual1.DueDate);
            Assert.AreEqual(expected1.Priority.Id, actual1.Priority.Id);
            Assert.AreEqual(expected1.Priority.Name, actual1.Priority.Name);
            Assert.AreEqual(expected1.Notes, actual1.Notes);
            Assert.AreEqual(expected1.Completed, actual1.Completed);
            Assert.AreEqual(expected1.CompletedDate, actual1.CompletedDate);

            // Assert - Check entity 2
            Assert.IsNotNull(actual2);
            Assert.AreEqual(expected2.Id, actual2.Id);
            Assert.AreEqual(expected2.List.Id, actual2.List.Id);
            Assert.AreEqual(expected2.Description, actual2.Description);
            Assert.AreEqual(expected2.DueDate, actual2.DueDate);
            Assert.AreEqual(expected2.Priority.Id, actual2.Priority.Id);
            Assert.AreEqual(expected2.Priority.Name, actual2.Priority.Name);
            Assert.AreEqual(expected2.Notes, actual2.Notes);
            Assert.AreEqual(expected2.Completed, actual2.Completed);
            Assert.AreEqual(expected2.CompletedDate, actual2.CompletedDate);
        }

        [TestMethod]
        public void TodoItem_Update_Single_Test()
        {
            // Arrange
            _uow1.TodoItems.EagerLoad(i => i.List, i => i.Priority);
            var expected1 = _uow1.TodoItems.GetById(1);

            // Act - Update entity
            expected1.Description = "Changed!";
            expected1.DueDate = DateTime.Now.AddDays(-1);
            expected1.Completed = true;
            expected1.CompletedDate = DateTime.Now;
            expected1.Notes = "Changed";
            expected1.Priority = _uow1.TodoPriorities.GetAll().SingleOrDefault(i => i.Id == (int)TodoPriorities.High);
            _uow2.TodoItems.Update(expected1);
            _uow2.Commit();

            // Assert - Get the updated entity
            _uow2.TodoItems.EagerLoad(i => i.List, i => i.Priority);
            var actual1 = _uow2.TodoItems.GetById(expected1.Id);
            Assert.IsNotNull(actual1);
            Assert.AreEqual(expected1.Id, actual1.Id);
            Assert.AreEqual(expected1.List.Id, actual1.List.Id);
            Assert.AreEqual(expected1.Description, actual1.Description);
            Assert.AreEqual(expected1.DueDate, actual1.DueDate);
            Assert.AreEqual(expected1.Priority.Id, actual1.Priority.Id);
            Assert.AreEqual(expected1.Priority.Name, actual1.Priority.Name);
            Assert.AreEqual(expected1.Notes, actual1.Notes);
            Assert.AreEqual(expected1.Completed, actual1.Completed);
            Assert.AreEqual(expected1.CompletedDate, actual1.CompletedDate);
        }

        [TestMethod]
        public void TodoItem_Update_Multiple_Test()
        {
            // Arrange - Get entities
            _uow1.TodoItems.EagerLoad(i => i.List, i => i.Priority);
            var expected1 = _uow1.TodoItems.GetById(1);
            var expected2 = _uow1.TodoItems.GetById(2);
            
            // Act - Change entities
            expected1.Description = "Changed!";
            expected1.DueDate = DateTime.Now.AddDays(-1);
            expected1.Completed = true;
            expected1.CompletedDate = DateTime.Now;
            expected1.Notes = "Changed";
            expected1.Priority = _uow1.TodoPriorities.GetAll().SingleOrDefault(i => i.Id == (int)TodoPriorities.High);

            expected2.Description = "Changed123!";
            expected2.DueDate = DateTime.Now.AddDays(-2);
            expected2.Completed = false;
            expected2.CompletedDate = DateTime.Now;
            expected2.Notes = "Test Changed";
            expected2.Priority = _uow1.TodoPriorities.GetAll().SingleOrDefault(i => i.Id == (int)TodoPriorities.Low);

            var expectedList = new List<TodoItem> { expected1, expected2 };

            // Act - Update entities
            _uow2.TodoItems.Update(expectedList);
            _uow2.Commit();

            // Assert - Get entities to check
            _uow2.TodoItems.EagerLoad(i => i.List, i => i.Priority);
            var actual1 = _uow2.TodoItems.GetById(expected1.Id);
            var actual2 = _uow2.TodoItems.GetById(expected2.Id);

            // Assert - Check entity 1
            Assert.IsNotNull(actual1);
            Assert.AreEqual(expected1.Id, actual1.Id);
            Assert.AreEqual(expected1.List.Id, actual1.List.Id);
            Assert.AreEqual(expected1.Description, actual1.Description);
            Assert.AreEqual(expected1.DueDate, actual1.DueDate);
            Assert.AreEqual(expected1.Priority.Id, actual1.Priority.Id);
            Assert.AreEqual(expected1.Priority.Name, actual1.Priority.Name);
            Assert.AreEqual(expected1.Notes, actual1.Notes);
            Assert.AreEqual(expected1.Completed, actual1.Completed);
            Assert.AreEqual(expected1.CompletedDate, actual1.CompletedDate);

            // Assert - Check entity 2
            Assert.IsNotNull(actual2);
            Assert.AreEqual(expected2.Id, actual2.Id);
            Assert.AreEqual(expected2.List.Id, actual2.List.Id);
            Assert.AreEqual(expected2.Description, actual2.Description);
            Assert.AreEqual(expected2.DueDate, actual2.DueDate);
            Assert.AreEqual(expected2.Priority.Id, actual2.Priority.Id);
            Assert.AreEqual(expected2.Priority.Name, actual2.Priority.Name);
            Assert.AreEqual(expected2.Notes, actual2.Notes);
            Assert.AreEqual(expected2.Completed, actual2.Completed);
            Assert.AreEqual(expected2.CompletedDate, actual2.CompletedDate);
        }

        [TestMethod]
        public void TodoItem_Delete_Single_By_Id_Test()
        {
            // Arrange
            var expectedId = 1;

            // Act
            _uow1.TodoItems.Delete(expectedId);
            _uow1.Commit();

            // Assert
            var actual = _uow2.TodoItems.GetById(expectedId);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TodoItem_Delete_Single_By_Entity_Test()
        {
            // Arrange
            _uow1.TodoItems.EagerLoad(i => i.List, i => i.Priority);
            var expected = _uow1.TodoItems.GetById(1);

            // Act
            _uow2.TodoItems.Delete(expected);
            _uow2.Commit();

            // Assert
            var actual = _uow1.TodoItems.GetById(expected.Id);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TodoItem_Delete_Multiple_Test()
        {
            // Arrange
            _uow1.TodoItems.EagerLoad(i => i.List, i => i.Priority);
            var expected1 = _uow1.TodoItems.GetById(1);
            var expected2 = _uow1.TodoItems.GetById(2);
            var expectedList = new List<TodoItem> { expected1, expected2 };

            // Act
            _uow2.TodoItems.Delete(expectedList);
            _uow2.Commit();

            // Assert
            var actual1 = _uow1.TodoItems.GetById(expected1.Id);
            var actual2 = _uow1.TodoItems.GetById(expected2.Id);
            Assert.IsNull(actual1);
            Assert.IsNull(actual2);
        }
    }
}
