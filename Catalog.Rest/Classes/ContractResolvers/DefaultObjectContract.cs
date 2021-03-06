using Newtonsoft.Json.Serialization;
using System;

namespace Catalog.RestClient.Serialization
{
    /// <summary>
    /// Default object creator contract
    /// </summary>
    public class DefaultObjectContract : IObjectContract
    {
        public JsonObjectContract CreateObjectContract(JsonObjectContract parentObjectContract, Type objectType)
        {
            return parentObjectContract;
        }

        public IObjectContract Clone()
        {
            return new DefaultObjectContract();
        }
    }
}