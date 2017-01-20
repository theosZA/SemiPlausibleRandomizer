using System;
using System.Collections.Generic;
using System.Linq;

namespace SemiPlausibleRandomizer.EU4
{
    internal class World
    {
        public Localisation Localisation { get; }

        public World()
        {
            Localisation = new Localisation();
        }

        public void LoadFromDirectory(string eu4Path)
        {
            Localisation.AddFromDirectory(eu4Path + @"\localisation");
            regions.AddFromFile(eu4Path + @"\map\region.txt");
            areas.AddFromFile(eu4Path + @"\map\area.txt");
            provinces.LoadAll(eu4Path);
        }

        public IEnumerable<string> GetAllRegionNames()
        {
            return regions.GetAllRegionNames(Localisation);
        }

        public IEnumerable<string> GetAllAreaNames()
        {
            return areas.GetAllAreaNames(Localisation);
        }

        public IEnumerable<string> GetAllProvinceNames()
        {
            return provinces.GetAllProvinceNames(Localisation);
        }

        public IEnumerable<Area> GetAreasInRegions(IEnumerable<string> regionNames)
        {
            var areaKeys = new List<string>();
            var chosenRegions = regions.GetRegionsByName(Localisation, regionNames);
            foreach (var region in chosenRegions)
            {
                areaKeys.AddRange(region.AreaKeys);
            }
            return areaKeys.Select(key => areas[key]);
        }

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

        RegionCollection regions = new RegionCollection();
        AreaCollection areas = new AreaCollection();
        ProvinceCollection provinces = new ProvinceCollection();
    }
}
