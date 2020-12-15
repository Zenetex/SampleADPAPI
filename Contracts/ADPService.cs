using System;
using System.Net.Http;

namespace SampleADPAPI.Contracts
{
    public interface ADPService
    {
        public HttpResponseMessage sendGetMessage(Uri requestURI, string roleCode);
    }
}
