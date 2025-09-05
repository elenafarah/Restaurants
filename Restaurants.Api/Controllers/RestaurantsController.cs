using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Restaurants.Application.Dtos;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commands;
using Restaurants.Application.Restaurants.Queries;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RestaurantsController(IRestaurantsService restaurantsService, IMediator mediator, IMapper mapper) : ControllerBase
    {
        /*

        // Sans Mediator

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
        {
            var restaurants = await restaurantsService.GetAllRestaurants();
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDto>> GetById([FromRoute] int id)
        {
            var restaurant = await restaurantsService.GetById(id);

            if (restaurant is null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }

        [HttpPost]
        public async Task<ActionResult<RestaurantDto>> CreateRestaurant([FromBody] CreateRestaurantDto createRestaurant)
        {
            var restaurantId = await restaurantsService.Create(createRestaurant);
            var restaurantDto = await restaurantsService.GetById(restaurantId);
           // return CreatedAtAction(nameof(GetById), new { id = restaurantId }, restaurantDto);

            var locationUri = $"/api/restaurants/{restaurantId}";
            return Created(locationUri, restaurantDto);

        }

    
         */

        // Avec Mediator

        [HttpGet]
        [AllowAnonymous]
        //[Authorize(Policy = PolicyNames.CreatedAtLeastTwoRestaurants)]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll([FromQuery] GetAllRestaurantsQuery query, CancellationToken ct)
        {
            var restaurants = await mediator.Send(query, ct);
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
       // [Authorize(Policy = PolicyNames.HasNationality)]
        public async Task<ActionResult<RestaurantDto>> GetById([FromRoute] int id)
        {
            var restaurant = await mediator.Send(new GetByIdQuery(id));
            return Ok(restaurant);
        }


        [HttpPost]
        [Authorize(Roles = UserRoles.Owner)]
        public async Task<ActionResult<RestaurantDto>> CreateRestaurant([FromBody] CreateRestaurantCommand command)
        {
          //  var command = mapper.Map<CreateRestaurantCommand>(dto);
            var id = await mediator.Send(command);
            var restaurant = await mediator.Send(new GetByIdQuery(id));
            return Created($"/api/restaurants/{id}", restaurant);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<RestaurantDto>> DeleteRestaurant([FromRoute] int id)
        {
             await mediator.Send(new DeleteRestaurantCommand(id));
             return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] PatchRestaurantCommand command)
        {
            command.Id = id;
            await mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateRestaurantCommand command)
        {
            var fullCommand = command with { Id = id };
            await mediator.Send(fullCommand);
            return NoContent();
        }

        [HttpPost("{id}/logo")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadLogo([FromRoute] int id, IFormFile file)
        {
            if (file is null || file.Length == 0) return BadRequest("Empty file.");

            var stream = file.OpenReadStream();
            var command = new UploadRestaurantLogoCommand()
            {
                RestaurantId = id,
                FileName = $"{id}-{file.FileName}",
                File = stream
            };

            await mediator.Send(command);

            return NoContent();
        }
    }
}
