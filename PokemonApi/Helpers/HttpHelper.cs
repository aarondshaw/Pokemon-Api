using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace PokemonApi.Helpers
{
	public class HttpHelper : IHttpHelper

	{
		/// <summary>
		/// Gets the response from a HTTP Rest endpoint
		/// </summary>
		/// <param name="uri">The URI to GETt</param>
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
	}
}
