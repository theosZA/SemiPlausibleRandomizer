using SemiPlausibleRandomizer.Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SemiPlausibleRandomizer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog()
            {
                Description = "Select the Europa Universalis IV folder",
                SelectedPath = @"C:\Program Files (x86)\Steam\steamapps\common\Europa Universalis IV\"
            };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                eu4Path = dialog.SelectedPath;
                world.LoadFromDirectory(eu4Path);

                // Populate controls.
                RegionList.Items.AddRange(world.GetAllRegionNames().ToArray());
            }
        }

        private void RegionList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Wait until after the item checked status is update before updating our view.
            BeginInvoke((MethodInvoker)(
                () =>
                {
                    var selectedRegionNames = RegionList.CheckedItems.Cast<string>();
                    var regionCount = selectedRegionNames.Count();
                    var areaCount = world.GetAreasInRegions(selectedRegionNames).Count();
                    var provinces = world.GetProvincesInRegions(selectedRegionNames);
                    var provinceCount = provinces.Count();
                    var totalDevelopment = provinces.Sum(i => i.Development);
                    SelectionRegionsInfo.Text = $"{regionCount} regions, {areaCount} areas, {provinceCount} provinces - {totalDevelopment} total development";
                    // 25% development -> large countries
                    var largeDevelopment = totalDevelopment / 4;
                    countryCount0.Value = Math.Round(largeDevelopment / developmentLimit0.Value);
                    // 30% development -> medium countries
                    var mediumDevelopment = totalDevelopment * 3 / 10;
                    countryCount1.Value = Math.Round(countryCount0.Value + mediumDevelopment / developmentLimit1.Value);
                    // 35% development -> small countries
                    var smallDevelopment = totalDevelopment * 7 / 20;
                    countryCount2.Value = Math.Round(countryCount1.Value + smallDevelopment / developmentLimit2.Value);
                    // rest -> tiny countries
                }));
        }

        private void CreateModButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            var provinces = world.GetProvincesInRegions(RegionList.CheckedItems.Cast<string>());

            var countrySizeLimits = new List<Tuple<int, int>>();
            if (countryCount0.Value > 0)
            {
                countrySizeLimits.Add(new Tuple<int, int>((int)countryCount0.Value, (int)developmentLimit0.Value));
            }
            if (countryCount1.Value > 0)
            {
                countrySizeLimits.Add(new Tuple<int, int>((int)countryCount1.Value, (int)developmentLimit1.Value));
            }
            if (countryCount2.Value > 0)
            {
                countrySizeLimits.Add(new Tuple<int, int>((int)countryCount2.Value, (int)developmentLimit2.Value));
            }
            countrySizeLimits.Add(new Tuple<int, int>(int.MaxValue, (int)developmentLimitFinal.Value));

            var mod = new EU4Mod();
            mod.Build(world, provinces, countrySizeLimits);
            mod.Save(".", "Randomized World", "1.19");  // Create mod locally for now.

            Cursor = Cursors.Default;
        }

        EU4.World world = new EU4.World();
        string eu4Path = null;
    }
}
