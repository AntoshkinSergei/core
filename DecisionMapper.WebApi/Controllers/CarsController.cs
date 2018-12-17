using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DecisionMapper.Entities;
using DecisionMapper.Services.Interfaces;
using DecisionMapper.WebApi.Mappers;
using DecisionMapper.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DecisionMapper.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CarsController : Controller
    {
        private readonly ICarsService carsService;
        private readonly CarMapper carMapper;
        private readonly CarDynamicUpdateHelper carDynamicUpdateHelper;

        public CarsController(ICarsService carsService, CarMapper carMapper, CarDynamicUpdateHelper carDynamicUpdateHelper)
        {
            this.carsService = carsService;
            this.carMapper = carMapper;
            this.carDynamicUpdateHelper = carDynamicUpdateHelper;
        }

        // GET api/cars
        [HttpGet]
        public async Task<IEnumerable<CarModel>> Get()
        {             
            var cars = await carsService.Get();
            var models = carMapper.Map(cars);
            return models;
        }

        // GET api/cars/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var model = carMapper.Map(await carsService.Get(id));

            return (model != null)
                ? (IActionResult)new OkObjectResult(model)
                : NotFound();
        }

        // POST api/cars
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]CarModel value)
        {
            var carItem = carMapper.Back(value);

            try
            {
                await carsService.Add(carItem);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/cars
        [HttpPut()]
        public async Task<IActionResult> Update([FromBody]JObject value)
        {
            IEnumerable<Action<Car>> propertyUpdateActions;

            var itemId = carDynamicUpdateHelper.GetItemId(value);
            propertyUpdateActions = carDynamicUpdateHelper.GetItemUpdateDelegates(value);

            try
            {
                await carsService.CreateOrUpdate(itemId, propertyUpdateActions);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE api/cars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await carsService.Delete(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
