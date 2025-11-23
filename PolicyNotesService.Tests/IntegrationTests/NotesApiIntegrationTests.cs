using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using PolicyNotesService.DTOs;
using PolicyNotesService.Models;
using Xunit;

namespace PolicyNotesService.IntegrationTests;

public class NotesApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public NotesApiIntegrationTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task PostNotes_ReturnsCreated()
    {
        var client = _factory.CreateClient();
        var dto = new CreateNoteDto("POL-INT-1", "integration note");

        var res = await client.PostAsJsonAsync("/notes", dto);

        Assert.Equal(HttpStatusCode.Created, res.StatusCode);

        var created = await res.Content.ReadFromJsonAsync<PolicyNote>();
        Assert.NotNull(created);
        Assert.Equal("POL-INT-1", created!.PolicyNumber);
    }

    [Fact]
    public async Task GetNotes_ReturnsOk()
    {
        var client = _factory.CreateClient();


        var get = await client.GetAsync("/notes");
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);

        var notes = await get.Content.ReadFromJsonAsync<List<PolicyNote>>();
        Assert.NotNull(notes);
        Assert.True(notes!.Count >= 1);
    }

    [Fact]
    public async Task GetById_Returns200_WhenFound()
    {
        var client = _factory.CreateClient();

        var dto = new CreateNoteDto("POL-INT-3", "third note");
        var post = await client.PostAsJsonAsync("/notes", dto);
        post.EnsureSuccessStatusCode();
        var created = await post.Content.ReadFromJsonAsync<PolicyNote>();
        Assert.NotNull(created);

        var found = await client.GetAsync($"/notes/{created!.Id}");
        Assert.Equal(HttpStatusCode.OK, found.StatusCode);

    }



    [Fact]
    public async Task GetById_Returns404_WhenMissing()
    {
        var client = _factory.CreateClient();

        var dto = new CreateNoteDto("POL-INT-4", "fourth note");
        var post = await client.PostAsJsonAsync("/notes", dto);
        post.EnsureSuccessStatusCode();
        var created = await post.Content.ReadFromJsonAsync<PolicyNote>();
        Assert.NotNull(created);

       
        var missing = await client.GetAsync("/notes/999999");
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }
}

