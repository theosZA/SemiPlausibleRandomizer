using SemiPlausibleRandomizer.EU4;
using SemiPlausibleRandomizer.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

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

        public void FinalizeCountries()
        {
            // For now we just name each country after it's capital province.
            foreach (var country in countries)
            {
                countryNames[country.Key] = EU4World.GetProvince(country.CapitalProvinceKey).GetName(EU4World.Localisation);
            }

            // Each country gets a solid flag of its colour.
            foreach (var country in countries)
            {
                countryFlags[country.Key] = new TgaImage(128, 128, country.Color);
            }
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
            Directory.CreateDirectory($"{ourModPath}\\localisation");
            Directory.CreateDirectory($"{ourModPath}\\gfx");
            Directory.CreateDirectory($"{ourModPath}\\gfx\\flags");

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

            // Save country names.
            var countryNamesLines = new List<string>();
            countryNamesLines.Add("\xfeffl_english:");  // include BOM as it's required by EU4
            foreach (var countryName in countryNames)
            {
                countryNamesLines.Add($" {countryName.Key}:0 \"{countryName.Value}\"");
                countryNamesLines.Add($" {countryName.Key}_ADJ:0 \"{countryName.Value}\"");
            }
            File.WriteAllLines($"{ourModPath}\\localisation\\randomized_countries_l_english.yml", countryNamesLines);

            // Save flags.
            foreach (var countryFlag in countryFlags)
            {
                countryFlag.Value.Save($"{ourModPath}\\gfx\\flags\\{countryFlag.Key}.tga");
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
        Dictionary<string, string> countryNames = new Dictionary<string, string>();
        Dictionary<string, TgaImage> countryFlags = new Dictionary<string, TgaImage>();
    }
}
