using SemiPlausibleRandomizer.EU4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SemiPlausibleRandomizer.Mod
{
    internal class EU4Mod
    {
        public string Name { get; set; }
        public string EU4Version { get; set; }
        public World EU4World { get; set; }

        public void AddNewCountry(Province homeProvince)
        {
            var country = new Country()
            {
                Key = CountryIndexToTag(countries.Count),
                GraphicalCulture = "westerngfx",
                Color = GetNextRandomColor(),
                Government = "feudal_monarchy",
                GovernmentRank = 1,
                PrimaryCulture = homeProvince.Culture,
                Religion = homeProvince.Religion,
                TechnologyGroup = "western",
                CapitalProvinceKey = homeProvince.Key
            };
            countries.Add(country);
            homeProvince.Owner = country.Key;
            homeProvince.Controller = country.Key;
            homeProvince.Cores = new string[] { country.Key };
            provinces.Add(homeProvince);
        }

        public void Save(string modPath)
        {
            // Create the mod.
            string[] modLines =
            {
                $"name = \"{Name}\"",
                $"path = \"mod/{Name}\"",
                $"supported_version = \"{EU4Version}\""
            };
            File.WriteAllLines($"{modPath}\\{Name}.mod", modLines);
            string ourModPath = $"{modPath}\\{Name}";
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

        Random random = new Random();

        List<Country> countries = new List<Country>();
        List<Province> provinces = new List<Province>();
    }
}
