namespace AzureXplatCliJsonToYml
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    public class Command
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

        [YamlMember(Alias = "filePath")]
        [JsonProperty("filePath")]
        public string FilePath { get; set; }

        [YamlMember(Alias = "options")]
        [JsonProperty("options")]
        public List<Option> Options { get; set; } = new List<Option>();
    }
}
