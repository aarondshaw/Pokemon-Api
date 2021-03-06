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
		private readonly ITranslationApiProvider _translationApiProvider;
		private const string YodaTranslation = "Yoda";
		private const string ShakespeareTranslation = "Shakespeare";

		public PokemonController(IPokeApiProvider pokeApiProvider, ITranslationApiProvider translationApiProvider)
		{
			_pokeApiProvider = pokeApiProvider;
			_translationApiProvider = translationApiProvider;
		}


		/// <summary>
		/// Get a Pokemon by name
		/// </summary>
		/// <remarks>Returns a single Pokemon response</remarks>
		/// <param name="name">Name of pokemon to return</param>
		/// <response code="200">Successful Operation</response>
		/// <response code="404">Pokemon not found</response>
		/// <response code="503">Poke API not available</response>
		[HttpGet]
		[Route("/pokemon/{name}")]
		[SwaggerResponse(statusCode: 200, type: typeof(Pokemon), description: "Successful Operation")]
		[SwaggerResponse(statusCode: 404, type: typeof(NotFoundResult),
			description: "Pokemon not found for given name")]
		[SwaggerResponse(statusCode: 503, type: typeof(StatusCodeResult), description: "Poke Api Service unavailable")]
		public IActionResult Get(string name)
		{
			try
			{
				var pokemon = _pokeApiProvider.GetPokemon(name);
				return new OkObjectResult(pokemon);
			}
			catch (PokemonNotFoundException)
			{
				return new NotFoundResult();
			}
			catch (ApiException)
			{
				return new StatusCodeResult((int) HttpStatusCode.ServiceUnavailable);
			}
		}


		/// <summary>
		/// Get a Pokemon by name with a translated description
		/// </summary>
		/// <remarks>Returns a single Pokemon response</remarks>
		/// <param name="name">Name of pokemon to return</param>
		/// <response code="200">Successful Operation</response>
		/// <response code="404">Pokemon not found</response>
		/// <response code="503">Poke API not available</response>
		[HttpGet]
		[Route("/pokemon/translated/{name}")]
		[SwaggerResponse(statusCode: 200, type: typeof(Pokemon), description: "Successful Operation")]
		[SwaggerResponse(statusCode: 404, type: typeof(NotFoundResult),
			description: "Pokemon not found for given name")]
		[SwaggerResponse(statusCode: 503, type: typeof(StatusCodeResult), description: "Poke Api Service unavailable")]
		public IActionResult GetTranslated(string name)
		{
			try
			{
				var pokemon = _pokeApiProvider.GetPokemon(name);
				var translationLang = ShakespeareTranslation;
				if (pokemon.IsYodaTypePokemon())
				{
					translationLang = YodaTranslation;
				}
				pokemon.Description = _translationApiProvider.GetTranslation(translationLang, pokemon.Description);

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
