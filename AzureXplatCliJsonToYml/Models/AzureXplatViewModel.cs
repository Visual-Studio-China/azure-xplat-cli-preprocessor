namespace AzureXplatCliJsonToYml
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    public class AzureXplatViewModel
    {
        public AzureXplatViewModel()
        {
            this.Categories = new List<Category>();
        }

        [YamlMember(Alias = "fullName")]
        [JsonProperty("fullName")]
        public string Name { get; set; }

        [YamlMember(Alias = "description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [YamlMember(Alias = "author")]
        [JsonProperty("author")]
        public string Author { get; set; }

        [YamlMember(Alias = "version")]
        [JsonProperty("version")]
        public string Version { get; set; }

        [YamlMember(Alias = "contributors")]
        [JsonProperty("contributors")]
        public string Contributors { get; set; }
        
        [YamlMember(Alias = "homepage")]
        [JsonProperty("homepage")]
        public string Homepage { get; set; }

        [YamlMember(Alias = "usage")]
        [JsonProperty("usage")]
        public string Usage { get; set; }
        
        [YamlMember(Alias = "commands")]
        [JsonProperty("commands")]
        public List<Command> Commands { get; set; }

        [YamlMember(Alias = "categories")]
        [JsonProperty("categories")]
        public List<Category> Categories { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> Metadata = new Dictionary<string, object>();
    }
}
