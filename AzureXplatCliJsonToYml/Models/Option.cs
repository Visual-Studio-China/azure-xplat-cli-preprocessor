namespace AzureXplatCliJsonToYml
{
    using System;
    using Microsoft.DocAsCode.Common.EntityMergers;
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    [Serializable]
    public class Option
    {
        [YamlMember(Alias = "flags")]
        [JsonProperty("flags")]
        [MergeOption(MergeOption.MergeKey)]
        public string Flags { get; set; }

        [YamlMember(Alias = "required")]
        [JsonProperty("required")]
        public string Required { get; set; }

        [YamlMember(Alias = "optional")]
        [JsonProperty("optional")]
        public string Optional { get; set; }

        [YamlMember(Alias = "bool")]
        [JsonProperty("bool")]
        public string BoolValue { get; set; }

        [YamlMember(Alias = "short")]
        [JsonProperty("short")]
        public string ShortValue { get; set; }

        [YamlMember(Alias = "long")]
        [JsonProperty("long")]
        public string LongValue { get; set; }

        [YamlMember(Alias = "description")]
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
