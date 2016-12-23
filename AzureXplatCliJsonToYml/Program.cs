﻿namespace AzureXplatCliJsonToYml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using YamlDotNet.Serialization;

    public class Program
    {
        private static readonly Serializer Ser = new SerializerBuilder().Build();

        /// <summary>
        /// expected arguments:
        /// mode name: currently azure cli mode (asm/arm)
        /// azureFilePath: .json file generated by command: azure help --json
        /// pluginsFilePath: .json file in source code repo
        /// destnation directory where generate .yml files to
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            try
            {
                if (null == args || 4 != args.Length || string.IsNullOrEmpty(args[0]))
                {
                    throw new Exception("Invalid arguments: expecting mode, azure .json file, plugins .json file and destination directory!");
                }
                if (!string.Equals(Constants.SourceExtension, Path.GetExtension(args[1])) ||
                    !string.Equals(Constants.SourceExtension, Path.GetExtension(args[2])))
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
            }
        }

        private static void Save(string name, object obj)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), string.Concat(name, Constants.DestExtension));
            using (var stw = new StreamWriter(file))
            {
                stw.Write("### ");
                stw.WriteLine(Constants.SerializeComments.TrimEnd('\r'));
                stw.Write(Ser.Serialize(obj), true);
            }
        }

        private static List<Category> ParseCategoryObjectToArray(JToken jobject)
        {
            if (jobject?[Constants.Categories] == null)
            {
                return null;
            }

            var categories = JObject.Parse(jobject[Constants.Categories].ToString());
            var keys = categories.Properties().Select(p => p.Name).ToList();

            if (0 == keys.Count)
            {
                return null;
            }

            var results = new List<Category>();

            foreach (var key in keys)
            {
                var temp = jobject[Constants.Categories][key];
                var category = new Category
                {
                    Name = (string) temp[Constants.Name],
                    Description = (string) temp[Constants.Description],
                    Usage = (string) temp[Constants.Usage],
                    Commands = temp[Constants.Commands].ToObject<List<Command>>()
                };
                if (0 != JObject.Parse(temp[Constants.Categories].ToString()).Count)
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
            if (!string.Equals(Constants.SourceExtension, Path.GetExtension(azureFilePath)) ||
                    !string.Equals(Constants.SourceExtension, Path.GetExtension(pluginsFilePath)))
            {
                return;
            }
            var modeName = Constants.ModeNameMapping[mode];
            var modePath = Path.Combine(destRoot, modeName);
            Directory.CreateDirectory(modePath);
            Directory.SetCurrentDirectory(modePath);
            var vm = new AzureXplatCliViewModel();
            using (var str = new StreamReader(azureFilePath))
            {
                var jobject = JObject.Parse(str.ReadToEnd());
                vm.Name = (string) jobject[Constants.Name];
                vm.Description = (string) jobject[Constants.Description];
                vm.Usage = (string) jobject[Constants.Usage];
            }

            // use plugins.arm/asm.json to set commands and categories to fullfill the filepath
            using (var str = new StreamReader(pluginsFilePath))
            {
                var jobject = JObject.Parse(str.ReadToEnd());
                vm.Commands = jobject[Constants.Commands].ToObject<List<Command>>();
                if (null != jobject[Constants.Categories])
                {
                    SaveCategories(ParseCategoryObjectToArray(jobject));
                }
            }
            Directory.SetCurrentDirectory(modePath);
            Save(modeName, vm);
        }
    }
}
