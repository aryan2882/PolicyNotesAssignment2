using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Data;
using PolicyNotesService.Models;
using PolicyNotesService.Repositories;
using PolicyNotesService.Services;
using Xunit;

namespace PolicyNotesService.Tests.UnitTests;

public class PolicyNotesService_NoMoqTests
{
    private PolicyNotesService.Services.PolicyNoteService CreateService()
    {
        var options = new DbContextOptionsBuilder<NotesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        var db = new NotesDbContext(options);
        var repo = new PolicyNotesRepository(db);
        return new PolicyNotesService.Services.PolicyNoteService(repo);
    }

    [Fact]
    public async Task AddNote_ShouldStoreInDatabase()
    {
        var service = CreateService();

        var note = await service.AddNoteAsync("POL500", "Stored note");

        Assert.NotNull(note);
        Assert.True(note.Id > 0);
        Assert.Equal("POL500", note.PolicyNumber);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllStoredNotes()
    {
        var service = CreateService();

        await service.AddNoteAsync("POL111", "Note 1");
        await service.AddNoteAsync("POL222", "Note 2");
        await service.AddNoteAsync("POL333", "Note 3");

        var allNotes = await service.GetAllAsync();

        Assert.NotNull(allNotes);
        Assert.Equal(3, allNotes.Count);
    }

    [Fact]
    public async Task GetById_ShouldReturnStoredNote()
    {
        var service = CreateService();
        var created = await service.AddNoteAsync("POL777", "Hello");

        var result = await service.GetByIdAsync(created.Id);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result!.Id);
    }

    [Fact]
    public async Task GetById_WhenMissing_ShouldReturnNull()
    {
        var service = CreateService();

        var result = await service.GetByIdAsync(999);

        Assert.Null(result);
    }
}
