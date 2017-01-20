using Pfarah;
using System.Collections.Generic;

namespace SemiPlausibleRandomizer.EU4
{
    internal class Region
    {
        public string Key { get; set; }
        public IEnumerable<string> AreaKeys { get; set; }

        public void LoadFromRecord(string key, ParaValue.Record data)
        {
            Key = key;
            if (data.TryGet("areas", out var areas))
            {
                AreaKeys = areas.ToStringArray();
            }
        }

        public string GetName(Localisation localisation)
        {
            return localisation[Key];
        }
    }
}
