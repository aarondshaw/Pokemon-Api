using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokemonApi.Model
{
	public class PokeApiResponse
	{
		public PokeApiResponse()
		{
			Names = new List<PokeName>();
			FlavorTexts = new List<FlavorText>();
		}

		[JsonProperty("names")]
		public List<PokeName> Names { get; set; }

		[JsonProperty("flavor_text_entries")]
		public List<FlavorText> FlavorTexts { get; set; }
		[JsonProperty("habitat")]
		public Habitat Habitat { get; set; }
		[JsonProperty("is_legendary")]
		public bool IsLegendary { get; set; }
	}

	public class PokeName
	{
		[JsonProperty("language")]
		public Language Language { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
	}

	public class FlavorText
	{
		[JsonProperty("flavor_text")]
		public string FlavorTextContent { get; set; }
		[JsonProperty("language")]
		public Language Language { get; set; }
	}

	public class Language
	{
		[JsonProperty("name")]
		public string Name { get; set; }
	}

	public class Habitat
	{
		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
