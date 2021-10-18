using PokemonApi.Model;

namespace PokemonApi.Providers
{
	public interface IPokeApiProvider
	{
		public Pokemon GetPokemon(string name);
	}
}
