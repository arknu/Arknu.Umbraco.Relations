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
    }
}
