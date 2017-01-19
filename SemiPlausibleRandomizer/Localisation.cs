using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;

namespace SemiPlausibleRandomizer
{
    /// <summary>
    /// Holds the localisations for each string in the Paradox game.
    /// </summary>
    /// <remarks>
    /// English-only at the moment.
    /// </remarks>
    internal class Localisation
    {
        /// <summary>
        /// Finds the English localisation for the given key.
        /// </summary>
        /// <param name="key">String key, e.g. "CANT_BUILD_SHIPS_WITHOUT_PORTS"</param>
        /// <returns>The localisation for the key, e.g. "Can't build any ships without having at least one port."</returns>
        public string this[string key]
        {
            get
            {
                return englishLocalisations[key];
            }
        }

        /// <summary>
        /// Adds the localisations from each file in the given directory. Any files that can't be parsed are ignored.
        /// </summary>
        /// <param name="directory">The absolute or relative path to the directory being added.</param>
        /// <remarks>Only English localisations are added at the moment.</remarks>
        public void AddFromDirectory(string directory)
        {
            var localisationFiles = Directory.GetFiles(directory, "*_l_english.yml");
            foreach (var localisationFile in localisationFiles)
            {
                try
                {
                    var text = File.ReadAllText(localisationFile);
                    var yamlText = Regex.Replace(text, @":\d """, @": """); // Paradox YAML is non-standard - this removes the middle digit (0 or 1) from the entries
                    var yaml = new YamlStream();
                    yaml.Load(new StringReader(yamlText));
                    var root = (YamlMappingNode)yaml.Documents[0].RootNode;
                    foreach (var entry in root.Children)
                    {
                        var language = ((YamlScalarNode)entry.Key).Value;
                        if (language == "l_english")
                        {
                            var currentLocalisations = (YamlMappingNode)entry.Value;
                            foreach (var localisation in currentLocalisations.Children)
                            {
                                englishLocalisations.Add(((YamlScalarNode)localisation.Key).Value, ((YamlScalarNode)localisation.Value).Value);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Not all of the Paradox YAML files are good. Hope we don't need this one.
                }
            }
        }

        Dictionary<string, string> englishLocalisations = new Dictionary<string, string>();
    }
}
