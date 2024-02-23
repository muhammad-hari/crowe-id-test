using Events.Domain.Models;
using Events.Domain.Models.RestAPI;

namespace Events.Services.Interfaces
{
    public interface IAgendasService
    {
        Task<Response<IEnumerable<AgendasModel>>> GetAllAsync(int start = 0, int end = 10);
        Task<int> GetCountAsync();
        Response<IEnumerable<AgendasModel>> FindByArgument(Func<AgendasModel, bool> predicate, int start = 0, int end = 10);
        Response<AgendasModel> FindByID(Guid id);
        Task<Response<AgendasModel>> CreateAsync(AgendasModel agendas);
        Task<Response<AgendasModel>> UpdateAsync(AgendasModel agendas);
        Task<Response<List<Guid>>> DeleteBulkAsync(List<Guid> ids);
        Task<Response<AgendasModel>> DeleteAsync(Guid id);    
    }
}
