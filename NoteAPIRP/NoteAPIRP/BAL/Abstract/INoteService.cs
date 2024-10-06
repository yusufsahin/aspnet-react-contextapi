using NoteAPIRP.Dto;

namespace NoteAPIRP.BAL.Abstract;

public interface INoteService
{
    Task<IEnumerable<NoteDTO>> GetAllNotesAsync();
    Task<NoteDTO> GetNoteByIdAsync(int id);
    Task<NoteDTO> AddNoteAsync(NoteDTO noteDto);
    Task UpdateNoteAsync(NoteDTO noteDto);
    Task DeleteNoteAsync(int id); 
}