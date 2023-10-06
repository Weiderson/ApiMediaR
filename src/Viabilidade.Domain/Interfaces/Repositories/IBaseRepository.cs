using Viabilidade.Domain.Entities;

namespace Viabilidade.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAsync(bool? active = null);
        Task<T> GetAsync(int id);
    }
}
