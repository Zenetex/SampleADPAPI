using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using SampleADPAPI.Contracts;
using SampleADPAPI.Models;
using System.Net.Http.Headers;

namespace SampleADPAPI.Services
{
    class WorkforceNowService : ADPService
    {
        #region Private Fields
        private BearerToken _Token;

        private ADPSecrets _SecretVault;

        private Uri _TokenRequestURI;
        #endregion //Private Fields

        #region Private Properties
        private BearerToken Token
        {
            get
            {
                if (this._Token == null || this._Token.ExpirationTime < DateTime.Now)
                    refreshToken();
                return this._Token;
            }
            set
            {
                this._Token = value;
            }
        }
        #endregion //Public Properties

        #region Constructors
        public WorkforceNowService(ADPSecrets secretVault, Uri tokenRequestURI)
        {
            this._SecretVault = secretVault;
            this._TokenRequestURI = tokenRequestURI;
            refreshToken();
        }
        #endregion //Constructors

        #region Public Methods
        public HttpResponseMessage sendGetMessage(Uri requestURI, string roleCode)
        {
            HttpClientHandler handler = createHandler();

            HttpClient webclient = new HttpClient(handler);
            webclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Token.AccessToken);
            var request = new HttpRequestMessage(HttpMethod.Get, requestURI);
            request.Headers.Add("roleCode", roleCode);
            Task<HttpResponseMessage> resultTask = webclient.SendAsync(request);
            return resultTask.Result;
        }
        #endregion //Public Methods

        #region Private Methods
        private void refreshToken()
        {
            HttpResponseMessage tokenResponse = postTokenContent(this._TokenRequestURI, createTokenRequestContent());

            Task<string> tokenJSONTask = tokenResponse.Content.ReadAsStringAsync();
            tokenJSONTask.Wait();
            string tokenJSON = tokenJSONTask.Result;

            this._Token = JsonConvert.DeserializeObject<BearerToken>(tokenJSON);
        }

        private HttpResponseMessage postTokenContent(Uri commandURI, FormUrlEncodedContent content)
        {
            HttpClientHandler handler = createHandler();

            HttpClient webclient = new HttpClient(handler);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, commandURI) { Content = content };
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            Task<HttpResponseMessage> resultTask = webclient.SendAsync(request);
            resultTask.Wait();

            return resultTask.Result;
        }

        private HttpClientHandler createHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            handler.ClientCertificates.Add(this._SecretVault.getSSLCertificate());

            return handler;
        }

        private FormUrlEncodedContent createTokenRequestContent()
        {
            Dictionary<string, string> formData = new Dictionary<string, string>();
            formData.Add("grant_type", "client_credentials");
            formData.Add("client_id", this._SecretVault.getClientID());
            formData.Add("client_secret", this._SecretVault.getClientSecret());

            return new FormUrlEncodedContent(formData);
        }
        #endregion //Private Methods
    }
}
