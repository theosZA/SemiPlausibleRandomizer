using System;
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
                world.LoadFromDirectory(dialog.SelectedPath);

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
                    var provinceCount = world.GetProvincesInRegions(selectedRegionNames).Count();
                    SelectionRegionsInfo.Text = $"{regionCount} regions, {areaCount} areas, {provinceCount} provinces";
                }));
        }

        EU4.World world = new EU4.World();
    }
}
