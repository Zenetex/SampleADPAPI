using System;
using Newtonsoft.Json;

using SampleADPAPI.Models.Generic;

namespace SampleADPAPI.Models.EventNotification
{
    public class ADPEvent
    {
        [JsonProperty("eventID")]
        public string EventID { get; set; }

        [JsonProperty("eventTitle")]
        public string EventTitle { get; set; }

        [JsonProperty("adp-msg-msgid")]
        public string MessageID { get; set; }

        [JsonProperty("serviceCategoryCode")]
        public ADPCode ServiceCategoryCode { get; set; }

        [JsonProperty("eventNameCode")]
        public ADPCode EvenNameCode { get; set; }

        [JsonProperty("eventStatusCode")]
        public ADPCode EventStatusCode { get; set; }

        [JsonProperty("recordDateTime")]
        public DateTime RecordDateTime { get; set; }

        [JsonProperty("creationDateTime")]
        public DateTime CreationDateTime { get; set; }

        [JsonProperty("effectiveDateTime")]
        public DateTime EffectiveDateTime { get; set; }

        [JsonProperty("dueDateTime")]
        public DateTime DueDateTime { get; set; }
    }
}
