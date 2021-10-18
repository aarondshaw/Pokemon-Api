using System;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PokemonApi.Exceptions;
using PokemonApi.Model;
using PokemonApi.Helpers;

namespace PokemonApi.Providers
{
	public class PokeApiProvider : IPokeApiProvider
	{
		private readonly string _apiBaseUrl;
		private readonly IHttpHelper _httpHelper;
		private const string EnCountryCode = "en";

		/// <summary>
		/// Initialises the Poke Api Provider
		/// </summary>
		/// <param name="configuration">An instance of an IConfiguration object</param>
		/// <param name="httpHelper">An instance of an IHttpHelper object</param>
		public PokeApiProvider(IConfiguration configuration, IHttpHelper httpHelper)
		{
			_apiBaseUrl = configuration["PokeApiBaseUrl"];
			_httpHelper = httpHelper;
		}

		/// <summary>
		/// Get a Pokemon by name
		/// </summary>
		/// <remarks>Returns a single Pokemon response from the Poke Api</remarks>
		/// <param name="name">Name of pokemon to return</param>
		public Pokemon GetPokemon(string name)
		{
			try
			{
				var response = _httpHelper.GetRestResponse($"{_apiBaseUrl}/pokemon-species/{name}");
				switch (response.StatusCode)
				{
					case HttpStatusCode.NotFound:
						throw new PokemonNotFoundException();
					case HttpStatusCode.OK:
					{
						var responseObj = JsonConvert.DeserializeObject<PokeApiResponse>(response.Content);
						if (responseObj != null)
						{

							return new Pokemon()
							{
								Name = responseObj.Names.First(x => string.Equals(x.Language.Name, EnCountryCode, StringComparison.CurrentCultureIgnoreCase)).Name,
								Description = CleanDescription(responseObj.FlavorTexts.First(x => string.Equals(x.Language.Name,EnCountryCode, StringComparison.CurrentCultureIgnoreCase))
									.FlavorTextContent),
								Habitat = responseObj.Habitat.Name,
								IsLegendary = responseObj.IsLegendary
							};
						}

						throw new PokemonNotFoundException();
					}
					default:
						throw new ApiException($"Unexpected status code - {response.StatusCode} {response.Content}");
				}
			}
			catch (PokemonNotFoundException)
			{
				throw;
			}
			catch (Exception ex)
			{
				// TODO: log an error here
				throw new ApiException(ex.Message);
			}
		}

		private static string CleanDescription(string description)
		{
			return description.Replace("\n", " ").Replace("\f", " ");
		}
	}
}
