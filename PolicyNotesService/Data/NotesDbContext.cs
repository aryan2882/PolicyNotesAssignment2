using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Models;

namespace PolicyNotesService.Data;

public class NotesDbContext : DbContext
{
    public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }
    public DbSet<PolicyNote> PolicyNotes => Set<PolicyNote>();
}
