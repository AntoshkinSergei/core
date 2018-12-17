using DecisionMapper.Data.Interfaces;
using DecisionMapper.Entities;
using DecisionMapper.Entities.Infrastructure;
using DecisionMapper.Infrastructure;
using MongoDB.Driver;

namespace DecisionMapper.Data
{
    public class DbContext: IDbContext
    {
        private readonly IMongoDatabase database; 

        public DbContext(DatabaseSettings config)
        {
            var connection = new MongoUrlBuilder(config.ConnectionString);
            MongoClient client = new MongoClient(config.ConnectionString);
            database = client.GetDatabase(config.Database);
        }

        public IMongoCollection<Car> Cars
        {
            get
            {
                return database.GetCollection<Car>("Cars");
            }
        }

        public IMongoCollection<Sequence> Counters
        {
            get
            {
                return database.GetCollection<Sequence>("Counters");
            }
        }
    }
}
