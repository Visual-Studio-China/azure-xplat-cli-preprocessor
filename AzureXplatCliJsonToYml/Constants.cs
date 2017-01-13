namespace AzureXplatCliJsonToYml
{
    using System.Collections.Generic;

    public static class Constants
    {
        public const string YamlMime = "YamlMime:AzureXplatCli";
        public const string SourceExtension = ".json";
        public const string DestExtension = ".yml";
        public const string Categories = "categories";
        public const string Name = "name";
        public const string Description = "description";
        public const string Commands = "commands";
        public const string Usage = "usage";

        public static readonly Dictionary<string, string> ModeNameMapping = new Dictionary<string, string>()
        {
            {"arm", "Azure Resource Management"},
            {"asm", "Azure Service Management"}
        };
    }
}
