using SemiPlausibleRandomizer.EU4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SemiPlausibleRandomizer.Mod
{
    /// <summary>
    /// A CountryBuilder is used to build a country, deferring needed country choices, like name, until later.
    /// </summary>
    internal class CountryBuilder
    {
        public int Development => provinces.Sum(p => p.Development);
        public string Tag { get; set; }
        public string Name { get; set; }
        public bool IsNewTag => Tag != null && Tag.Length == 3 && char.IsDigit(Tag[2]);
        public Color Colour { get; set; }
        public IEnumerable<Province> Provinces => provinces;
        public Province Capital => provinces.First();

        public void AddProvince(Province newProvince)
        {
            provinces.Add(newProvince);
        }

        public void UpdateProvinces()
        {
            foreach (var province in provinces)
            {
                province.Owner = Tag;
                province.Controller = Tag;
                province.Cores = new string[] { Tag };
            }
        }

        public Country CreateCountry()
        {
            return new Country()
            {
                Key = this.Tag,
                GraphicalCulture = "westerngfx",
                Color = this.Colour,
                Government = "feudal_monarchy",
                GovernmentRank = provinces.Count > 1 ? 2 : 1,
                PrimaryCulture = Capital.Culture,
                Religion = Capital.Religion,
                TechnologyGroup = "western",
                CapitalProvinceKey = Capital.Key
            };
        }

        IList<Province> provinces = new List<Province>();
    }
}
