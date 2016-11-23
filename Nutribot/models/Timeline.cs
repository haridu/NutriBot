using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nutribot.models
{
    public class Timeline
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime createdAt { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public DateTime updatedAt { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string version { get; set; }

        [JsonProperty(PropertyName = "deleted")]
        public bool deleted { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string username { get; set; }

        [JsonProperty(PropertyName = "food")]
        public string food { get; set; }

        [JsonProperty(PropertyName = "importance")]
        public string importance { get; set; }

    }
}