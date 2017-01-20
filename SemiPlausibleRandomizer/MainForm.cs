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

        EU4.World world = new EU4.World();
    }
}
