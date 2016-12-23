namespace AzureXplatCliJsonToYml
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    public class AzureXplatCliViewModel : AzureXplatCliBaseModel
    {
        [YamlMember(Alias = "commands")]
        [JsonProperty("commands")]
        public List<Command> Commands { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> Metadata = new Dictionary<string, object>();
    }
}
