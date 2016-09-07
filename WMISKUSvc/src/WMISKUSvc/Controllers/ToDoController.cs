using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Models;
using System.Data.Common;
using System.Data;

namespace GameStop.SupplyChain.Services.WMISKUSvc.Controllers
{
    [Route("api/[controller]")]
    public class ToDoController : Controller
    {
        public ToDoController(IToDoRepository todoItems)
        {



            DBTest test = new DBTest();
            string ctn = test.GetCarton();

            ToDoItems = todoItems;
        }
        public IToDoRepository ToDoItems { get; set; }

        public IEnumerable<ToDoItem> GetAll()
        {
            return ToDoItems.GetAll();
        }

        [HttpGet("{id}", Name = "GetToDo")]
        public IActionResult GetById(string id)
        {
            var item = ToDoItems.Find(id);
            if (TempData == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ToDoItem item)
        {
            if(item == null)
            {
                return BadRequest();
            }
            ToDoItems.Add(item);
            return CreatedAtRoute("GetTodo", new { id = item.Key }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] ToDoItem item)
        {
            if (item == null || item.Key != id)
            {
                return BadRequest();
            }

            var todo = ToDoItems.Find(id);
            if(todo == null)
            {
                return NotFound();
            }

            ToDoItems.Update(item);
            return new NoContentResult();
        }

        [HttpPatch("{id}")]
        public IActionResult Update([FromBody] ToDoItem item, string id)
        {
            if(item == null)
            {
                return BadRequest();
            }

            var todo = ToDoItems.Find(id);
            if(todo == null)
            {
                return NotFound();
            }

            item.Key = todo.Key;

            ToDoItems.Update(item);
            return new NoContentResult();
        }
    }
}
