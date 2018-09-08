using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Arknu.Umbraco.Relations.Models
{
    public class Relation
    {
        [JsonProperty("relationId")]
        public int RelationId { get; set; }
        [JsonProperty("contentId")]
        public int ContentId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("thrashed")]
        public bool Thrashed { get; set; }
        [JsonProperty("published")]
        public bool Published { get; set; }
    }
}
