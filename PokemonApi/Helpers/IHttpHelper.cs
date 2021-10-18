using RestSharp;

namespace PokemonApi.Helpers
{
	public interface IHttpHelper
	{
		public IRestResponse GetRestResponse(string uri);

		public IRestResponse GetPostJsonResponse(string uri, object body);
	}
}
