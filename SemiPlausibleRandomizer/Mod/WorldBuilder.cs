using SemiPlausibleRandomizer.EU4;
using SemiPlausibleRandomizer.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemiPlausibleRandomizer.Mod
{
    /// <summary>
    /// A WorldBuilder is used to build multiple countries, deferring needed country choices, like name, until later.
    /// </summary>
    internal class WorldBuilder
    {
        public World EU4World { get; set; }
        public IEnumerable<Province> Provinces { get; set; }
        /// <summary>
        /// For each tuple (a, b) there can be no more than a countries with more than b development.
        /// </summary>
        public IEnumerable<Tuple<int, int>> CountrySizeLimits { get; set; }

        /// <summary>
        /// Creates countries over the provinces provided and according to the limits provided.
        /// </summary>
        public void Build()
        {
            AddRandomStartCountries();
            while (AddProvinceToRandomCountry())
            {}
            AssignTags();
            AssignNames();
            AssignColours();
            UpdateProvinces();
        }

        /// <summary>
        /// Writes all countries, provinces and associated data to files.
        /// </summary>
        /// <param name="rootPath">The root path of the mod being created.</param>
        public void Save(string rootPath)
        {
            // Create the province files.
            foreach (var province in Provinces)
            {
                province.Save($"{rootPath}\\history\\provinces");
            }

            var newCountries = countries.Where(c => c.IsNewTag).Select(c => c.CreateCountry());

            // Create the country files.
            foreach (var country in newCountries)
            {
                country.Save(rootPath);
            }
            File.WriteAllLines($"{rootPath}\\common\\country_tags\\randomized_contries.txt",
                               countries.Select(country => $"{country.Tag} = \"countries/{country.Tag}.txt\"").ToArray());

            // Save country names.
            var countryNamesLines = new List<string>();
            countryNamesLines.Add("\xfeffl_english:");  // include BOM as it's required by EU4
            foreach (var country in countries.Where(c => c.IsNewTag))
            {
                countryNamesLines.Add($" {country.Tag}:0 \"{country.Name}\"");
                countryNamesLines.Add($" {country.Tag}_ADJ:0 \"{country.Name}\"");
            }
            File.WriteAllLines($"{rootPath}\\localisation\\randomized_countries_l_english.yml", countryNamesLines);

            // Save flags.
            foreach (var country in newCountries)
            {
                var flag = new TgaImage(128, 128, country.Color);
                flag.Save($"{rootPath}\\gfx\\flags\\{country.Key}.tga");
            }
        }

        /// <summary>
        /// Adds a number of countries for the largest size limit specified.
        /// </summary>
        void AddRandomStartCountries()
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
        bool AddProvinceToRandomCountry()
        {
            var country = PickRandomCountry();
            if (country == null)
            {
                country = AddNewRandomCountry();
                return (country != null);
            }
            var province = PickProvinceToAddToCountry(country);
            AddProvinceToCountry(country, province);
            return true;
        }

        /// <summary>
        /// Returns a random country in the mod that is still allowed to grow.
        /// </summary>
        /// <returns>A country that is allowed to grow. May be null if no countries are allowed to grow.</returns>
        CountryBuilder PickRandomCountry()
        {
            var maxAllowedDevelopment = CalculateMaxAllowedDevelopment();

            var availableCountries = countries.Where(c => c.Development < maxAllowedDevelopment && GetAllAvailableAdjacentProvinces(c).Count() > 0);
            if (availableCountries.Count() == 0)
            {
                return null;
            }
            int randomIndex = random.Next(availableCountries.Count());
            return availableCountries.Skip(randomIndex).First();
        }

        CountryBuilder AddNewRandomCountry()
        {
            var availableProvinces = Provinces.Except(usedProvinces);
            if (availableProvinces.Count() == 0)
            {
                return null;
            }
            int randomIndex = random.Next(availableProvinces.Count());
            return AddNewCountry(availableProvinces.Skip(randomIndex).First());
        }

        CountryBuilder AddNewCountry(Province homeProvince)
        {
            var country = new CountryBuilder();
            countries.Add(country);
            AddProvinceToCountry(country, homeProvince);
            return country;
        }

        /// <summary>
        /// Returns a province that can be added to the specified country.
        /// </summary>
        /// <param name="country">The country to have a province added to.</param>
        /// <returns>An unused province that can be added to the country. May be null if no such province is found.</returns>
        Province PickProvinceToAddToCountry(CountryBuilder country)
        {
            var candidateProvinces = GetAllAvailableAdjacentProvinces(country);
            if (candidateProvinces.Count() == 0)
            {   // This country can't grow.
                return null;
            }

            var preferenceScores = new Dictionary<Province, int>();
            foreach (var province in candidateProvinces)
            {
                preferenceScores[province] = country.CalculatePreferenceScore(province, EU4World);
            }
            int randomValue = random.Next(preferenceScores.Sum(t => t.Value));
            int cumulativeValue = 0;
            foreach (var pair in preferenceScores)
            {
                cumulativeValue += pair.Value;
                if (randomValue < cumulativeValue)
                {
                    return pair.Key;
                }
            }
            throw new Exception("Unexpected end of range reached when picking a province to add to country");
        }

        /// <summary>
        /// Returns all provinces that are adjacent to existing provinces in the country and are still available to be added to that country.
        /// </summary>
        /// <param name="country">The country to have a province added to.</param>
        /// <returns>All unused provinces that can be added to the country. May be empty if there are no such provinces found.</returns>
        IEnumerable<Province> GetAllAvailableAdjacentProvinces(CountryBuilder country)
        {
            var adjacentProvinces = new List<Province>();
            foreach (var province in country.Provinces)
            {
                var currentAdjacentProvinces = province.AdjacentProvinces.Select(k => EU4World.GetProvince(k)).Intersect(Provinces);
                adjacentProvinces.AddRange(currentAdjacentProvinces);
            }
            return adjacentProvinces.Distinct().Except(usedProvinces);
        }

        void AddProvinceToCountry(CountryBuilder country, Province province)
        {
            country.AddProvince(province);
            usedProvinces.Add(province);
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
                    int countriesOverLimit = countries.Where(c => c.Development > developmentLimit).Count();
                    if (countriesOverLimit < countriesAllowed)
                    {
                        maxLimitUnreached = developmentLimit;
                    }
                }
            }
            return maxLimitUnreached;
        }

        void AssignTags()
        {
            // Check for pre-defined tags.
            var tagAssignment = new TagAssignment();
            tagAssignment.LoadFromFile("countries.txt");
            tagAssignment.AssignTags(countries);
            // Allocate generated tags to the rest.
            int countryIndex = 0;
            foreach (var country in countries)
            {
                if (country.Tag == null)
                {
                    country.Tag = CountryIndexToTag(countryIndex);
                    ++countryIndex;
                }
            }
        }

        void AssignNames()
        {
            // Only need to assign names to generated countries.
            foreach (var country in countries.Where(c => c.IsNewTag))
            {
                // For now we just use the capital province name.
                country.Name = country.Capital.GetName(EU4World.Localisation);
            }
        }

        void AssignColours()
        {
            // Only need to assign colours to generated countries.
            foreach (var country in countries.Where(c => c.IsNewTag))
            {
                country.Colour = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            }
        }

        void UpdateProvinces()
        {
            foreach (var country in countries)
            {
                country.UpdateProvinces();
            }
        }

        static string CountryIndexToTag(int index)
        {
            int value = index % 100;
            const string prefixChars = "RSTUVWXYZQPONMLKJIHGFEDBA"; // C is reserved for colonial nations
            char prefix = prefixChars[index / 100];
            return $"{prefix}{value / 10}{value % 10}";
        }

        List<CountryBuilder> countries = new List<CountryBuilder>();
        List<Province> usedProvinces = new List<Province>();
        Random random = new Random();
    }
}
