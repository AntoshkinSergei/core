using DecisionMapper.Entities;
using DecisionMapper.Entities.Infrastructure;
using MongoDB.Driver;

namespace DecisionMapper.Data.Interfaces
{
    public interface IDbContext
    {
        IMongoCollection<Car> Cars { get; }

        IMongoCollection<Sequence> Counters { get; }
    }
}
