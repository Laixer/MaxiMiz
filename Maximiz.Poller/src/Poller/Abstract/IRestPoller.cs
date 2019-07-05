using System;
using System.Net.Http;

namespace MaxiMiz.Poller.Poller.Abstract
{
    internal interface IRestPoller : IPoller, IDisposable
    {
        // TODO: NOTE: Is this not an impl. detail?
        HttpClient Client { get; }
    }
}
