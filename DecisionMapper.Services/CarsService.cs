using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DecisionMapper.Data.Interfaces;
using DecisionMapper.Entities;
using DecisionMapper.Infrastructure.Helpers;
using DecisionMapper.Services.Interfaces;

namespace DecisionMapper.Services
{
    public class CarsService : ICarsService
    {
        public const int NameValueMaxLength = 300;
        public const int DescriptionValueMaxLength = 3000;

        private readonly ICarsRepository carsRepository;

        public CarsService(ICarsRepository carsRepository)
        {
            this.carsRepository = carsRepository;
        }

        public async Task<int> Add(Car item)
        {
            ValidateItem(item);
            return await carsRepository.Add(item);
        }

        public async Task Delete(int id)
        {
            var deletedCount = await carsRepository.Delete(id);
            if (deletedCount == 0)
            {
                throw new KeyNotFoundException();
            }
        }

        public async Task<IEnumerable<Car>> Get()
        {
            return await carsRepository.Get();
        }

        public async Task<Car> Get(int id)
        {
            return await carsRepository.Get(id);
        }

        public async Task Update(Car item)
        {
            ValidateItem(item);
            await carsRepository.Update(item);
        }

        public async Task<int> CreateOrUpdate(int? id, IEnumerable<Action<Car>> updateProcs)
        {
            Check.NotEmpty(updateProcs, nameof(updateProcs));

            int addedOrUpdatedItemPublicId;

            if (id.HasValue)
            {
                // Need to update item
                var item = await Get(id.Value);
                if (item == null)
                {
                    // Item to be updated not found
                    throw new KeyNotFoundException();
                }

                item = ApplyUpdateDelegates(updateProcs, item);
                item.PublicId = id.Value;

                await Update(item);
                addedOrUpdatedItemPublicId = item.PublicId;
            }
            else
            {
                // Need to add new item
                var item = ApplyUpdateDelegates(updateProcs);
                addedOrUpdatedItemPublicId = await Add(item);
            }

            return addedOrUpdatedItemPublicId;
        }

        private Car ApplyUpdateDelegates(IEnumerable<Action<Car>> updateProcs, Car item = null)
        {
            if (item == null)
            {
                item = new Car();
            }

            foreach (var updateProc in updateProcs)
            {
                updateProc?.Invoke(item);
            }

            return item;
        }

        private void ValidateItem(Car item)
        {
            Check.NotNull(item, nameof(item));

            if (String.IsNullOrWhiteSpace(item.Name))
            {
                throw new ValidationException($"Value for '{ nameof(Car.Name) }' property is required");
            }

            if (item.Name.Length > NameValueMaxLength)
            {
                throw new ValidationException($"Value of the property '{ nameof(Car.Name) }' exceeds maximum length of { NameValueMaxLength } characters");
            }

            if (item.Description?.Length > DescriptionValueMaxLength)
            {
                throw new ValidationException($"Value of the property '{ nameof(Car.Description) }' exceeds maximum length of { DescriptionValueMaxLength } characters");
            }
        }
    }
}
