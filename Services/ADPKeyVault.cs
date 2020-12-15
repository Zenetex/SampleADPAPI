using System;
using System.Security.Cryptography.X509Certificates;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

using SampleADPAPI.Contracts;

namespace SampleADPAPI.Services
{
    class ADPKeyVault : ADPSecrets
    {
        /// <summary>
        /// The URI for the vault containing WorkforceNow Secrets
        /// </summary>
        private Uri _KeyVaultURI;
        private string _CertificateName;
        private string _ClientIDName;
        private string _ClientSecretName;


        public ADPKeyVault(Uri vaultURI, string certificateName, string clientIDName, string clientSecretName)
        {
            this._KeyVaultURI = vaultURI;
            this._CertificateName = certificateName;
            this._ClientIDName = clientIDName;
            this._ClientSecretName = clientSecretName;
        }

        public X509Certificate2 getCertificateFromKeyVault(string certName)
        {
            string encodedCertificate = getSecretFromKeyVault(certName);
            byte[] certificate = System.Convert.FromBase64String(encodedCertificate);
            return new X509Certificate2(certificate);
        }

        public string getSecretFromKeyVault(string secretName)
        {
            SecretClient secretClient = new SecretClient(_KeyVaultURI, new DefaultAzureCredential());
            KeyVaultSecret secret = secretClient.GetSecret(secretName);
            return secret.Value;
        }

        public string getClientID()
        {
            return getSecretFromKeyVault(this._ClientIDName);
        }

        public string getClientSecret()
        {
            return getSecretFromKeyVault(this._ClientSecretName);
        }

        public X509Certificate2 getSSLCertificate()
        {
            return getCertificateFromKeyVault(this._CertificateName);
        }
    }
}
