using System.Collections.Generic;

namespace SemiPlausibleRandomizer.EU4
{
    internal class World
    {
        public void LoadFromDirectory(string eu4Path)
        {
            localisation.AddFromDirectory(eu4Path + @"\localisation");
            regions.AddFromFile(eu4Path + @"\map\region.txt");
        }

        public IEnumerable<string> GetAllRegionNames()
        {
            return regions.GetAllRegionNames(localisation);
        }

        Localisation localisation = new Localisation();
        RegionCollection regions = new RegionCollection();
    }
}
