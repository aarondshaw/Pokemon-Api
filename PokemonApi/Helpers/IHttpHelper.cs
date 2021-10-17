using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace PokemonApi.Helpers
{
	public interface IHttpHelper
	{
		public IRestResponse GetRestResponse(string uri);
	}
}
