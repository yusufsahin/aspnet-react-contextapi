using System.ComponentModel.DataAnnotations;

namespace NoteApiEF.Dto
{
    public class NoteDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters.")]
        public string Description { get; set; }
    }
}
