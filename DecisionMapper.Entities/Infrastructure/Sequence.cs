using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DecisionMapper.Entities.Infrastructure
{
    public class Sequence
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public int Value { get; set; }
    }
}
