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
            var provinces = world.GetProvincesInRegions(regionNames);

            // For now we make one country per province.
            var mod = new EU4Mod()
            {
                Name = "Randomized World",
                EU4Version = "1.19",
                EU4World = world
            };
            foreach (var province in provinces)
            {
                mod.AddNewCountry(province);
            }
            mod.DetermineCountryNames();

            mod.Save(modPath);
        }

        readonly World world;
    }
}
