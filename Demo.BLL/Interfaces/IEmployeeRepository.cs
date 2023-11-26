using Demo.DAL.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public Task<IQueryable<Employee>> GetEmployeesByName(string Name);
        void DetachEntity(Employee entity);
    }
}
