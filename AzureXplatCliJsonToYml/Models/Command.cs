namespace AzureXplatCliJsonToYml
{
    using System;
    using System.Collections.Generic;
    using Microsoft.DocAsCode.Common.EntityMergers;
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    [Serializable]
    public class Command
    {
        [YamlMember(Alias = "name")]
        [JsonProperty("name")]
        [MergeOption(MergeOption.MergeKey)]
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
