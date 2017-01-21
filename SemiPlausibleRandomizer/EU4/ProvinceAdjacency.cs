using SemiPlausibleRandomizer.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SemiPlausibleRandomizer.EU4
{
    internal class ProvinceAdjacency
    {
        public IEnumerable<int> this[int key]
        {
            get
            {
                return adjacencies[key];
            }
        }

        public void Load(string eu4Path)
        {
            var colourToProvinceID = LoadColourToProvinceIDMapping(eu4Path + @"\map\definition.csv");

            using (var map = new Bitmap(eu4Path + @"\map\provinces.bmp"))
            {
                var colourAdjacencies = map.CalculateAreaAdjacency();
                foreach (var colourAdjacency in colourAdjacencies)
                {
                    if (colourToProvinceID.TryGetValue(colourAdjacency.Key, out int provinceID))
                    {
                        adjacencies[provinceID] = colourAdjacency.Value.Where(c => colourToProvinceID.ContainsKey(c)).Select(c => colourToProvinceID[c]);
                    }
                }
            }
        }

        IDictionary<Color, int> LoadColourToProvinceIDMapping(string fileName)
        {
            var colourToProvinceID = new Dictionary<Color, int>();
            var rgbMappingLines = File.ReadAllLines(fileName);
            foreach (var line in rgbMappingLines)
            {
                var lineItems = line.Split(';');
                if (lineItems.Length >= 4)
                {
                    if (int.TryParse(lineItems[0], out int provinceID))
                    {
                        var colour = Color.FromArgb(int.Parse(lineItems[1]), int.Parse(lineItems[2]), int.Parse(lineItems[3]));
                        colourToProvinceID[colour] = provinceID;
                    }
                }
            }
            return colourToProvinceID;
        }

        IDictionary<int, IEnumerable<int>> adjacencies = new Dictionary<int, IEnumerable<int>>();
    }
}
