using System;
using Newtonsoft.Json;

namespace SampleADPAPI.Models.Generic
{
    public class ADPCode
    {
        [JsonProperty("codeValue")]
        public string CodeValue { get; set; }
    }
}
