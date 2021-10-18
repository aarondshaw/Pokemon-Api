namespace PokemonApi.Model
{

	public class Pokemon
	{
		private const string YodaHabitat = "cave";
		public string Name { get; set; }
		public string Description { get; set; }
		public string Habitat { get; set; }
		public bool IsLegendary { get; set; }

		public bool IsYodaTypePokemon()
		{
			return Habitat == YodaHabitat || IsLegendary;
		}
	}
}
