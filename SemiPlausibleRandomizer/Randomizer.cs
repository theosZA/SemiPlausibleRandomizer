using SemiPlausibleRandomizer.EU4;
using SemiPlausibleRandomizer.Mod;
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
        /// <param name="regions">Regions in which to create the new countries.</param>
        public void CreateRandomMod(IEnumerable<string> regionNames, string modPath)
        {
            var mod = new EU4Mod()
            {
                Name = "Randomized World",
                EU4Version = "1.19",
                EU4World = world,
                Provinces = world.GetProvincesInRegions(regionNames)
            };

            // For now we just make 10 countries.
            // Choose a home province for each of them.
            for (int i = 0; i < 10; ++i)
            {
                mod.AddNewRandomCountry();
            }
            // Now grow them until no provinces are left.
            while (mod.AddProvinceToRandomCountry())
            {}

            mod.FinalizeCountries();
            mod.Save(modPath);
        }

        readonly World world;
    }
}
