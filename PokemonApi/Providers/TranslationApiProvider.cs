using System;
using System.Collections.Generic;
using System.Globalization;
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
	public class TranslationApiProvider : ITranslationApiProvider
	{
		private readonly string _apiBaseUrl;
		private readonly IHttpHelper _httpHelper;

		public const string ToYoda = "yoda";
		public const string ToShakespeare = "shakespeare";

		/// <summary>
		/// Initialises the Translation Api Provider
		/// </summary>
		/// <param name="configuration">An instance of an IConfiguration object</param>
		/// <param name="httpHelper">An instance of an IHttpHelper object</param>
		public TranslationApiProvider(IConfiguration configuration, IHttpHelper httpHelper)
		{
			_apiBaseUrl = configuration["TranslationApiBaseUrl"];
			_httpHelper = httpHelper;
		}

		/// <summary>
		/// Get a translated string
		/// </summary>
		/// <param name="translateTo">Language to translate to</param>
		/// <param name="text">Text to translate</param>
		public string GetTranslation(string translateTo, string text)
		{
			var translation = text;
			var requestBody = new { text = text };
			try
			{
				var requestUri = _apiBaseUrl.Replace("{language}", translateTo.ToLower(), StringComparison.InvariantCultureIgnoreCase);
				var response = _httpHelper.GetPostJsonResponse(requestUri, requestBody);
				if (response.StatusCode != HttpStatusCode.OK) return translation;
				var responseObj = JsonConvert.DeserializeObject<TranslationResponse>(response.Content);
				if (responseObj != null && responseObj.Success.Total == 1)
				{
					translation = responseObj.Contents.Translated;
				}
			}
			catch (Exception ex)
			{
				//TODO: log the API call failure
			}
			return translation;
		}
	}
}
