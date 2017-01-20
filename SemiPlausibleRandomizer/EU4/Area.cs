using Pfarah;
using System.Collections.Generic;

namespace SemiPlausibleRandomizer.EU4
{
    internal class Area
    {
        public string Key { get; set; }
        public IEnumerable<int> ProvinceKeys { get; set; }

        public void Load(string key, ParaValue data)
        {
            Key = key;
            ProvinceKeys = data.ToIntCollection();
        }

        public string GetName(Localisation localisation)
        {
            return localisation[Key];
        }
    }
}
