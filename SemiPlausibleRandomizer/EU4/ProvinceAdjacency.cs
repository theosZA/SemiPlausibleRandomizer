using System.Collections.Generic;
using System.Drawing;
using System.IO;

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
                // For each pixel on the map, see if it's adjacent to a different coloured pixel.
                // If that's the case, then we have an adjacency.
                for (int y = 0; y < map.Height; ++y)
                    for (int x = 0; x < map.Width; ++x)
                    {
                        var pixel = map.GetPixel(x, y);
                        var provinceID = colourToProvinceID[pixel];
                        if (y > 0)
                        {
                            CheckAndAddAdjacency(provinceID, colourToProvinceID, map, x, y - 1);
                        }
                        if (y < map.Height - 1)
                        {
                            CheckAndAddAdjacency(provinceID, colourToProvinceID, map, x, y + 1);
                        }
                        CheckAndAddAdjacency(provinceID, colourToProvinceID, map, (x + 1) % map.Width, y);
                        CheckAndAddAdjacency(provinceID, colourToProvinceID, map, (x + map.Width - 1) % map.Width, y);
                    }
            }
        }

        void CheckAndAddAdjacency(int startProvinceID, IDictionary<Color, int> colourToProvinceMapping, Bitmap map, int x, int y)
        {
            var pixel = map.GetPixel(x, y);
            if (colourToProvinceMapping.TryGetValue(pixel, out int endProvinceID))
            {
                if (startProvinceID != endProvinceID)
                {
                    AddAdjacency(startProvinceID, endProvinceID);
                }
            }
        }

        void AddAdjacency(int start, int end)
        {
            if (adjacencies.TryGetValue(start, out var startAdjacencies))
            {
                startAdjacencies.Add(end);
            }
            else
            {
                adjacencies.Add(start, new HashSet<int>());
                adjacencies[start].Add(end);
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

        IDictionary<int, ISet<int>> adjacencies = new Dictionary<int, ISet<int>>();
    }
}
