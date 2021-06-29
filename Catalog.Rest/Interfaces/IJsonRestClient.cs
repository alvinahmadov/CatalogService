using Catalog.RestClient.Serialization;

namespace Catalog.RestClient
{
    /// <summary>
    /// REST client whose body is in JSON
    /// </summary>
    public interface IJsonRestClient : IRestClient<IJsonSerializer>
    {
    }
}