using System;
using Newtonsoft.Json;

namespace SampleADPAPI.Models
{
    class BearerToken
    {
        #region Private Fields
        private int _ExpiresIn;
        #endregion //Private Fields

        #region Public Properties
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn
        {
            get
            {
                return _ExpiresIn;
            }
            set
            {
                ExpirationTime = DateTime.Now.AddSeconds(value - 60);
                _ExpiresIn = value;
            }
        }

        [JsonProperty("scope")]
        public string Sope { get; set; }

        public DateTime ExpirationTime { get; set; }
        #endregion //Public Properties
    }
}
