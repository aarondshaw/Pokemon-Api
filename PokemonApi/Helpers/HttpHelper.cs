using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serialization;

namespace PokemonApi.Helpers
{
	public class HttpHelper : IHttpHelper

	{
		/// <summary>
		/// Gets the response from a HTTP Rest GET endpoint
		/// </summary>
		/// <param name="uri">The URI to GET</param>
		public IRestResponse GetRestResponse(string uri)
		{
			var client = new RestClient(uri)
			{
				Encoding = Encoding.UTF8
			};
			var request = new RestRequest(Method.GET);
			var response = client.Execute(request);
			return response;
		}

		/// <summary>
		/// Gets the response from a HTTP Rest POST endpoint
		/// </summary>
		/// <param name="uri">The URI to POST to</param>
		/// <param name="body">The object to serialize and post in the body as JSON</param>
		public IRestResponse GetPostJsonResponse(string uri, object body)
		{
			var client = new RestClient(uri)
			{
				Encoding = Encoding.UTF8
			};
			var request = new RestRequest(Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.AddJsonBody(body ?? string.Empty);
			var response = client.Execute(request);
			return response;
		}
	}
}
