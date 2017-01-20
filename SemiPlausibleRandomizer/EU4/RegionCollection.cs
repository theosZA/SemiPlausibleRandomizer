﻿using Pfarah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemiPlausibleRandomizer.EU4
{
    internal class RegionCollection
    {
        public Region this[string key]
        {
            get
            {
                return regions[key];
            }
        }

        public void AddFromFile(string fileName)
        {
            var regionsRecord = ParaValue.LoadText(fileName) as ParaValue.Record;
            foreach (var regionItem in regionsRecord.properties)
            {
                var key = regionItem.Item1;
                if (key != "random_new_world_region")
                {
                    if (regionItem.Item2.IsRecord)
                    {
                        var value = regionItem.Item2 as ParaValue.Record;
                        var region = new Region();
                        region.LoadFromRecord(key, value);
                        regions[key] = region;
                    }
                }
            }
        }

        public IEnumerable<string> GetAllRegionNames(Localisation localisation)
        {
            return regions.Select(i => i.Value.GetName(localisation));
        }

        Dictionary<string, Region> regions = new Dictionary<string, Region>();
    }
}