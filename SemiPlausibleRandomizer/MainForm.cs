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
                    var provinceCount = world.GetProvincesInRegions(selectedRegionNames).Count();
                    SelectionRegionsInfo.Text = $"{regionCount} regions, {areaCount} areas, {provinceCount} provinces";
                }));
        }

        private void CreateModButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var randomizer = new Randomizer(world);
            randomizer.CreateRandomMod(RegionList.CheckedItems.Cast<string>(), ".");    // Create mod locally for now
            Cursor = Cursors.Default;
        }

        EU4.World world = new EU4.World();
        string eu4Path = null;
    }
}
