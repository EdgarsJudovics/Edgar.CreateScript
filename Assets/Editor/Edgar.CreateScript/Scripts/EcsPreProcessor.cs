using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Edgar.CreateScript
{
    public class EcsPreProcessor
    {
        public readonly Dictionary<string, string> Tags = new Dictionary<string, string>();
        public readonly EcsConfig Config;
        public readonly string FilePath;

        public EcsPreProcessor(EcsConfig config, string filePath)
        {
            Config = config;
            FilePath = filePath;
            CreateTagsDictionary();
            EcsCustomDefinitions.ProcessPreProcessor(this);
        }

        private void CreateTagsDictionary()
        {
            var dateTime = DateTime.Now;
            var t = new Dictionary<string, string>()
            {
                { "date",       dateTime.ToString(Config.DateFormat) },
                { "time",       dateTime.ToString(Config.TimeFormat) },
                { "company",    Config.CompanyName },
                { "product",    Config.ProductName },
                { "namespace",  Config.IsNamespaceAutoNesting ? CreateNestedNamespace() : Config.Namespace },
                { "filename",   Path.GetFileName(FilePath) },
                { "scriptname", Path.GetFileNameWithoutExtension(FilePath) },
            };
            foreach (var tag in t)
                Tags.Add(tag.Key, tag.Value);

            foreach (var tag in Config.CustomTags)
                Tags.Add(tag.Name, tag.Value);
        }

        private string CreateNestedNamespace()
        {
            var p = FilePath.Substring(FilePath.IndexOf(Config.NamespaceAutoNestingRoot, StringComparison.InvariantCulture) +
                                        Config.NamespaceAutoNestingRoot.Length);
            var dirs = Path.GetDirectoryName(p).Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var ns = new StringBuilder(Config.Namespace);
            foreach (var dir in dirs)
            {
                ns.Append(".");
                ns.Append(new Regex("[^a-zA-Z0-9 \\.]").Replace(dir, ""));
            }
            return ns.ToString();
        }

        public string ProcessScript(string text)
        {
            foreach (var kvp in Tags)
            {
                if (string.IsNullOrEmpty(kvp.Value))
                    continue;
                text = Regex.Replace(text, "#" + kvp.Key + "#", kvp.Value, RegexOptions.IgnoreCase);
            }

            return text;
        }
    }

}