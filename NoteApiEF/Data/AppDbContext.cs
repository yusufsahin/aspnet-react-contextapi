using Microsoft.EntityFrameworkCore;
using NoteApiEF.Models;

namespace NoteApiEF.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Note> Notes { get; set; }
}