using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using DecisionMapper.Entities;
using DecisionMapper.Services.Interfaces;
using DecisionMapper.Testing.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DecisionMapper.Services.Test
{
    [TestClass]
    public class CarsServiceTest
    {
        private IContainer container;

        public CarsServiceTest()
        {
        }

        [TestInitialize]
        public void InitializeTest()
        {
            var builder = new ContainerBuilder();
            ContainerConfig.Configure(builder);
            container = builder.Build();
        }

        [TestMethod]
        public async Task CreateCarEntityViaDelegatesTest()
        {
            const string expectedItemName = "First added item";
            const string expectedItemDescription = "First added to collection item description";

            var updateItemDelegates = new List<Action<Car>>
            {
                x => x.Name = expectedItemName,
                x => x.Description = expectedItemDescription
            };

            var carsService = GetCarsService();

            // Item must be added to storage as a new one. Do it. Then try to get it from storage
            var addedItemPublicId = await carsService.CreateOrUpdate(null, updateItemDelegates);
            var recentAddedItem = await carsService.Get(addedItemPublicId);

            Assert.IsNotNull(recentAddedItem);
            Assert.AreEqual(expectedItemName, recentAddedItem.Name);
            Assert.AreEqual(expectedItemDescription, recentAddedItem.Description);
        }

        [TestMethod]
        public async Task UpdateCarEntityViaDelegatesTest()
        {
            const string expectedItemName = "Updated item name";
            const string expectedItemDescription = null;

            var car1 = new Car()
            {
                Name = "First added item",
                Description = "First added item description"
            };

            var car2 = new Car()
            {
                Name = "Second added item",
                Description = "Second added item description"
            };


            var updateItemDelegates = new List<Action<Car>>
            {
                x => x.Name = expectedItemName,
                x => x.Description = expectedItemDescription
            };

            var carsService = GetCarsService();

            await carsService.Add(car1);
            var itemToUpdatePublicId = await carsService.Add(car2);

            // Car2 item must be updated here
            var updatedItemPublicId = await carsService.CreateOrUpdate(itemToUpdatePublicId, updateItemDelegates);
            var recentUpdatedItem = await carsService.Get(updatedItemPublicId);

            Assert.IsNotNull(recentUpdatedItem);
            Assert.AreEqual(itemToUpdatePublicId, updatedItemPublicId);
            Assert.AreEqual(expectedItemName, recentUpdatedItem.Name);
            Assert.AreEqual(expectedItemDescription, recentUpdatedItem.Description);
        }

        private ICarsService GetCarsService()
        {
            return container.Resolve<ICarsService>();
        }
    }
}
