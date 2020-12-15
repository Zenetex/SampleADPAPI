using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

using SampleADPAPI.Models.EventNotification;
using SampleADPAPI.Contracts;

namespace SampleADPAPI.Services
{
    public class EventNotificationService
    {
        private ADPService _ADPService;
        private Uri _EventURI;

        public EventNotificationService(ADPService adpService, Uri eventURI)
        {
            this._ADPService = adpService;
            this._EventURI = eventURI;
        }

        public ADPEventMessage getEvents()
        {
            HttpResponseMessage eventResponse = this._ADPService.sendGetMessage(_EventURI, "practioner");
            Task<string> eventJSONTask = eventResponse.Content.ReadAsStringAsync();
            eventJSONTask.Wait();
            string eventJSON = eventJSONTask.Result;

            ADPEventMessage toReturn = JsonConvert.DeserializeObject<ADPEventMessage>(eventJSON);

            IEnumerable<string> messageIds;
            if (eventResponse.Headers.TryGetValues("adp-msg-msgid", out messageIds))
            {
                string currentID = messageIds.First();
                foreach (ADPEvent curEvent in toReturn.Events)
                    curEvent.MessageID = currentID;
            }

            return toReturn;
        }
    }
}
