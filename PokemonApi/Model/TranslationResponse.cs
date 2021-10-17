using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PokemonApi.Model
{
	public class TranslationResponse
	{
		[JsonProperty("success")]
		public Success Success { get; set; }
		[JsonProperty("contents")]
		public Contents Contents { get; set; }
	}

	public class Success
	{
		[JsonProperty("total")]
		public int Total { get; set; }
	}

	public class Contents
	{
		[JsonProperty("translated")]
		public string Translated { get; set; }
		[JsonProperty("text")]
		public string Text { get; set; }
		[JsonProperty("translation")]
		public string Translation { get; set; }
	}
}
