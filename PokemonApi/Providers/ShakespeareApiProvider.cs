using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PokemonApi.Exceptions;
using PokemonApi.Model;
using PokemonApi.Helpers;
using RestSharp;

namespace PokemonApi.Providers
{
	public class ShakespeareApiProvider : IShakespeareApiProvider
	{
		private readonly string _apiBaseUrl;
		private readonly IHttpHelper _httpHelper;

		/// <summary>
		/// Initialises the Shakespeare Api Provider
		/// </summary>
		/// <param name="configuration">An instance of an IConfiguration object</param>
		/// <param name="httpHelper">An instance of an IHttpHelper object</param>
		public ShakespeareApiProvider(IConfiguration configuration, IHttpHelper httpHelper)
		{
			_apiBaseUrl = configuration["ShakespeareApiBaseUrl"];
			_httpHelper = httpHelper;
		}

		/// <summary>
		/// Get a translated string
		/// </summary>
		/// <param name="text">Text to translate</param>
		public string GetTranslation(string text)
		{
			return "blah";
		}
	}
}
