using SemiPlausibleRandomizer.EU4;
using SemiPlausibleRandomizer.Graphics;
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
        public IEnumerable<Province> Provinces { get; set; }
        /// <summary>
        /// For each tuple (a, b) there can be no more than a countries with more than b development.
        /// </summary>
        public IEnumerable<Tuple<int, int>> CountrySizeLimits { get; set; }

        /// <summary>
        /// Adds a number of countries for the largest size limit specified.
        /// </summary>
        public void AddRandomStartCountries()
        {
            var numCountries = CountrySizeLimits.Aggregate((a, b) => a.Item2 > b.Item2 ? a : b).Item1;
            if (numCountries > Provinces.Count())
            {
                numCountries = Provinces.Count();
            }
            for (int i = 0; i < numCountries; ++i)
            {
                AddNewRandomCountry();
            }
        }

        /// <summary>
        /// Adds a province to a random eligible country.
        /// </summary>
        /// <returns>True if a province could be added. False if no more provinces are available.</returns>
        public bool AddProvinceToRandomCountry()
        {
            var country = GetRandomCountry();
            if (country == null)
            {
                country = AddNewRandomCountry();
                if (country == null)
                {
                    return false;
                }
            }
            var province = PickProvinceToAddToCountry(country.Key);
            AddProvinceToCountry(country.Key, province);
            return true;
        }

        public void FinalizeCountries()
        {
            // For each of our countries, see what pre-existing tag we can assign it.
            var tagAssignment = new TagAssignment();
            tagAssignment.LoadFromFile("countries.txt");
            var tagMapping = tagAssignment.AssignTags(countries);

            // Reassign provinces to the new tags.
            foreach (var province in Provinces)
            {
                if (tagMapping.ContainsKey(province.Owner))
                {
                    province.Owner = tagMapping[province.Owner];
                }
                if (tagMapping.ContainsKey(province.Controller))
                {
                    province.Controller = tagMapping[province.Controller];
                }
                province.Cores = province.Cores.Select(i => tagMapping.ContainsKey(i) ? tagMapping[i] : i);
            }

            // Remove the old countries.
            countries.RemoveAll(i => tagMapping.ContainsKey(i.Key));

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
            foreach (var province in Provinces)
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

        void AddProvinceToCountry(string countryKey, Province province)
        {
            if (province == null)
            {
                return;
            }
            province.Owner = countryKey;
            province.Controller = countryKey;
            province.Cores = new string[] { countryKey };
            usedProvinces.Add(province);
            countryDevelopment[countryKey] += province.Development;
        }

        Country AddNewRandomCountry()
        {
            var availableProvinces = Provinces.Except(usedProvinces);
            if (availableProvinces.Count() == 0)
            {
                return null;
            }
            int randomIndex = random.Next(availableProvinces.Count());
            return AddNewCountry(availableProvinces.Skip(randomIndex).First());
        }

        Country AddNewCountry(Province homeProvince)
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
            countryDevelopment[country.Key] = 0;
            AddProvinceToCountry(country.Key, homeProvince);
            return country;
        }

        /// <summary>
        /// Returns a random country in the mod that is still allowed to grow.
        /// </summary>
        /// <returns>A country that is allowed to grow. May be null if no countries are allowed to grow.</returns>
        /// <remarks>For now there are no limits on country growth so any country could be returned as long as it has an available adjacent province.</remarks>
        Country GetRandomCountry()
        {
            var maxAllowedDevelopment = CalculateMaxAllowedDevelopment();

            var availableCountries = countries.Where(c => countryDevelopment[c.Key] < maxAllowedDevelopment && GetAllAvailableAdjacentProvinces(c.Key).Count() > 0);
            if (availableCountries.Count() == 0)
            {
                return null;
            }
            int randomIndex = random.Next(availableCountries.Count());
            return availableCountries.Skip(randomIndex).First();
        }

        /// <summary>
        /// Calculates the maximum development allowed for countries to still be able to grow.
        /// </summary>
        /// <returns>Countries with this amount of development or less are still allowed to grow.</returns>
        int CalculateMaxAllowedDevelopment()
        {
            int maxLimitUnreached = 0;
            foreach (var limit in CountrySizeLimits)
            {
                int developmentLimit = limit.Item2;
                if (developmentLimit > maxLimitUnreached)
                {
                    int countriesAllowed = limit.Item1;
                    int countriesOverLimit = countries.Where(c => countryDevelopment[c.Key] > developmentLimit).Count();
                    if (countriesOverLimit < countriesAllowed)
                    {
                        maxLimitUnreached = developmentLimit;
                    }
                }
            }
            return maxLimitUnreached;
        }

        /// <summary>
        /// Returns a province that can be added to the specified country.
        /// </summary>
        /// <param name="countryKey">The key (tag) of the country to have a province added to.</param>
        /// <returns>An unused province that can be added to the country. May be null if no such province is found.</returns>
        Province PickProvinceToAddToCountry(string countryKey)
        {
            var candidateProvinces = GetAllAvailableAdjacentProvinces(countryKey);
            if (candidateProvinces.Count() == 0)
            {   // This country can't grow.
                return null;    
            }
            int randomIndex = random.Next(candidateProvinces.Count());
            return candidateProvinces.Skip(randomIndex).First();
        }

        IEnumerable<Province> GetAllProvincesOwnedByCountry(string countryKey)
        {
            return Provinces.Where(p => p.Owner == countryKey);
        }

        IEnumerable<Province> GetAllAvailableAdjacentProvinces(string countryKey)
        {
            var provincesInCountry = GetAllProvincesOwnedByCountry(countryKey);
            var adjacentProvinces = new List<Province>();
            foreach (var province in provincesInCountry)
            {
                var currentAdjacentProvinces = province.AdjacentProvinces.Select(k => EU4World.GetProvince(k)).Intersect(Provinces);
                adjacentProvinces.AddRange(currentAdjacentProvinces);
            }
            return adjacentProvinces.Distinct().Except(usedProvinces);
        }

        string CountryIndexToTag(int index)
        {
            int value = index % 100;
            const string prefixChars = "RSTUVWXYZQPONMLKJIHGFEDBA"; // C is reserved for colonial nations
            char prefix = prefixChars[index / 100];
            return $"{prefix}{value / 10}{value % 10}";
        }

        Color GetNextRandomColor()
        {
            return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }

        Random random = new Random();

        List<Country> countries = new List<Country>();
        List<Province> usedProvinces = new List<Province>();
        Dictionary<string, int> countryDevelopment = new Dictionary<string, int>();
        Dictionary<string, string> countryNames = new Dictionary<string, string>();
        Dictionary<string, TgaImage> countryFlags = new Dictionary<string, TgaImage>();
    }
}
