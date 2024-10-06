using NoteAPIRP.Models;

namespace NoteAPIRP.DAL.Abstract;


public interface INoteRepository
{
    Task<IEnumerable<Note>> GetAllNotesAsync();
    Task<Note> GetNoteByIdAsync(int id);
    Task<Note> AddNoteAsync(Note note);
    Task UpdateNoteAsync(Note note);
    Task DeleteNoteAsync(int id);
}
