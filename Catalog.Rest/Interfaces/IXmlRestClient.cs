using Catalog.RestClient.Serialization;

namespace Catalog.RestClient
{
    /// <summary>
    /// REST Client whos body is in XML
    /// </summary>
    public interface IXmlRestClient : IRestClient<IXmlSerializer>
    {
    }
}