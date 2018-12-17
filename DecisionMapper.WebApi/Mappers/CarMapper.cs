using System.Collections.Generic;
using System.Linq;
using DecisionMapper.Entities;
using DecisionMapper.WebApi.Models;

namespace DecisionMapper.WebApi.Mappers
{
    public class CarMapper
    {
        public CarModel Map(Car item)
        {
            if (item == null)
            {
                return null;
            }

            return new CarModel()
            {
                Id = item.PublicId,
                Name = item.Name,
                Description = item.Description
            };
        }

        public IEnumerable<CarModel> Map(IEnumerable<Car> items)
        {
            return items?.Select(x => Map(x));
        }

        public Car Back(CarModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new Car()
            {
                PublicId = model.Id,
                Name = model.Name,
                Description = model.Description
            };
        }
    }
}
