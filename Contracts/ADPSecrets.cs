using System;
using System.Security.Cryptography.X509Certificates;

namespace SampleADPAPI.Contracts
{
    public interface ADPSecrets
    {
        public X509Certificate2 getSSLCertificate();

        public string getClientID();

        public string getClientSecret();
    }
}
