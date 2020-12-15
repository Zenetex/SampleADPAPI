using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SampleADPAPI.Models.EventNotification
{
    public class ADPEventMessage
    {
        [JsonProperty("events")]
        public List<EventNotification.ADPEvent> Events;
    }
}
