using PolicyNotesService.Models;

namespace PolicyNotesService.Repositories;

public interface IPolicyNotesRepository
{
    Task<PolicyNote> AddAsync(PolicyNote note, CancellationToken ct = default);
    Task<PolicyNote?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<PolicyNote>> GetByPolicyNumberAsync(string policyNumber, CancellationToken ct = default);
    Task<List<PolicyNote>> GetAllAsync(CancellationToken ct = default);
}
