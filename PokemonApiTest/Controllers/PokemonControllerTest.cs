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
	public class Tests
	{
		private IPokeApiProvider _pokeApiProvider;
		private static Fixture _fixture;

		[SetUp]
		public void Setup()
		{
			_pokeApiProvider = Substitute.For<IPokeApiProvider>();
			_fixture = new Fixture();
		}

		[Test]
		public void TestPokemonExisting()
		{
			var pokeMock = _fixture.Create<Pokemon>();
			_pokeApiProvider.GetPokemon(Arg.Any<string>()).Returns(pokeMock);
			var api = new PokemonController(_pokeApiProvider);
			var result = api.Get("DummyPokemon") as ObjectResult;
			Assert.AreEqual((int)HttpStatusCode.OK,result.StatusCode);
			var resultObj = result.Value as Pokemon;
			Assert.AreEqual(resultObj.Name,pokeMock.Name);
		}

		[Test]
		public void TestPokemonNonExisting()
		{
			_pokeApiProvider.GetPokemon(Arg.Any<string>()).Throws(new PokemonNotFoundException());
			var api = new PokemonController(_pokeApiProvider);
			var result = api.Get("DummyPokemon") as StatusCodeResult;
			Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
		}

		[Test]
		public void TestApiException()
		{
			_pokeApiProvider.GetPokemon(Arg.Any<string>()).Throws(new ApiException("api unavailable"));
			var api = new PokemonController(_pokeApiProvider);
			var result = api.Get("DummyPokemon") as StatusCodeResult;
			Assert.AreEqual((int)HttpStatusCode.ServiceUnavailable, result.StatusCode);
		}


	}
}