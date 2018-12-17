using System.Collections.Generic;
using System.Threading.Tasks;
using DecisionMapper.Entities;

namespace DecisionMapper.Data.Interfaces
{
    public interface ICarsRepository
    {
        Task<Car> Get(int id);

        Task<IEnumerable<Car>> Get();

        Task<int> Add(Car item);

        Task Update(Car item);

        Task<int> Delete(int id);
    }
}
