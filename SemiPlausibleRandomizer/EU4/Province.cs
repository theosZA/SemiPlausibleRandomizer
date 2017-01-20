using Pfarah;

namespace SemiPlausibleRandomizer.EU4
{
    internal class Province
    {
        public int Key { get; set; }

        public int Development { get; set; }

        public void LoadFromFile(int key, string historyFileName)
        {
            Key = key;
            history = ParaValue.LoadText(historyFileName) as ParaValue.Record;
            Development = history.GetInt("base_tax", 0) + history.GetInt("base_production", 0) + history.GetInt("base_manpower", 0);
        }

        public string GetName(Localisation localisation)
        {
            return localisation[$"PROV{Key}"];
        }

        ParaValue.Record history;
    }
}
