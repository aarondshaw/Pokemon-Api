using System.Net;
using AutoFixture;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using PokemonApi.Controllers;
using PokemonApi.Exceptions;
using PokemonApi.Model;
using PokemonApi.Providers;

namespace PokemonApiTest.Controllers
{
	public class PokemonApiTests
	{
		private IPokeApiProvider _pokeApiProvider;
		private ITranslationApiProvider _translationApiProvider;
		private static Fixture _fixture;

		[SetUp]
		public void Setup()
		{
			_pokeApiProvider = Substitute.For<IPokeApiProvider>();
			_translationApiProvider = Substitute.For<ITranslationApiProvider>();
			_fixture = new Fixture();
		}

		[Test]
		public void TestPokemonExisting()
		{
			var pokeMock = _fixture.Create<Pokemon>();
			_pokeApiProvider.GetPokemon(Arg.Any<string>()).Returns(pokeMock);
			var api = new PokemonController(_pokeApiProvider, _translationApiProvider);
			var result = api.Get("DummyPokemon") as ObjectResult;
			Assert.AreEqual((int)HttpStatusCode.OK,result.StatusCode);
			var resultObj = result.Value as Pokemon;
			Assert.AreEqual(resultObj.Name,pokeMock.Name);
		}

		[Test]
		public void TestPokemonNonExisting()
		{
			_pokeApiProvider.GetPokemon(Arg.Any<string>()).Throws(new PokemonNotFoundException());
			var api = new PokemonController(_pokeApiProvider, _translationApiProvider);
			var result = api.Get("DummyPokemon") as StatusCodeResult;
			Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
		}

		[Test]
		public void TestApiException()
		{
			_pokeApiProvider.GetPokemon(Arg.Any<string>()).Throws(new ApiException("api unavailable"));
			var api = new PokemonController(_pokeApiProvider, _translationApiProvider);
			var result = api.Get("DummyPokemon") as StatusCodeResult;
			Assert.AreEqual((int)HttpStatusCode.ServiceUnavailable, result.StatusCode);
		}

		[Test]
		public void TestPokemonExistingTranslatedShakespeare()
		{
			var pokeMock = _fixture.Create<Pokemon>();
			pokeMock.Habitat = "anything";
			pokeMock.IsLegendary = false;
			_pokeApiProvider.GetPokemon(Arg.Any<string>()).Returns(pokeMock);
			var api = new PokemonController(_pokeApiProvider, _translationApiProvider);
			var result = api.GetTranslated("DummyPokemon") as ObjectResult;
			Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
			_translationApiProvider.Received(1).GetTranslation("Shakespeare",Arg.Any<string>());
			var resultObj = result.Value as Pokemon;
			Assert.AreEqual(resultObj.Name, pokeMock.Name);
		}

		[Test]
		public void TestPokemonExistingTranslatedYodaHabitat()
		{
			var pokeMock = _fixture.Create<Pokemon>();
			pokeMock.Habitat = "cave";
			pokeMock.IsLegendary = false;
			_pokeApiProvider.GetPokemon(Arg.Any<string>()).Returns(pokeMock);
			var api = new PokemonController(_pokeApiProvider, _translationApiProvider);
			var result = api.GetTranslated("DummyPokemon") as ObjectResult;
			Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
			_translationApiProvider.DidNotReceive().GetTranslation("Shakespeare", Arg.Any<string>());
			_translationApiProvider.Received(1).GetTranslation("Yoda", Arg.Any<string>());
			var resultObj = result.Value as Pokemon;
			Assert.AreEqual(resultObj.Name, pokeMock.Name);
		}

		[Test]
		public void TestPokemonExistingTranslatedYodaIsLegendary()
		{
			var pokeMock = _fixture.Create<Pokemon>();
			pokeMock.Habitat = "any";
			pokeMock.IsLegendary = true;
			_pokeApiProvider.GetPokemon(Arg.Any<string>()).Returns(pokeMock);
			var api = new PokemonController(_pokeApiProvider, _translationApiProvider);
			var result = api.GetTranslated("DummyPokemon") as ObjectResult;
			Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
			_translationApiProvider.DidNotReceive().GetTranslation("Shakespeare", Arg.Any<string>());
			_translationApiProvider.Received(1).GetTranslation("Yoda", Arg.Any<string>());
			var resultObj = result.Value as Pokemon;
			Assert.AreEqual(resultObj.Name, pokeMock.Name);
		}

	}
}