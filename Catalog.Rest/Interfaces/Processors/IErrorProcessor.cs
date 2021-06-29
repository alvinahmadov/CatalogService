using Catalog.RestClient.Serialization;

namespace Catalog.RestClient
{
    /// <summary>
    /// Processor in charge of serializing errors.
    /// </summary>
    public interface IErrorProcessor<TSerializer>
        where TSerializer : ISerializer
    {
        TSerializer ErrorSerializer { set; }
    }
}