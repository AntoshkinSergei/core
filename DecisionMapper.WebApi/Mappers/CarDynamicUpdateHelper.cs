using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DecisionMapper.Entities;
using Newtonsoft.Json.Linq;

namespace DecisionMapper.WebApi.Mappers
{
    public class CarDynamicUpdateHelper
    {
        public int? GetItemId(JObject rawModel)
        {
            if (rawModel == null)
            {
                return null;
            }

            var idAsRaw = rawModel.GetValue(nameof(Car.Id));
            if (idAsRaw != null)
            {
                if (int.TryParse(idAsRaw.Value<string>(), out var itemId))
                {
                    return itemId;
                }
                else
                {
                    throw new ValidationException($"Value for property '{ nameof(Car.Id) }' is incorrect");
                }
            }

            return null;
        }

        public IEnumerable<Action<Car>> GetItemUpdateDelegates(JObject rawModel)
        {
            if (rawModel == null)
            {
                return null;
            }

            // May be implemented via dynamic Expression building, but it seems to be a more complex deal
            var updateDelegates = new List<Action<Car>>();

            var rawValue = rawModel.GetValue(nameof(Car.Name));
            if (rawValue != null)
            {
                var newValue = rawValue.Value<string>();
                updateDelegates.Add((x) => x.Name = newValue);
            }

            rawValue = rawModel.GetValue(nameof(Car.Description));
            if (rawValue != null)
            {
                var newValue = rawValue.Value<string>();
                updateDelegates.Add((x) => x.Description = newValue);
            }

            return updateDelegates;
        }
    }
}
