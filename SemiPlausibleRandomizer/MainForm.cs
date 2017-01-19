using Pfarah;
using System;
using System.Data;
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
                var path = dialog.SelectedPath;

                // Load regions
                var regions = ParaValue.LoadText(path + @"\map\region.txt") as ParaValue.Record;
                var regionNames = regions.properties.Select(x => x.Item1).Skip(1);  // We skip the first region because it's special for the Random New World
                RegionList.Items.AddRange(regionNames.ToArray());

            }
        }
    }
}
