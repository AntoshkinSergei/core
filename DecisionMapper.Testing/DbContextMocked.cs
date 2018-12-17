using DecisionMapper.Data.Interfaces;
using DecisionMapper.Entities;
using DecisionMapper.Entities.Infrastructure;
using MongoDB.Driver;

namespace DecisionMapper.Testing.Infrastructure
{
    public class DbContextMocked : IDbContext
    {
        private IMongoCollection<Car> cars;
        private IMongoCollection<Sequence> counters;

        public DbContextMocked()
        {
            cars = new MongoCollectionMocked<Car>();
            counters = new MongoCollectionMocked<Sequence>();
        }

        public IMongoCollection<Car> Cars => cars;

        public IMongoCollection<Sequence> Counters => counters;
    }
}
