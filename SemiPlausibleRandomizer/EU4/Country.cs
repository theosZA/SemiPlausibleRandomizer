using System.Drawing;
using System.IO;

namespace SemiPlausibleRandomizer.EU4
{
    internal class Country
    {
        public string Key { get; set; }
        public string GraphicalCulture { get; set; }
        public Color Color { get; set; }
        public string Government { get; set; }
        public int GovernmentRank { get; set; }
        public string PrimaryCulture { get; set; }
        public string Religion { get; set; }
        public string TechnologyGroup { get; set; }
        public int CapitalProvinceKey { get; set; }

        public void Save(string path)
        {
            // Common.
            string[] commonLines =
            {
                $"graphical_culture = \"{GraphicalCulture}\"",
                $"color = {{ {Color.R} {Color.G} {Color.B} }}"
            };
            File.WriteAllLines($"{path}\\common\\countries\\{Key}.txt", commonLines);

            // History.
            string[] historyLines =
            {
                $"government = \"{Government}\"",
                $"government_rank = {GovernmentRank}",
                $"primary_culture = {PrimaryCulture}",
                $"religion = {Religion}",
                $"technology_group = {TechnologyGroup}",
                $"capital = {CapitalProvinceKey}"
            };
            File.WriteAllLines($"{path}\\history\\countries\\{Key}.txt", historyLines);
        }
    }
}
