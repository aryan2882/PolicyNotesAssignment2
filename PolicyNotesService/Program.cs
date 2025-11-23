using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Data;
using PolicyNotesService.DTOs;
using PolicyNotesService.Repositories;
using PolicyNotesService.Services;

var builder= WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NotesDbContext>(options =>
    options.UseInMemoryDatabase("PolicyNotesDb"));

builder.Services.AddScoped<IPolicyNotesRepository, PolicyNotesRepository>();
builder.Services.AddScoped<PolicyNoteService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/notes", async (PolicyNoteService svc) =>
{
    var notes = await svc.GetAllAsync();
    return Results.Ok(notes);
});

app.MapGet("/notes/{id:int}", async (int id, PolicyNoteService svc) =>
{
    var note = await svc.GetByIdAsync(id);
    return note is null ? Results.NotFound() : Results.Ok(note);
});

app.MapGet("/notes/by-policy/{policyNumber}", async (string policyNumber, PolicyNoteService svc) =>
{
    var notes = await svc.GetByPolicyNumberAsync(policyNumber);
    return Results.Ok(notes);
});

app.MapPost("/notes", async (CreateNoteDto dto, PolicyNoteService svc, HttpContext ctx) =>
{
    var created = await svc.AddNoteAsync(dto.PolicyNumber, dto.Note);
    return Results.Created($"/notes/{created.Id}", created);
});
app.Run();

public partial class Program { }