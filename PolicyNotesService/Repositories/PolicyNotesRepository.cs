using Microsoft.EntityFrameworkCore;
using PolicyNotesService.Data;
using PolicyNotesService.Models;
namespace PolicyNotesService.Repositories
{
    public class PolicyNotesRepository : IPolicyNotesRepository
    {
        private readonly NotesDbContext _db;
        public PolicyNotesRepository(NotesDbContext db)
        {
            _db = db;
        }

        public async Task<PolicyNote> AddAsync(PolicyNote note, CancellationToken ct = default)
        {
            _db.PolicyNotes.Add(note);
            await _db.SaveChangesAsync(ct);
            return note;
        }
        public async Task<PolicyNote?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await _db.PolicyNotes.FindAsync(new object[] { id }, ct);

        public async Task<List<PolicyNote>> GetByPolicyNumberAsync(string policyNumber, CancellationToken ct = default)
        => await _db.PolicyNotes.Where(n => n.PolicyNumber == policyNumber).ToListAsync(ct);

        public async Task<List<PolicyNote>> GetAllAsync(CancellationToken ct = default)
            => await _db.PolicyNotes.ToListAsync(ct);
    }
}
