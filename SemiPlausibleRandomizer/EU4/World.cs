using System;
using System.Collections.Generic;
using System.Linq;

namespace SemiPlausibleRandomizer.EU4
{
    internal class World
    {
        public void LoadFromDirectory(string eu4Path)
        {
            localisation.AddFromDirectory(eu4Path + @"\localisation");
            regions.AddFromFile(eu4Path + @"\map\region.txt");
            areas.AddFromFile(eu4Path + @"\map\area.txt");
            provinces.LoadAll(eu4Path);
        }

        public IEnumerable<string> GetAllRegionNames()
        {
            return regions.GetAllRegionNames(localisation);
        }

        public IEnumerable<string> GetAllAreaNames()
        {
            return areas.GetAllAreaNames(localisation);
        }

        public IEnumerable<string> GetAllProvinceNames()
        {
            return provinces.GetAllProvinceNames(localisation);
        }

        public IEnumerable<Area> GetAreasInRegions(IEnumerable<string> regionNames)
        {
            var areaKeys = new List<string>();
            var chosenRegions = regions.GetRegionsByName(localisation, regionNames);
            foreach (var region in chosenRegions)
            {
                areaKeys.AddRange(region.AreaKeys);
            }
            return areaKeys.Select(key => areas[key]);
        }

        Localisation localisation = new Localisation();
        RegionCollection regions = new RegionCollection();

        public IEnumerable<Province> GetProvincesInRegions(IEnumerable<string> regionNames)
        {
            var provinceKeys = new List<int>();
            var chosenAreas = GetAreasInRegions(regionNames);
            foreach (var area in chosenAreas)
            {
                provinceKeys.AddRange(area.ProvinceKeys);
            }
            return provinceKeys.Select(key => provinces[key]);
        }

        AreaCollection areas = new AreaCollection();
        ProvinceCollection provinces = new ProvinceCollection();
    }
}
