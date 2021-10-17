using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokemonApi.Model;

namespace PokemonApi.Providers
{
	public interface IYodaApiProvider
	{
		public string GetTranslation(string text);
	}
}
