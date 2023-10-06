using Viabilidade.Domain.Entities.Alert;

namespace Viabilidade.Domain.Interfaces.Repositories.Alert
{
    public interface IParameterRepository : IBaseRepository<ParameterEntity>
    {
        Task<ParameterEntity> CreateAsync(ParameterEntity entity);

        Task<ParameterEntity> UpdateAsync(int id, ParameterEntity entity);

        Task<bool> DeleteAsync(int id);
    }
}