using System.Net;
using Catalog.RestClient.Serialization;

namespace Catalog.RestClient.Processors
{
    /// <summary>
    /// Processor that maps 404 Not Found into False and 2xx to True.
    /// </summary>
    /// <typeparam name="TSerializer">Type of the serializer</typeparam>
    public class BoolAsNotFoundProcessor<TSerializer> : ISimpleProcessorNode<bool, TSerializer>
        where TSerializer : ISerializer
    {
        public bool CanProcess(IRestResponse response)
        {
            return response.StatusCode == HttpStatusCode.NotFound || response.StatusCode.IsSuccessful();
        }

        public bool Process(IRestResponse response, TSerializer serializer)
        {
            return response.StatusCode != HttpStatusCode.NotFound;
        }
    }

    /// <summary>
    /// Boolean processor with JSON serializer.
    /// </summary>
    public class BoolAsNotFoundProcessor : BoolAsNotFoundProcessor<IJsonSerializer>
    {
    }
}