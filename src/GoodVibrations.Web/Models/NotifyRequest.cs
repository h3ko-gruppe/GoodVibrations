﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NuGet.Protocol.Core.v3;

namespace GoodVibrations.Web.Models
{
    public class NotifyRequest
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }
    }
}