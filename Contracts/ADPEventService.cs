using System;
using System.Collections.Generic;
using System.Text;

using SampleADPAPI.Models.EventNotification;

namespace SampleADPAPI.Contracts
{
    public interface ADPEventService
    {
        public ADPEventMessage getEvents();
    }
}
