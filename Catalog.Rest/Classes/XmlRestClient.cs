using Catalog.RestClient.Serialization;

namespace Catalog.RestClient.Serialization
{
    public class XmlRestClient : RestClient<IXmlSerializer>, IXmlRestClient
    {
        public XmlRestClient(IRestClientExecuter restClientExecuter)
            : base(restClientExecuter)
        {
        }

        protected override IXmlSerializer CreateSuccessSerializer()
        {
            return new XmlSerializer();
        }

        protected override IXmlSerializer CreateErrorSerializer()
        {
            return new XmlSerializer();
        }
    }
}