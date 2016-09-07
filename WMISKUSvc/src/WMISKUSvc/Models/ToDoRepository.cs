using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ToDoApi.Models
{
    public class ToDoRepository : IToDoRepository
    {
        private static ConcurrentDictionary<string, ToDoItem> _todos = new ConcurrentDictionary<string, ToDoItem>();

        public ToDoRepository()
        {
            Add(new ToDoItem { Name = "Item1" });
        }

        public IEnumerable<ToDoItem> GetAll()
        {
            return _todos.Values;
        }

        public void Add(ToDoItem item)
        {
            item.Key = Guid.NewGuid().ToString();
            _todos[item.Key] = item;
        }

        public ToDoItem Find(string key)
        {
            ToDoItem item;
            _todos.TryGetValue(key, out item);
            return item;
        }

        public ToDoItem Remove(string key)
        {
            ToDoItem item;
            _todos.TryRemove(key, out item);
            return item;
        }

        public void Update(ToDoItem item)
        {
            _todos[item.Key] = item; 
        }
    }
}
