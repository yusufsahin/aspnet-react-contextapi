using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteApiEF.Data;
using NoteApiEF.Dto;
using NoteApiEF.Models;

namespace NoteApiEF.Controllers;
[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly AppDbContext _context;

    public NotesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/notes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotes()
    {
        var notes = await _context.Notes
            .Select(n => new NoteDto
            {
                Id = n.Id,
                Name = n.Name,
                Description = n.Description
            })
            .ToListAsync();

        return Ok(notes);
    }

    // GET: api/notes/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<NoteDto>> GetNoteById(int id)
    {
        var note = await _context.Notes
            .Where(n => n.Id == id)
            .Select(n => new NoteDto
            {
                Id = n.Id,
                Name = n.Name,
                Description = n.Description
            })
            .FirstOrDefaultAsync();

        if (note == null)
        {
            return NotFound();
        }

        return Ok(note);
    }

    // POST: api/notes
    [HttpPost]
    public async Task<ActionResult<NoteDto>> CreateNote([FromBody] NoteDto newNoteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var note = new Note
        {
            Name = newNoteDto.Name,
            Description = newNoteDto.Description
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        // Return the created note with its new Id
        newNoteDto.Id = note.Id;

        return CreatedAtAction(nameof(GetNoteById), new { id = newNoteDto.Id }, newNoteDto);
    }

    // PUT: api/notes/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(int id, [FromBody] NoteDto updatedNoteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != updatedNoteDto.Id)
        {
            return BadRequest();
        }

        var note = await _context.Notes.FindAsync(id);
        if (note == null)
        {
            return NotFound();
        }

        note.Name = updatedNoteDto.Name;
        note.Description = updatedNoteDto.Description;

        _context.Entry(note).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Notes.Any(n => n.Id == id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // DELETE: api/notes/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNote(int id)
    {
        var note = await _context.Notes.FindAsync(id);
        if (note == null)
        {
            return NotFound();
        }

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
/*
[ApiController]
[Route("api/[controller]")]
public class NotesController:ControllerBase
{
   
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            return await _context.Notes.ToListAsync();
            
            
        }

        // GET: api/notes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNoteById(int id)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            return note;
        }

        // POST: api/notes
        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote([FromBody] Note newNote)
        {
            _context.Notes.Add(newNote);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNoteById), new { id = newNote.Id }, newNote);
        }

        // PUT: api/notes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] Note updatedNote)
        {
            if (id != updatedNote.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedNote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Notes.Any(n => n.Id == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/notes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
*/