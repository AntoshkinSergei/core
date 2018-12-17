using System;
using System.Collections.Generic;
using DecisionMapper.Entities;
using DecisionMapper.WebApi.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace DecisionMapper.WebApi.Test
{
    [TestClass]
    public class DynamicUpdateHelperTest
    {
        private CarDynamicUpdateHelper dynamicHelper;

        public DynamicUpdateHelperTest()
        {
            dynamicHelper = new CarDynamicUpdateHelper();
        }

        [DataTestMethod]
        [DataRow("{ Id: 20, Name: \"Item Name 20\" }")]
        [DataRow("{ Name: \"Item Name 21\", Description: \"Item Description 21\" }")]
        public void ItemIdExtractionTest(string json)
        {
            var jObject = JObject.Parse(json);
            var expectedId = jObject.GetValue("Id");

            var actualId = dynamicHelper.GetItemId(jObject);

            if (expectedId != null)
            {
                // Json has Id value
                Assert.IsTrue(actualId.HasValue);
                Assert.AreEqual(expectedId.Value<int>(), actualId.Value);
            }
            else
            {
                // Json has no Id property
                Assert.IsFalse(actualId.HasValue);
            }
        }

        [DataTestMethod]
        [DataRow("{ Name: \"Item Name 10\", Description: \"Item Description 10\" }")]
        [DataRow("{ Name: \"Item Name 11\", Description: null }")]
        [DataRow("{ Name: null, Description: \"Item Description 12\" }")]
        [DataRow("{ Name: \"Item Name 13\" }")]
        [DataRow("{ Description: \"Item Description 14\" }")]
        public void UpdateEntityDelegatesGenerationTest(string json)
        {
            // Get expected values from incoming json
            var jObject = JObject.Parse(json);

            var nameExpectedValue = jObject.GetValue("Name")?.Value<string>();
            var nameValueExists = jObject.GetValue("Name") != null;

            var descriptionExpectedValue = jObject.GetValue("Description")?.Value<string>();
            var descriptionValueExists = jObject.GetValue("Description") != null;

            // Create entity to be updated with values from json
            var sourceEntity = new Car()
            {
                Name = Guid.NewGuid().ToString("N"),
                Description = Guid.NewGuid().ToString("N")
            };

            // Building update delegated & apply updates
            var updateDelegates = dynamicHelper.GetItemUpdateDelegates(jObject);
            var updatedEntity = ApplyUpdateDelegates(updateDelegates, sourceEntity);

            if (nameValueExists)
            {
                // Expect name was updated
                Assert.AreEqual(nameExpectedValue, updatedEntity.Name);
            }
            else
            {
                // Expect name left unchanged
                Assert.AreEqual(sourceEntity.Name, updatedEntity.Name);
            }

            if (descriptionValueExists)
            {
                // Expect description was updated
                Assert.AreEqual(descriptionExpectedValue, updatedEntity.Description);
            }
            else
            {
                // Expect description left unchanged
                Assert.AreEqual(sourceEntity.Description, updatedEntity.Description);
            }
        }

        private Car ApplyUpdateDelegates(IEnumerable<Action<Car>> updateProcs, Car item)
        {
            var updatedEntity = new Car()
            {
                Name = item.Name,
                Description = item.Description
            };

            foreach (var updateProc in updateProcs)
            {
                updateProc?.Invoke(updatedEntity);
            }

            return updatedEntity;
        }
    }
}
