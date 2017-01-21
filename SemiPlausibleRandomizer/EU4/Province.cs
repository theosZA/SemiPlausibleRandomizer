using Pfarah;
using System.Collections.Generic;
using System.IO;

namespace SemiPlausibleRandomizer.EU4
{
    internal class Province
    {
        public int Key { get; set; }

        public string Owner { get; set; }
        public string Controller { get; set; }
        public IEnumerable<string> Cores { get; set; }
        public string Culture { get; set; }
        public string Religion { get; set; }
        public bool IsInHRE { get; set; }
        public int BaseTax { get; set; }
        public int BaseProduction { get; set; }
        public int BaseManpower { get; set; }
        public string Capital { get; set; }
        public bool IsCity { get; set; }
        public int ExtraCost { get; set; }
        public string TradeGoods { get; set; }
        public string Estate { get; set; }
        public bool HasFort { get; set; }
        public IEnumerable<string> DiscoveredBy { get; set; }
        public IEnumerable<ParaValue.Record> PermanentModifiers { get; set; }

        public IEnumerable<int> AdjacentProvinces { get; set; }

        public int Development
        {
            get => BaseTax + BaseProduction + BaseManpower;
        }

        public void LoadFromFile(int key, string historyFileName)
        {
            this.historyFileName = new FileInfo(historyFileName).Name;
            Key = key;
            var history = ParaValue.LoadText(historyFileName) as ParaValue.Record;
            Owner = history.GetString("owner", null);
            Controller = history.GetString("controller", null);
            Cores = history.GetAllStrings("add_core");
            Culture = history.GetString("culture", null);
            Religion = history.GetString("religion", null);
            IsInHRE = history.GetBool("hre");
            BaseTax = history.GetInt("base_tax", 0);
            BaseProduction = history.GetInt("base_production", 0);
            BaseManpower = history.GetInt("base_manpower", 0);
            Capital = history.GetString("capital", null);
            IsCity = history.GetBool("is_city");
            ExtraCost = history.GetInt("extra_cost", 0);
            TradeGoods = history.GetString("trade_goods", null);
            Estate = history.GetString("estate", null);
            HasFort = history.GetBool("fort_15th");
            DiscoveredBy = history.GetAllStrings("discovered_by");
            PermanentModifiers = history.GetAllRecords("add_permanent_province_modifier");
        }

        public void Save(string historyPath)
        {
            List<string> historyLines = new List<string>();
            if (Owner != null)
            {
                historyLines.Add($"owner = {Owner}");
            }
            if (Controller != null)
            {
                historyLines.Add($"controller = {Controller}");
            }
            foreach (var core in Cores)
            {
                historyLines.Add($"add_core = {core}");
            }
            if (Culture != null)
            {
                historyLines.Add($"culture = {Culture}");
            }
            if (Religion != null)
            {
                historyLines.Add($"religion = {Religion}");
            }
            if (IsInHRE)
            {
                historyLines.Add("hre = yes");
            }
            if (BaseTax != 0)
            {
                historyLines.Add($"base_tax = {BaseTax}");
            }
            if (BaseProduction != 0)
            {
                historyLines.Add($"base_production = {BaseProduction}");
            }
            if (BaseManpower != 0)
            {
                historyLines.Add($"base_manpower = {BaseManpower}");
            }
            if (Capital != null)
            {
                historyLines.Add($"capital = {Capital}");
            }
            if (IsCity)
            {
                historyLines.Add("is_city = yes");
            }
            if (ExtraCost != 0)
            {
                historyLines.Add($"extra_cost = {ExtraCost}");
            }
            if (TradeGoods != null)
            {
                historyLines.Add($"trade_goods = {TradeGoods}");
            }
            if (Estate != null)
            {
                historyLines.Add($"estate = {Estate}");
            }
            if (HasFort)
            {
                historyLines.Add("fort_15th = yes");
            }
            foreach (var discorer in DiscoveredBy)
            {
                historyLines.Add($"discovered_by = {discorer}");
            }
            foreach (var modifier in PermanentModifiers)
            {
                historyLines.Add("add_permanent_province_modifier = {");
                historyLines.Add(modifier.ToString());
                historyLines.Add("}");
            }
            File.WriteAllLines($"{historyPath}\\{historyFileName}", historyLines);
        }

        public string GetName(Localisation localisation)
        {
            return localisation[$"PROV{Key}"];
        }

        private string historyFileName = null;
    }
}
