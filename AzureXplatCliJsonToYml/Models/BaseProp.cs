namespace AzureXplatCliJsonToYml
{
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    public class BaseProp
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
    }
}
