namespace AzureXplatCliJsonToYml
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    public class AzureXplatCliViewModel
    {
        [YamlMember(Alias = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [YamlMember(Alias = "description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [YamlMember(Alias = "usage")]
        [JsonProperty("usage")]
        public string Usage { get; set; }
        
        [YamlMember(Alias = "commands")]
        [JsonProperty("commands")]
        public List<Command> Commands { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> Metadata = new Dictionary<string, object>();
    }
}
