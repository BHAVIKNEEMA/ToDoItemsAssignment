using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDo.Models;

namespace ToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles ="Admin,User")]
        [Route("GetAll", Name = "Get All ToDo Items")]
        //Status Codes Documentation
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public ActionResult<IEnumerable<TodoDTO>> GetAllItems()
        {
            var toDoItems = TodoRepository.toDoItems.Select(td => new TodoDTO()
            {
                Id = td.Id,
                ToDoItems = td.ToDoItems
            });

            //200 - Success
            return Ok(toDoItems);
        }

        [HttpGet("get/{id:int}", Name = "GetItemsById"),Authorize(Roles="Admin")]
        //Status codes documentation
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<TodoDTO> GetItemsById(int id)
        {
            if (id <= 0)
               return BadRequest();
            //Bad Request - 400 (Client error)

            var toDoItem = TodoRepository.toDoItems.Where(td => td.Id == id).FirstOrDefault();

            if(toDoItem == null)
            {
                return NotFound($"ToDoItem with id = {id} not found");
            }

            var toDoDTO = new TodoDTO()
            {
                Id = toDoItem.Id,
                ToDoItems = toDoItem.ToDoItems
            };
            return Ok(toDoDTO);
        }

        [HttpPut, Authorize(Roles = "Admin")]
        [Route("put/{id:int}", Name = "UpdateSingleToDoItem")]
        //Status Codes Documentation
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult UpdateSingleToDoItem(int id,TodoDTO todoDTO)
        {
            var existingToDoItems = TodoRepository.toDoItems.Where(td => td.Id == id).FirstOrDefault();
            if (existingToDoItems == null)
                return NotFound();

            existingToDoItems.ToDoItems= todoDTO.ToDoItems;
            return NoContent();
        }

        [HttpPost, Authorize(Roles = "Admin")]
        [Route("Create/{id:int}")]
        //Status Codes documentation
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<TodoDTO> CreateToDoItems(int id,[FromBody] TodoDTO todoDTO)
        {
            var toDoItem = TodoRepository.toDoItems.Where(td => td.Id==id).FirstOrDefault();
            if(toDoItem != null)
            {
                return BadRequest($"Please enter a different id. This id = {id} already exists");
            }
            Todo todo = new Todo()
            {
                Id = id,
                ToDoItems = todoDTO.ToDoItems
            };
            TodoRepository.toDoItems.Add(todo);
            todoDTO.Id = id;
            return CreatedAtRoute("GetItemsById", new { id = todoDTO.Id }, todoDTO);
        }

        [HttpDelete, Authorize(Roles = "Admin")]
        [Route("delete/{id:int}",Name ="DeleteItemsById")]
        //Status Codes Documentation
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<TodoDTO> DeleteItemsById(int id)
        {
            if (id <= 0)
                BadRequest();

            var existingItems = TodoRepository.toDoItems.Where(td => td.Id == id).FirstOrDefault();
            if (existingItems == null)
                return NotFound($"Item with id = {id} not found to be deleted");
            TodoRepository.toDoItems.Remove(existingItems);
            return Ok(existingItems);
        }
    }
}
