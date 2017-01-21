using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SemiPlausibleRandomizer.EU4
{
    internal class ProvinceCollection
    {
        public Province this[int key]
        {
            get
            {
                return provinces[key];
            }
        }

        public void LoadAll(string eu4Path)
        {
            var provinceHistoryFiles = Directory.GetFiles(eu4Path + @"\history\provinces", "*.txt");
            foreach (var provinceHistoryFile in provinceHistoryFiles)
            {
                // Read the key from the beginning of the file name.
                var fileName = new FileInfo(provinceHistoryFile).Name;
                int key = int.Parse(new string(fileName.TakeWhile(c => char.IsDigit(c)).ToArray()));

                var province = new Province();
                province.LoadFromFile(key, provinceHistoryFile);
                provinces[key] = province;
            }

            // Determine province adjacencies.
            var provinceAdjacency = new ProvinceAdjacency();
            provinceAdjacency.Load(eu4Path);
            foreach (var province in provinces)
            {
                province.Value.AdjacentProvinces = provinceAdjacency[province.Key];
            }
        }

        public IEnumerable<string> GetAllProvinceNames(Localisation localisation)
        {
            return provinces.Select(i => i.Value.GetName(localisation));
        }

        public IEnumerable<Province> GetProvincesWithCulture(string cultureKey)
        {
            return provinces.Where(p => p.Value.Culture == cultureKey).Select(p => p.Value);
        }

        Dictionary<int, Province> provinces = new Dictionary<int, Province>();
    }
}
