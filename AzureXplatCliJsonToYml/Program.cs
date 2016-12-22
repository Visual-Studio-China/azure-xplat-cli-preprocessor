namespace AzureXplatCliJsonToYml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using YamlDotNet.Serialization;

    class Program
    {
        private const string SourceExtension = ".json";
        private const string DestExtension = ".xyml";
        private const string Categories = "categories";
        private const string Name = "name";
        private const string Description = "description";
        private const string Commands = "commands";
        private const string Usage = "usage";
        

        private static readonly Dictionary<string, string> ModeNameMapping = new Dictionary<string, string>()
        {
            {"arm", "Azure Resource Management"},
            {"asm", "Azure Service Management"}
        };

        private static readonly Serializer Ser = new SerializerBuilder().Build();

        private static void Save(string name, object obj)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), string.Concat(name, DestExtension));
            using (var stw = new StreamWriter(file))
            {
                stw.Write(Ser.Serialize(obj));
            }
        }

        public static List<Category> ParseCategoryObjectToArray(JToken jobject)
        {
            if (jobject?[Categories] == null)
            {
                return null;
            }

            var categories = JObject.Parse(jobject[Categories].ToString());
            var keys = categories.Properties().Select(p => p.Name).ToList();

            if (0 == keys.Count)
            {
                return null;
            }

            var results = new List<Category>();

            foreach (var key in keys)
            {
                var temp = jobject[Categories][key];
                var category = new Category
                {
                    Name = (string) temp[Name],
                    Description = (string) temp[Description],
                    Usage = (string) temp[Usage],
                    Commands = temp[Commands].ToObject<List<Command>>()
                };
                if (0 != JObject.Parse(temp[Categories].ToString()).Count)
                {
                    category.Categories = ParseCategoryObjectToArray(temp);
                }
                results.Add(category);
            }
            return results;
        }

        private static void SaveCategories(IReadOnlyCollection<Category> categories)
        {
            // foreach vm.categories, output commands to one page, output category to one page
            if (null == categories || 0 == categories.Count)
            {
                return;
            }

            foreach (var c in categories)
            {
                Directory.CreateDirectory(c.Name);
                Directory.SetCurrentDirectory(Path.Combine(Directory.GetCurrentDirectory(), c.Name));
                if (null != c.Categories && 0 != c.Categories.Count)
                {
                    SaveCategories(c.Categories);
                }
                c.Categories = null;
                Save(c.Name, c);
                Directory.SetCurrentDirectory(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.FullName);
            }
        }

        private static void SplitJsonToYamls(string mode, string azureFilePath, string pluginsFilePath, string destRoot)
        {
            if (string.IsNullOrEmpty(mode) || !File.Exists(azureFilePath) || !File.Exists(pluginsFilePath))
            {
                return;
            }
            if (!string.Equals(SourceExtension, Path.GetExtension(azureFilePath)) ||
                    !string.Equals(SourceExtension, Path.GetExtension(pluginsFilePath)))
            {
                return;
            }
            var modeName = ModeNameMapping[mode];
            var modePath = Path.Combine(destRoot, modeName);
            Directory.CreateDirectory(modePath);
            Directory.SetCurrentDirectory(modePath);
            var vm = new AzureXplatCliViewModel();
            using (var str = new StreamReader(azureFilePath))
            {
                var jobject = JObject.Parse(str.ReadToEnd());
                vm.Name = (string) jobject[Name];
                vm.Description = (string) jobject[Description];
                vm.Usage = (string) jobject[Usage];
            }

            // use plugins.arm/asm.json to set commands and categories to fullfill the filepath
            using (var str = new StreamReader(pluginsFilePath))
            {
                var jobject = JObject.Parse(str.ReadToEnd());
                vm.Commands = jobject[Commands].ToObject<List<Command>>();
                if (null != jobject[Categories])
                {
                    SaveCategories(ParseCategoryObjectToArray(jobject));
                }
            }
            Directory.SetCurrentDirectory(modePath);
            Save(modeName, vm);
        }

        /// <summary>
        /// expected arguments:
        /// mode name: currently azure cli mode (asm/arm)
        /// azureFilePath: .json file generated by command: azure help --json
        /// pluginsFilePath: .json file in source code repo
        /// destnation directory where generate .yml files to
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                if (null == args || 4 != args.Length || string.IsNullOrEmpty(args[0]))
                {
                    throw new Exception(
                        "Invalid arguments: extecting mode, azure .json file, plugins .json file and destination directory!");
                }
                if (!string.Equals(SourceExtension, Path.GetExtension(args[1])) ||
                    !string.Equals(SourceExtension, Path.GetExtension(args[2])))
                {
                    throw new Exception("Invalid arguments: .json file is expected!");
                }
                if (!File.Exists(args[1]) || !File.Exists(args[2]))
                {
                    throw new Exception("Invalid arguments: file not exits!");
                }

                SplitJsonToYamls(args[0], args[1], args[2], args[3]);
            }
            catch (Exception ex)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = color;
                throw;
            }
        }
    }
}
