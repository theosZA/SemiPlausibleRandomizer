using Pfarah;
using System.Collections.Generic;
using System;
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

        Dictionary<string, Area> areas = new Dictionary<string, Area>();
    }
}
