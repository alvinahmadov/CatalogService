using Catalog.RestClient.Processors;
using Catalog.RestClient.Serialization;

namespace Catalog.RestClient
{
    /// <summary>
    /// Base class for a REST Client that manages exceptions
    /// </summary>
    /// <typeparam name="TSerializer">Type of the serializer of the body</typeparam>
    public abstract class BaseRestClient<TSerializer>
        where TSerializer : ISerializer
    {
        /// <summary>
        /// Creates the serializer for the body of successful response.
        /// </summary>
        protected abstract TSerializer CreateSuccessSerializer();

        /// <summary>
        /// Creates the serializer for the body of failed response.
        /// </summary>
        protected abstract TSerializer CreateErrorSerializer();

        /// <summary>
        /// Obtains the processor of exceptions.
        /// </summary>
        protected virtual ExceptionProcessor<TResult, RestBusinessError, RestException, TSerializer> ObtainExceptionProcessor<TResult>()
        {
            return new RestExceptionProcessor<TResult, RestBusinessError, RestHttpError, RestException, TSerializer>().Default();
        }
    }
}