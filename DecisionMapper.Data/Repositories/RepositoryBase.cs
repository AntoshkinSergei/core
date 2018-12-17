using System;
using System.Threading.Tasks;
using DecisionMapper.Data.Interfaces;
using DecisionMapper.Entities.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DecisionMapper.Data
{
    public abstract class RepositoryBase
    {
        protected readonly IDbContext dbContext;

        public RepositoryBase(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected abstract ObjectId GetSequenceId();

        protected async Task<int> GetNewPublicId()
        {
            var sequenceId = GetSequenceId();
            var counter = await dbContext.Counters.FindOneAndUpdateAsync<Sequence>(new BsonDocument("_id", sequenceId), Builders<Sequence>.Update.Inc(nameof(Sequence.Value), 1));

            if (counter == null)
            {
                await TryInitSequence(sequenceId);
                counter = await dbContext.Counters.FindOneAndUpdateAsync<Sequence>(new BsonDocument("_id", sequenceId), Builders<Sequence>.Update.Inc(nameof(Sequence.Value), 1));

                if (counter == null)
                {
                    throw new Exception($"Unable to init sequence for items collection. Sequence Id: { sequenceId.ToString() }'");
                }
            }

            return counter.Value;
        }

        protected async Task TryInitSequence(ObjectId sequenceId, int initialValue = 1)
        {
            try
            {
                await dbContext.Counters.InsertOneAsync(new Sequence()
                {
                    Id = sequenceId,
                    Value = initialValue
                });
            }
            catch
            {
            }
        }
    }
}
