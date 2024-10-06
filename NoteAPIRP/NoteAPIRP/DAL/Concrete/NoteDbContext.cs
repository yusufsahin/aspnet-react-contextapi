using Microsoft.EntityFrameworkCore;
using NoteAPIRP.Models;

namespace NoteAPIRP.DAL.Concrete;
public class NoteDbContext : DbContext
{
    public NoteDbContext(DbContextOptions<NoteDbContext> options) : base(options) { }

    public DbSet<Note> Notes { get; set; }
}
