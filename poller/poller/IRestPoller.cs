using System.Net.Http;

namespace poller
{

    internal interface IRestPoller : IPoller
    {
        HttpClient Client { get; }

    }
}


