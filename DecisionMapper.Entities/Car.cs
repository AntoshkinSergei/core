using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DecisionMapper.Entities
{
    public class Car
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public int PublicId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
