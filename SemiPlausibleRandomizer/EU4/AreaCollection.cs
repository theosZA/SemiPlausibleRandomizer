using Pfarah;
using System.Collections.Generic;
using System.Linq;

namespace SemiPlausibleRandomizer.EU4
{
    internal class AreaCollection
    {
        public Area this[string key]
        {
            get
            {
                return areas[key];
            }
        }

        public void AddFromFile(string fileName)
        {
            var areasRecord = ParaValue.LoadText(fileName) as ParaValue.Record;
            foreach (var areaItem in areasRecord.properties)
            {
                var key = areaItem.Item1;
                var area = new Area();
                area.Load(key, areaItem.Item2);
                areas[key] = area;
            }
        }

        public IEnumerable<string> GetAllAreaNames(Localisation localisation)
        {
            return areas.Select(i => i.Value.GetName(localisation));
        }

        public Area GetAreaContainingProvince(int provinceID)
        {
            return areas.Where(t => t.Value.ProvinceKeys.Contains(provinceID))
                        .Select(t => t.Value)
                        .FirstOrDefault();
        }

        Dictionary<string, Area> areas = new Dictionary<string, Area>();
    }
}
