using SemiPlausibleRandomizer.EU4;
using SemiPlausibleRandomizer.Mod;
using System;
using System.Collections.Generic;

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
        /// <param name="regionNames">Names of the regions in which to create the new countries.</param>
        /// <param name="countrySizeLimits">For each tuple (a, b) there can be no more than a countries with more than b development.</param>
        /// <param name="modPath">Path to write the mod files.</param>
        public void CreateRandomMod(IEnumerable<string> regionNames, IEnumerable<Tuple<int, int>> countrySizeLimits, string modPath)
        {
            var mod = new EU4Mod()
            {
                Name = "Randomized World",
                EU4Version = "1.19",
                EU4World = world,
                Provinces = world.GetProvincesInRegions(regionNames),
                CountrySizeLimits = countrySizeLimits
            };

            mod.AddRandomStartCountries();
            while (mod.AddProvinceToRandomCountry())
            {}

            mod.FinalizeCountries();
            mod.Save(modPath);
        }

        readonly World world;
    }
}
