namespace PokemonApi.Providers
{
	public interface ITranslationApiProvider
	{
		public const string ToYoda = "yoda";
		public const string ToShakespeare = "shakespeare";
		public string GetTranslation(string translateTo, string text);
	}
}
