﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoodVibrations.Web.Models
{
    public class NotifyRequest
    {
        [JsonProperty("EventId")]
        public string EventId { get; set; }
    }
}
