using System.Net.Http;

namespace MaxiMiz.Poller.Poller.Abstract
{

    internal interface IRestPoller : IPoller
    {
        HttpClient Client { get; }
    }
}


