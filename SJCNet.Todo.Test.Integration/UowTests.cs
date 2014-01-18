using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using SJCNet.Todo.Data;
using System.Data.Entity;
using SJCNet.Todo.Test.Common.Database;
using SJCNet.Todo.Data.Uow;
using System.Linq;
using SJCNet.Todo.Model;
using System.Collections.Generic;

namespace SJCNet.Todo.Test.Integration
{
    // TODO: Update tests
    [TestClass]
    [DeploymentItem(@"Artifacts")]
    public class UowTests
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

            if(_uow2 != null)
            {
                _uow2.Dispose();
                _uow2 = null;
            }
        }

        #endregion

        #region TodoList Tests

        [TestMethod]
        public void TodoList_GetAll_Test()
        {
            // Arrange
            var expectedCount = 4;

            // Act
            _uow1.TodoLists.EagerLoad(i => i.Items);
            var actualList = _uow1.TodoLists.GetAll();

            // Assert
            Assert.AreEqual(expectedCount, actualList.Count());
        }

        [TestMethod]
        public void TodoList_GetById_Test()
        {
            // Arrange
            var expectedId = 1;

            // Act
            var actual = _uow1.TodoLists.GetById(expectedId);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedId, actual.Id);
        }

        [TestMethod]
        public void TodoList_Add_Single_Test()
        {
            // Arrange
            var expected = new TodoList
            {
                Name = "New Todo List",
                Items = new List<TodoItem>
                {
                    new TodoItem { Description = "New Todo Item 1" },
                    new TodoItem { Description = "New Todo Item 2" },
                    new TodoItem { Description = "New Todo Item 3" }
                }
            };

            // Act
            _uow1.TodoLists.Add(expected);

            // Assert
            var actual = _uow2.TodoLists.GetById(expected.Id);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Items.Count, actual.Items.Count);
        }

        [TestMethod]
        public void TodoList_Add_Multiple_Test()
        {
            // Arrange
            var expected1 = new TodoList
            {
                Name = "New Todo List 1",
                Items = new List<TodoItem>
                {
                    new TodoItem { Description = "New Todo Item 1 1", Priority = _uow2.TodoPriorities.GetById((int)TodoPriorities.High), CreatedDate = DateTime.Now },
                    new TodoItem { Description = "New Todo Item 1 2", Priority = _uow2.TodoPriorities.GetById((int)TodoPriorities.Medium), CreatedDate = DateTime.Now },
                    new TodoItem { Description = "New Todo Item 1 3", Priority = _uow2.TodoPriorities.GetById((int)TodoPriorities.Low), CreatedDate = DateTime.Now }
                }
            };

            var expected2 = new TodoList
            {
                Name = "New Todo List 2",
                Items = new List<TodoItem>
                {
                    new TodoItem { Description = "New Todo Item 2 1", Priority = _uow2.TodoPriorities.GetById((int)TodoPriorities.High), CreatedDate = DateTime.Now },
                    new TodoItem { Description = "New Todo Item 2 2", Priority = _uow2.TodoPriorities.GetById((int)TodoPriorities.Medium), CreatedDate = DateTime.Now },
                    new TodoItem { Description = "New Todo Item 2 3", Priority = _uow2.TodoPriorities.GetById((int)TodoPriorities.Low), CreatedDate = DateTime.Now }
                }
            };

            var expectedList = new List<TodoList>();
            expectedList.Add(expected1);
            expectedList.Add(expected2);

            // Act
            _uow1.TodoLists.Add(expectedList);
            _uow1.Commit();

            // Assert
            var actual1 = _uow2.TodoLists.GetById(expected1.Id);
            Assert.IsNotNull(actual1);
            Assert.AreEqual(expected1.Id, actual1.Id);
            Assert.AreEqual(expected1.Name, actual1.Name);
            Assert.AreEqual(expected1.Items.Count, actual1.Items.Count);

            var actual2 = _uow2.TodoLists.GetById(expected2.Id);
            Assert.IsNotNull(actual2);
            Assert.AreEqual(expected2.Id, actual2.Id);
            Assert.AreEqual(expected2.Name, actual2.Name);
            Assert.AreEqual(expected2.Items.Count, actual2.Items.Count);
        }

        [TestMethod]
        public void TodoList_Update_Single_Test()
        {
            // Arrange
            var expected = _uow1.TodoLists.GetById(1);
            expected.Name = "Changed";

            // Act
            _uow1.TodoLists.Update(expected);
            _uow1.Commit();

            // Assert
            _uow2.TodoLists.EagerLoad(i => i.Items);
            var actual = _uow2.TodoLists.GetById(expected.Id);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Items.Count, actual.Items.Count);
        }

        [TestMethod]
        public void TodoList_Update_Multiple_Test()
        {
            /// Arrange
            var expected1 = _uow1.TodoLists.GetById(1);
            expected1.Name = "Changed1";

            var expected2 = _uow2.TodoLists.GetById(2);
            expected2.Name = "Changed2";

            var expectedList = new List<TodoList>();
            expectedList.Add(expected1);
            expectedList.Add(expected2);

            // Act
            _uow1.TodoLists.Update(expectedList);
            _uow1.Commit();

            // Assert
            _uow2.TodoLists.EagerLoad(i => i.Items);
            var actual1 = _uow2.TodoLists.GetById(expected1.Id);
            Assert.IsNotNull(actual1);
            Assert.AreEqual(expected1.Id, actual1.Id);
            Assert.AreEqual(expected1.Name, actual1.Name);
            Assert.AreEqual(expected1.Items.Count, actual1.Items.Count);

            _uow2.TodoLists.EagerLoad(i => i.Items);
            var actual2 = _uow2.TodoLists.GetById(expected2.Id);
            Assert.IsNotNull(actual2);
            Assert.AreEqual(expected2.Id, actual2.Id);
            Assert.AreEqual(expected2.Name, actual2.Name);
            Assert.AreEqual(expected2.Items.Count, actual2.Items.Count);
        }

        [TestMethod]
        public void TodoList_Delete_Single_By_Id_Test()
        {
            // Arrange
            var expectedId = 1;

            // Act
            _uow1.TodoLists.Delete(expectedId);
            _uow1.Commit();

            // Assert
            var actual = _uow2.TodoLists.GetById(expectedId);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TodoList_Delete_Single_By_Entity_Test()
        {
            // Arrange
            var expected = _uow1.TodoLists.GetById(1);

            // Act
            _uow1.TodoLists.Delete(expected);
            _uow1.Commit();

            // Assert
            var actual = _uow2.TodoLists.GetById(expected.Id);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TodoList_Delete_Multiple_Test()
        {
            // Arrange
            var expected1 = _uow1.TodoLists.GetById(1);
            var expected2 = _uow1.TodoLists.GetById(2);
            var expectedList = new List<TodoList>();
            expectedList.Add(expected1);
            expectedList.Add(expected2);

            // Act
            _uow1.TodoLists.Delete(expectedList);
            _uow1.Commit();

            // Assert
            var actual1 = _uow2.TodoLists.GetById(expected1.Id);
            Assert.IsNull(actual1);

            var actual2 = _uow2.TodoLists.GetById(expected2.Id);
            Assert.IsNull(actual2);
        }

        #endregion

        #region TodoItem Tests

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
            // Arrange
            var expected = new TodoItem
            {
                Description = "Todo Item",
                DueDate = _sourceDate.AddDays(5),
                Priority = _uow2.TodoPriorities.GetById((int)TodoPriorities.High),
                Notes = null,
                Completed = false,
                CompletedDate = null
            };

            // Arrange - Add new item
            var list = _uow1.TodoLists.GetById(1);
            list.Items.Add(expected);
            _uow1.Commit();

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

            // Act

            // Assert
            Assert.Inconclusive("Not created");
        }

        [TestMethod]
        public void TodoItem_Update_Single_Test()
        {
            // Arrange

            // Act

            // Assert
            Assert.Inconclusive("Not created");
        }

        [TestMethod]
        public void TodoItem_Update_Multiple_Test()
        {
            // Arrange

            // Act

            // Assert
            Assert.Inconclusive("Not created");
        }

        [TestMethod]
        public void TodoItem_Delete_Single_By_Id_Test()
        {
            // Arrange

            // Act

            // Assert
            Assert.Inconclusive("Not created");
        }

        [TestMethod]
        public void TodoItem_Delete_Single_By_Entity_Test()
        {
            // Arrange

            // Act

            // Assert
            Assert.Inconclusive("Not created");
        }

        [TestMethod]
        public void TodoItem_Delete_Multiple_Test()
        {
            // Arrange

            // Act

            // Assert
            Assert.Inconclusive("Not created");
        }

        #endregion
    }
}
