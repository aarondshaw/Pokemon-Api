using System.Net;
using AutoFixture;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using PokemonApi.Controllers;
using PokemonApi.Exceptions;
using PokemonApi.Helpers;
using PokemonApi.Model;
using PokemonApi.Providers;
using RestSharp;

namespace PokemonApiTest.Providers
{
	public class Tests
	{
		private static Fixture _fixture;

		[SetUp]
		public void Setup()
		{
			_fixture = new Fixture();
			Substitute.For<IConfiguration>();
			Substitute.For<IHttpHelper>();
		}

		[Test]
		public void TestGetPokemon()
		{
			var config = Substitute.For<IConfiguration>();
			var httpHelper = Substitute.For<IHttpHelper>();
			var mockPoke = _fixture.Create<PokeApiResponse>();
			mockPoke.Names.Add(new PokeName() { Language = new Language { Name = "EN" }, Name = "PokeName" });
			mockPoke.FlavorTexts.Add(new FlavorText() { Language = new Language { Name = "EN" }, FlavorTextContent = "A Poke Description" });
			var pokeApiJson = JsonConvert.SerializeObject(mockPoke);
			httpHelper.GetRestResponse(Arg.Any<string>()).Returns(new RestResponse() {Content = pokeApiJson, StatusCode = HttpStatusCode.OK});
			var provider = new PokeApiProvider(config, httpHelper);
			var response = provider.GetPokemon("PokeName");
			Assert.Pass();
		}

		[Test]
		public void TestGetPokemonNotFound()
		{
			var config = Substitute.For<IConfiguration>();
			var httpHelper = Substitute.For<IHttpHelper>();
			httpHelper.GetRestResponse(Arg.Any<string>()).Returns(new RestResponse() { StatusCode = HttpStatusCode.NotFound });
			var provider = new PokeApiProvider(config, httpHelper);
			Assert.Throws<PokemonNotFoundException>(delegate { provider.GetPokemon("dummy");});
		}

		[Test]
		public void TestGetPokemonApiError()
		{
			var config = Substitute.For<IConfiguration>();
			var httpHelper = Substitute.For<IHttpHelper>();
			httpHelper.GetRestResponse(Arg.Any<string>()).Returns(new RestResponse() { StatusCode = HttpStatusCode.InternalServerError });
			var provider = new PokeApiProvider(config, httpHelper);
			Assert.Throws<ApiException>(delegate { provider.GetPokemon("dummy"); });
		}

	}
}