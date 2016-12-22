namespace AzureXplatCliJsonToYml
{
    using System.Collections.Generic;
    using System.Dynamic;
    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    public class Category : BaseProp
    {
        [YamlMember(Alias = "commands")]
        [JsonProperty("commands")]
        public List<Command> Commands { get; set; } = new List<Command>();

        [YamlMember(Alias = "categories")]
        [JsonProperty("categories")]
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
