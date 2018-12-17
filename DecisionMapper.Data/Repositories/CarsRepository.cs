using System.Collections.Generic;
using System.Threading.Tasks;
using DecisionMapper.Data.Interfaces;
using DecisionMapper.Entities;
using DecisionMapper.Infrastructure.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DecisionMapper.Data
{
    public class CarsRepository: RepositoryBase, ICarsRepository
    {
        //  Document in collection Counters, that contains counter for publicId to use with Car objects
        private readonly ObjectId publicIdSequenceId = new ObjectId("5c0fa15e6e116c21c8e6379e");

        public CarsRepository(IDbContext dbContext): base(dbContext)
        {
        }

        public async Task<Car> Get(int id)
        {
            var filter = Builders<Car>.Filter.Eq(nameof(Car.PublicId), id);
            return await dbContext.Cars.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Car>> Get()
        {
            return await dbContext.Cars.Find(x => true).ToListAsync();
        }

        public async Task<int> Add(Car item)
        {
            Check.NotNull(item, nameof(item));

            item.PublicId = await GetNewPublicId();
            item.Id = ObjectId.GenerateNewId();

            await dbContext.Cars.InsertOneAsync(item);
            return item.PublicId;
        }

        public async Task Update(Car item)
        {
            Check.NotNull(item, nameof(item));
            await dbContext.Cars.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(item.Id.ToByteArray())), item);
        }

        public async Task<int> Delete(int id)
        {
            var filter = Builders<Car>.Filter.Eq(nameof(Car.PublicId), id);
            var delResult = await dbContext.Cars.DeleteOneAsync(filter);

            return delResult.IsAcknowledged ? (int)delResult.DeletedCount : 0;
        }

        protected override ObjectId GetSequenceId()
        {
            return publicIdSequenceId;
        }
    }
}
