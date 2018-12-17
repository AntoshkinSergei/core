using System;
using DecisionMapper.Entities;
using DecisionMapper.WebApi.Mappers;
using DecisionMapper.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DecisionMapper.WebApi.Test
{
    [TestClass]
    public class MappingTest
    {
        private CarMapper mapper;
        private Random generator;

        public MappingTest()
        {
            mapper = new CarMapper();
            generator = new Random(DateTime.Now.Millisecond);
        }

        [TestMethod]
        public void DirectMappingTest()
        {
            // Check entity to model mapping is correct
            var entity = new Car()
            {
                PublicId = generator.Next(1, 10000),
                Name = Guid.NewGuid().ToString("N"),
                Description = Guid.NewGuid().ToString("N")
            };

            var model = mapper.Map(entity);

            Assert.AreEqual(entity.PublicId, model.Id);
            Assert.AreEqual(entity.Name, model.Name);
            Assert.AreEqual(entity.Description, model.Description);

            // Set triggers on entity and model property list changed. Test must fail in this case
            Assert.AreEqual(4, GetPropertiesCount(entity), $"Property list at class '{ nameof(Car) }' changed. Test must be updated");
            Assert.AreEqual(3, GetPropertiesCount(model), $"Property list at class '{ model.GetType().Name }' changed. Test must be updated");
        }

        [TestMethod]
        public void DirectMappingForNullTest()
        {
            // Check entity to model mapping for null entity
            Car entity = null;
            var model = mapper.Map(entity);

            Assert.IsNull(model);
        }

        [TestMethod]
        public void BackMappingTest()
        {
            // Check model to entity mapping is correct
            var model = new CarModel()
            {
                Id = generator.Next(1, 10000),
                Name = Guid.NewGuid().ToString("N"),
                Description = Guid.NewGuid().ToString("N")
            };

            var entity = mapper.Back(model);

            Assert.AreEqual(model.Id, entity.PublicId);
            Assert.AreEqual(model.Name, entity.Name);
            Assert.AreEqual(model.Description, entity.Description);

            // Set triggers on entity and model property list changed. Test must fail in this case
            Assert.AreEqual(4, GetPropertiesCount(entity), $"Property list at class '{ nameof(Car) }' changed. Test must be updated");
            Assert.AreEqual(3, GetPropertiesCount(model), $"Property list at class '{ model.GetType().Name }' changed. Test must be updated");
        }

        [TestMethod]
        public void BackMappingForNullTest()
        {
            // Check model to entity mapping for null model
            CarModel model = null;
            var entity = mapper.Back(model);

            Assert.IsNull(entity);
        }

        private int GetPropertiesCount(object item)
        {
            return item.GetType().GetProperties().Length;
        }
    }
}
