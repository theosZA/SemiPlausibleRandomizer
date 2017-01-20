using SemiPlausibleRandomizer.EU4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SemiPlausibleRandomizer
{
    internal class Randomizer
    {
        public Randomizer(World world)
        {
            this.world = world;
        }

        /// <summary>
        /// Creates random countries in the given regions according to the specified parameters.
        /// </summary>
        /// <param name="regions">Regions in which to create the new countries.</param>
        public void CreateRandomMod(IEnumerable<string> regionNames, string modPath)
        {
            var provinces = world.GetProvincesInRegions(regionNames);

            // For now we make one country per province.
            var countries = new List<Country>();
            foreach (var province in provinces)
            {
                var country = new Country()
                {
                    Key = CountryIndexToTag(countries.Count),
                    GraphicalCulture = "westerngfx",
                    Color = GetNextRandomColor(),
                    Government = "feudal_monarchy",
                    GovernmentRank = 1,
                    PrimaryCulture = province.Culture,
                    Religion = province.Religion,
                    TechnologyGroup = "western",
                    CapitalProvinceKey = province.Key
                };
                countries.Add(country);
                province.Owner = country.Key;
                province.Controller = country.Key;
                province.Cores = new string[] { country.Key };
            }

            // Create the mod.
            string modName = "Randomized World";
            string[] modLines = 
            {
                $"name = \"{modName}\"",
                $"path = \"mod/{modName}\"",
                "supported_version = \"1.19\""
            };
            File.WriteAllLines($"{modPath}\\{modName}.mod", modLines);
            string ourModPath = $"{modPath}\\{modName}";
            Directory.CreateDirectory(ourModPath);
            Directory.CreateDirectory($"{ourModPath}\\history");
            Directory.CreateDirectory($"{ourModPath}\\history\\countries");
            Directory.CreateDirectory($"{ourModPath}\\history\\provinces");
            Directory.CreateDirectory($"{ourModPath}\\common");
            Directory.CreateDirectory($"{ourModPath}\\common\\countries");
            Directory.CreateDirectory($"{ourModPath}\\common\\country_tags");

            // Create the country files.
            foreach (var country in countries)
            {
                country.Save(ourModPath);
            }
            File.WriteAllLines($"{ourModPath}\\common\\country_tags\\randomized_contries.txt",
                               countries.Select(country => $"{country.Key} = \"countries/{country.Key}.txt\"").ToArray());

            // Create the province files.
            foreach (var province in provinces)
            {
                province.Save($"{ourModPath}\\history\\provinces");
            }
        }

        private string CountryIndexToTag(int index)
        {
            int value = index % 100;
            const string prefixChars = "RSTUVWXYZQPONMLKJIHGFEDCBA";
            char prefix = prefixChars[index / 100];
            return $"{prefix}{value / 10}{value % 10}";
        }

        private Color GetNextRandomColor()
        {
            return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }

        readonly World world;
        Random random = new Random();
    }
}
