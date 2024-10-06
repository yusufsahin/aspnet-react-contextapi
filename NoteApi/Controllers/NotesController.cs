using Microsoft.AspNetCore.Mvc;
using NoteApi.models;

namespace NoteApi.Controllers;

[ApiController]
[Route("[controller]")]
public class NotesController:ControllerBase
{
    private static List<Note> _notes = new List<Note>
    {
        new Note { Id = 1, Name = "Sample Note 1", Description = "This is the first note" },
        new Note { Id = 2, Name = "Sample Note 2", Description = "This is the second note" }
    };

    // GET: /notes
    [HttpGet]
    public ActionResult<List<Note>> GetNotes()
    {
        return Ok(_notes);
    }

    // GET: /notes/{id}
    [HttpGet("{id}")]
    public ActionResult<Note> GetNoteById(int id)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        if (note == null)
        {
            return NotFound();
        }
        return Ok(note);
    }

    // POST: /notes
    [HttpPost]
    public ActionResult CreateNote([FromBody] Note newNote)
    {
        newNote.Id = _notes.Max(n => n.Id) + 1; // Auto-increment Id
        _notes.Add(newNote);
        return CreatedAtAction(nameof(GetNoteById), new { id = newNote.Id }, newNote);
    }

    // PUT: /notes/{id}
    [HttpPut("{id}")]
    public ActionResult UpdateNote(int id, [FromBody] Note updatedNote)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        if (note == null)
        {
            return NotFound();
        }

        note.Name = updatedNote.Name;
        note.Description = updatedNote.Description;

        return NoContent();
    }

    // DELETE: /notes/{id}
    [HttpDelete("{id}")]
    public ActionResult DeleteNote(int id)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        if (note == null)
        {
            return NotFound();
        }

        _notes.Remove(note);
        return NoContent();
    }
}
    