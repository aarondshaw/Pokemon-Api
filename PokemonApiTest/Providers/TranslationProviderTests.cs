using System;
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
	public class TranslationProviderTests
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
		public void TestTranslate()
		{
			var config = Substitute.For<IConfiguration>();
			var httpHelper = Substitute.For<IHttpHelper>();
			var mockTranslation = _fixture.Create<TranslationResponse>();
			mockTranslation.Success.Total = 1;
			var translationApiJson = JsonConvert.SerializeObject(mockTranslation);
			var provider = new TranslationApiProvider(config, httpHelper);
			httpHelper.GetPostJsonResponse(Arg.Any<string>(),Arg.Any<object>()).Returns(new RestResponse() {Content = translationApiJson, StatusCode = HttpStatusCode.OK});
			var response = provider.GetTranslation("ToLanguage","Text to translate");
			Assert.AreEqual(mockTranslation.Contents.Translated, response);
			Assert.AreNotEqual("Text to translate",response);
			Assert.Pass();
		}

		[Test]
		public void TestTranslateFallbackUnsuccessfulTranslation()
		{
			var config = Substitute.For<IConfiguration>();
			var httpHelper = Substitute.For<IHttpHelper>();

			var provider = new TranslationApiProvider(config, httpHelper);
			httpHelper.GetPostJsonResponse(Arg.Any<string>(), Arg.Any<object>()).Returns(new RestResponse() {StatusCode = HttpStatusCode.BadRequest });
			var response = provider.GetTranslation("ToLanguage", "Text to translate");
			Assert.AreEqual("Text to translate", response);
			Assert.Pass();
		}

		[Test]
		public void TestTranslateFallbackApiFailure()
		{
			var config = Substitute.For<IConfiguration>();
			var httpHelper = Substitute.For<IHttpHelper>();

			var provider = new TranslationApiProvider(config, httpHelper);
			httpHelper.GetPostJsonResponse(Arg.Any<string>(), Arg.Any<object>()).Throws(new Exception());
			var response = provider.GetTranslation("ToLanguage", "Text to translate");
			Assert.AreEqual("Text to translate", response);
			Assert.Pass();
		}

	}
}