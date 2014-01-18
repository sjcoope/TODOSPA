INSERT INTO TodoPriority(Id, Name)
SELECT 1, 'None' UNION ALL
SELECT 2, 'High' UNION ALL
SELECT 3, 'Low'

INSERT INTO TodoList(Name)
SELECT 'List 1' UNION ALL
SELECT 'List 2' UNION ALL
SELECT 'List 3' UNION ALL
SELECT 'List 4' UNION ALL
SELECT 'List 5'

INSERT INTO Todoitem(Description, DueDate, TodoListId, TodoPriorityId, Completed, CompletedDate)
SELECT 'Description 1 1', CAST('2013-11-04 11:00' as datetime), 1, 3, 0, NULL UNION ALL
SELECT 'Description 1 2', NULL, 1, 3, 0, NULL UNION ALL
SELECT 'Description 1 3', DateAdd(dd, 3, GetDate()), 1, 3, 0, NULL UNION ALL
SELECT 'Description 2 1', DateAdd(dd, 5, GetDate()), 2, 3, 0, NULL UNION ALL
SELECT 'Description 3 1', DateAdd(dd, 2, GetDate()), 3, 3, 0, NULL UNION ALL
SELECT 'Description 3 2', DateAdd(dd, 3, GetDate()), 3, 3, 1, GetDate() UNION ALL
SELECT 'Description 4 1', DateAdd(dd, 7, GetDate()), 4, 3, 0, NULL UNION ALL
SELECT 'Description 4 2', DateAdd(dd, 4, GetDate()), 4, 3, 0, NULL UNION ALL
SELECT 'Description 5 1', DateAdd(dd, 3, GetDate()), 5, 3, 0, NULL UNION ALL
SELECT 'Description 5 2', DateAdd(dd, 2, GetDate()), 5, 3, 0, NULL UNION ALL
SELECT 'Description 5 3', DateAdd(dd, 1, GetDate()), 5, 3, 0, NULL


