using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands;
using Restaurants.Application.Dishes.Queries;
using Restaurants.Application.Dtos;

namespace Restaurants.Api.Controllers;

    [Route("api/restaurant/{restaurantId}/[controller]")]
    [ApiController]
    public class DishesController (IMediator mediator, IMapper mapper) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DishDto>>> GetAllDishes([FromRoute] int restaurantId)
        {
        var dishes = await mediator.Send(new GetAllDishesQuery(restaurantId));
        return Ok(dishes);
        }

        [HttpGet("{dishId}")]
        public async Task<ActionResult<DishDto>> GetDishById([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dish = await mediator.Send(new GetByIdDishQuery(restaurantId, dishId));
            return Ok(dish);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishCommand command)
        {
            command.RestaurantId = restaurantId;

            var dishId = await mediator.Send(command);

            return CreatedAtAction(nameof(GetDishById), new { restaurantId, dishId }, null);
        }

        [HttpPut("{dishId:int}")]
        public async Task<ActionResult> UpdateDish(
            [FromRoute] int restaurantId,
            [FromRoute] int dishId,
            [FromBody] UpdateDishCommand command,
            CancellationToken ct)
        {
            command.RestaurantId = restaurantId;
            command.DishId = dishId;
            await mediator.Send(command, ct);
            return NoContent();
        }


        [HttpPatch("{dishId:int}")]
        public async Task<IActionResult> PatchDish(
            [FromRoute] int restaurantId,
            [FromRoute] int dishId,
            [FromBody] PatchDishCommand command,
            CancellationToken ct)
        {
            command.RestaurantId = restaurantId;
            command.DishId = dishId;
            await mediator.Send(command, ct);
            return NoContent();
    }

        // DELETE /api/restaurant/{restaurantId}/dishes/{dishId}
        [HttpDelete("{dishId:int}")]
        public async Task<IActionResult> DeleteDish(
            [FromRoute] int restaurantId,
            [FromRoute] int dishId,
            CancellationToken ct)
        {
            await mediator.Send(new DeleteDishCommand(restaurantId, dishId), ct);
            return NoContent();
        }
}

