namespace AzureXplatCliJsonToYml
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    public class Command: BaseProp
    {
        [YamlMember(Alias = "filePath")]
        [JsonProperty("filePath")]
        public string FilePath { get; set; }

        [YamlMember(Alias = "options")]
        [JsonProperty("options")]
        public List<Option> Options { get; set; }
    }
}
