using PolicyNotesService.Models;
using PolicyNotesService.Repositories;

namespace PolicyNotesService.Services
{
    public class PolicyNoteService
    {
        private readonly IPolicyNotesRepository _repo;
        public PolicyNoteService(IPolicyNotesRepository repo)=>
            _repo = repo;

        public Task<PolicyNote> AddNoteAsync(string policyNumber, string note, CancellationToken ct = default)
        {
            var policyNote = new PolicyNote
            {
                PolicyNumber = policyNumber,
                Note = note,
                CreatedAt = DateTime.UtcNow
            };
            return _repo.AddAsync(policyNote, ct);
        }
        public Task<PolicyNote?> GetByIdAsync(int id, CancellationToken ct = default)
        => _repo.GetByIdAsync(id, ct);

        public Task<List<PolicyNote>> GetByPolicyNumberAsync(string policyNumber, CancellationToken ct = default)
            => _repo.GetByPolicyNumberAsync(policyNumber, ct);

        public Task<List<PolicyNote>> GetAllAsync(CancellationToken ct = default)
            => _repo.GetAllAsync(ct);
    }
}
