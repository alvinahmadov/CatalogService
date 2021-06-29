using System.Net;
using Catalog.RestClient.Serialization;

namespace Catalog.RestClient.Processors
{
    /// <summary>
    /// Processor that restricts the internal processing only to when the response is an error.
    /// </summary>
    /// <typeparam name="TErrorRest">Type of the result</typeparam>
    /// <typeparam name="TSerializer">Type of the serializer</typeparam>
    public class ErrorProcessor<TErrorRest, TSerializer>
        : RecursiveProcessorNode<TErrorRest, TErrorRest, TSerializer>, IErrorProcessor<TSerializer>
        where TSerializer : ISerializer
    {
        public TSerializer ErrorSerializer { private get; set; }

        protected override bool CanProcessSub(IRestResponse response)
        {
            return !response.StatusCode.IsSuccessful() && ProcessorStructure.CanProcess(response);
        }

        protected override TErrorRest ProcessSub(IRestResponse response, TSerializer serializer)
        {
            return ProcessorStructure.Process(response, ErrorSerializer);
        }
    }
}