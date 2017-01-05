namespace AzureXplatCliJsonToYml
{
    using System;
    using Microsoft.DocAsCode.Common.EntityMergers;
    using Microsoft.DocAsCode.DataContracts.Common;
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    [Serializable]
    public class AzureXplatCliBaseModel
    {
        [YamlMember(Alias = "uid")]
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [YamlMember(Alias = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [YamlMember(Alias = "description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [YamlMember(Alias = "usage")]
        [JsonProperty("usage")]
        public string Usage { get; set; }

        [YamlMember(Alias = "conceptual")]
        [JsonProperty("conceptual")]
        public string Conceptual { get; set; }

        [YamlMember(Alias = "documentation")]
        [JsonProperty("documentation")]
        [MergeOption(MergeOption.Ignore)]
        public SourceDetail Documentation { get; set; }
    }
}
