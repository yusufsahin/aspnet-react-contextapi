using NoteAPIRP.BAL.Abstract;
using NoteAPIRP.DAL.Abstract;
using NoteAPIRP.Dto;
using NoteAPIRP.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteAPIRP.BAL.Concrete
{
    public class NoteManager : INoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteManager(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<IEnumerable<NoteDTO>> GetAllNotesAsync()
        {
            var notes = await _noteRepository.GetAllNotesAsync();
            return notes.Select(n => new NoteDTO { Id = n.Id, Name = n.Name, Description = n.Description }).ToList();
        }

        public async Task<NoteDTO> GetNoteByIdAsync(int id)
        {
            var note = await _noteRepository.GetNoteByIdAsync(id);
            return note != null ? new NoteDTO { Id = note.Id, Name = note.Name, Description = note.Description } : null;
        }

        // Fix here: Return Task<NoteDTO> instead of void
        public async Task<NoteDTO> AddNoteAsync(NoteDTO noteDto)
        {
            var note = new Note
            {
                Name = noteDto.Name,
                Description = noteDto.Description
            };

            // Save the note and get the auto-generated Id
           var savedNote = await _noteRepository.AddNoteAsync(note);

            // Return the created NoteDTO including the generated Id
            return new NoteDTO
            {
                Id = note.Id,   // Assign the auto-generated Id
                Name = note.Name,
                Description = note.Description
            };
        }

        public async Task UpdateNoteAsync(NoteDTO noteDto)
        {
            var note = await _noteRepository.GetNoteByIdAsync(noteDto.Id);
            if (note != null)
            {
                note.Name = noteDto.Name;
                note.Description = noteDto.Description;
                await _noteRepository.UpdateNoteAsync(note);
            }
        }

        public async Task DeleteNoteAsync(int id)
        {
            await _noteRepository.DeleteNoteAsync(id);
        }
    }
}
