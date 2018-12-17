using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DecisionMapper.Entities;

namespace DecisionMapper.Services.Interfaces
{
    public interface ICarsService
    {
        Task<IEnumerable<Car>> Get();

        Task<Car> Get(int id);

        Task<int> Add(Car item);

        Task Update(Car item);

        Task<int> CreateOrUpdate(int? id, IEnumerable<Action<Car>> updateFuncs);

        Task Delete(int id);
    }
}
