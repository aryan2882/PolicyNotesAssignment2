using Moq;
using PolicyNotesService.Models;
using PolicyNotesService.Repositories;
using PolicyNotesService.Services;
using Xunit;

namespace PolicyNotesService.Tests.UnitTests;

public class PolicyNotesService_WithMoq
{
    [Fact]
    public async Task AddNote_ShouldReturnCreatedNote()
    {
        var mockRepo = new Mock<IPolicyNotesRepository>();

        mockRepo
            .Setup(r => r.AddAsync(It.IsAny<PolicyNote>(), default))
            .ReturnsAsync((PolicyNote note, CancellationToken _) =>
            {
                note.Id = 1;
                return note;
            });

        var service = new PolicyNotesService.Services.PolicyNoteService(mockRepo.Object);

        var createdNote = await service.AddNoteAsync("POL123", "Test note");

        Assert.NotNull(createdNote);
        Assert.Equal(1, createdNote.Id);
        Assert.Equal("POL123", createdNote.PolicyNumber);
        Assert.Equal("Test note", createdNote.Note);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllNotes()
    {
        var mockRepo = new Mock<IPolicyNotesRepository>();

        var notesList = new List<PolicyNote>
        {
            new PolicyNote { Id = 1, PolicyNumber = "POL1", Note = "Note 1" },
            new PolicyNote { Id = 2, PolicyNumber = "POL2", Note = "Note 2" },
            new PolicyNote { Id = 3, PolicyNumber = "POL3", Note = "Note 3" }
        };

        mockRepo
            .Setup(r => r.GetAllAsync(default))
            .ReturnsAsync(notesList);

        var service = new PolicyNotesService.Services.PolicyNoteService(mockRepo.Object);

        var result = await service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetById_WhenFound_ShouldReturnNote()
    {
        var note = new PolicyNote
        {
            Id = 10,
            PolicyNumber = "POL10",
            Note = "Sample"
        };

        var mockRepo = new Mock<IPolicyNotesRepository>();
        mockRepo
            .Setup(r => r.GetByIdAsync(10, default))
            .ReturnsAsync(note);

        var service = new PolicyNotesService.Services.PolicyNoteService(mockRepo.Object);

        var result = await service.GetByIdAsync(10);

        Assert.NotNull(result);
        Assert.Equal(10, result.Id);
        Assert.Equal("POL10", result.PolicyNumber);
    }

    [Fact]
    public async Task GetById_WhenMissing_ShouldReturnNull()
    {
        var mockRepo = new Mock<IPolicyNotesRepository>();
        mockRepo
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync((PolicyNote?)null);

        var service = new PolicyNotesService.Services.PolicyNoteService(mockRepo.Object);

        var result = await service.GetByIdAsync(999);

        Assert.Null(result);
    }
}
