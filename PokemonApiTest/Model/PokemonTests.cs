using AutoFixture;
using NUnit.Framework;
using PokemonApi.Model;

namespace PokemonApiTest.Model
{
	public class PokemonTests
	{
		private static Fixture _fixture;

		[SetUp]
		public void Setup()
		{
			_fixture = new Fixture();
		}

		[Test]
		public void TestPokemonYodaIsLegendary()
		{
			var mockPoke = _fixture.Create<Pokemon>();
			mockPoke.IsLegendary = true;
			Assert.AreEqual(true,mockPoke.IsYodaTypePokemon());
			Assert.Pass();
		}

		[Test]
		public void TestPokemonYodaHabitat()
		{
			var mockPoke = _fixture.Create<Pokemon>();
			mockPoke.IsLegendary = false;
			mockPoke.Habitat = "cave";
			Assert.AreEqual(true, mockPoke.IsYodaTypePokemon());
			Assert.Pass();
		}

		[Test]
		public void TestPokemonShakespeareHabitat()
		{
			var mockPoke = _fixture.Create<Pokemon>();
			mockPoke.IsLegendary = false;
			mockPoke.Habitat = "notacave";
			Assert.AreEqual(false, mockPoke.IsYodaTypePokemon());
			Assert.Pass();
		}

	}
}