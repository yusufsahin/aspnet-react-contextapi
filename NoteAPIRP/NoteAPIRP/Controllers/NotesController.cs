using Microsoft.AspNetCore.Mvc;
using NoteAPIRP.BAL.Abstract;
using NoteAPIRP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteAPIRP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        
        // GET: api/note
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDTO>>> GetAllNotes()
        {
            var notes = await _noteService.GetAllNotesAsync();
            return Ok(notes);
        }

        // GET: api/note/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDTO>> GetNoteById(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        // POST: api/note
        [HttpPost]
        public async Task<ActionResult> CreateNote([FromBody] NoteDTO noteDto)
        {
            if (noteDto == null)
            {
                return BadRequest();
            }

            await _noteService.AddNoteAsync(noteDto);
            return CreatedAtAction(nameof(GetNoteById), new { id = noteDto.Id }, noteDto);
        }

        // PUT: api/note/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateNote(int id, [FromBody] NoteDTO noteDto)
        {
            if (id != noteDto.Id)
            {
                return BadRequest("Note ID mismatch.");
            }

            var existingNote = await _noteService.GetNoteByIdAsync(id);
            if (existingNote == null)
            {
                return NotFound();
            }

            await _noteService.UpdateNoteAsync(noteDto);
            return NoContent();
        }

        // DELETE: api/note/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            await _noteService.DeleteNoteAsync(id);
            return NoContent();
        }
    }
}
