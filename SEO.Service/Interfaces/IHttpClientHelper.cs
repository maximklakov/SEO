using System.Threading.Tasks;

namespace SEO.Service.Interfaces
{
	public interface IHttpClientHelper
	{
		Task<string> GetStringAsync(string requestUri);
	}
}
