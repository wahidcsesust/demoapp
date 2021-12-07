using HealthCare.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare.Data.Services
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllActive();
        Task<T> Get(long id);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task Save(T entity);
    }
}
