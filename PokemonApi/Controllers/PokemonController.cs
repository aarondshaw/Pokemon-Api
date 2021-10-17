using Microsoft.AspNetCore.Mvc;
using System.Net;
using PokemonApi.Exceptions;
using PokemonApi.Model;
using PokemonApi.Providers;
using Swashbuckle.AspNetCore.Annotations;

namespace PokemonApi.Controllers
{
	[ApiController]
	
	public class PokemonController : ControllerBase
	{

		private readonly IPokeApiProvider _pokeApiProvider;
		private readonly IShakespeareApiProvider _shakespeareApiProvider;
		private readonly IYodaApiProvider _yodaApiProvider;
		private const string YodaHabitat = "cave";

		public PokemonController(IPokeApiProvider pokeApiProvider, IShakespeareApiProvider shakespeareApiProvider, IYodaApiProvider yodaApiProvider)
		{
			_pokeApiProvider = pokeApiProvider;
			_shakespeareApiProvider = shakespeareApiProvider;
			_yodaApiProvider = yodaApiProvider;
		}


		/// <summary>
		/// Find a Pokemon by name
		/// </summary>
		/// <remarks>Returns a single Pokemon response</remarks>
		/// <param name="name">Name of pokemon to return</param>
		/// <response code="200">Successful Operation</response>
		/// <response code="404">Pokemon not found</response>
		/// <response code="503">Poke API not available</response>
		[HttpGet]
		[Route("/pokemon/{name}")]
		[SwaggerResponse(statusCode: 200, type: typeof(Pokemon), description: "Successful Operation")]
		[SwaggerResponse(statusCode: 404, type: typeof(NotFoundResult), description: "Pokemon not found for given name")]
		[SwaggerResponse(statusCode: 503, type: typeof(StatusCodeResult), description: "Poke Api Service unavailable")]
		public IActionResult Get(string name)
		{
			try
			{
				var pokemon = _pokeApiProvider.GetPokemon(name);
				if (pokemon.Habitat == YodaHabitat || pokemon.IsLegendary)
				{
					pokemon.Description = _yodaApiProvider.GetTranslation(pokemon.Description);
				}
				else
				{
					pokemon.Description = _shakespeareApiProvider.GetTranslation(pokemon.Description);
				}
				return new OkObjectResult(pokemon);
			}
			catch (PokemonNotFoundException)
			{
				return new NotFoundResult();
			}
			catch (ApiException)
			{
				return new StatusCodeResult((int)HttpStatusCode.ServiceUnavailable);
			}
		}

	}
}
