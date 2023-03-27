using System.ComponentModel;
using System.Diagnostics;

namespace PicoNesLoader
{
    public partial class PicoLoader : Form
    {
        const long MaxTarSize = 12 * 1024 * 1024;
        private long totalTarSize;

        public SortableBindingList<NesRom> romList = new SortableBindingList<NesRom>();
        public PicoLoader()
        {
            InitializeComponent();
            CalculateTarSize();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialogNES.ShowDialog();
          

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "PicoTool\\picotool.exe",
                    Arguments = "info -a",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };


            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                Debug.Print(line);
            }
        }

        private void PicoLoader_Load(object sender, EventArgs e)
        {
            nesRomBindingSource.DataSource = romList;
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "DeleteRow")
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);
                CalculateTarSize();
            }
        }

        private void button_AddRoms_Click(object sender, EventArgs e)
        {
            if (openFileDialogNES.ShowDialog() == DialogResult.OK)
            {
                List<NesRom> list = new List<NesRom>();
                foreach (var file in openFileDialogNES.FileNames) {
                    list.Add(new NesRom(file));
                }
                foreach(var item in romList)
                {
                    list.Add((NesRom)item);
                }
                var distinctList = list.Distinct();
                romList.Clear();
                foreach (var item in distinctList)
                {
                    romList.Add((NesRom)item);
                }
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = romList;
                dataGridView1.Sort(dataGridView1.Columns["dataGridViewTextBoxColumnName"], ListSortDirection.Ascending);
            }
            CalculateTarSize();
        }

        private void CalculateTarSize()
        {
            totalTarSize = romList.Where(x => x.ValidRom == NesRom.RomType.Valid).Select(y => (y.SizeInBytes + 512 + 511) & ~512).Sum();
            // labelTotalSize.Text = $"{totalTarSize / 1024 / 1024} MB / {MaxTarSize / 1024 / 1024} MB";
            labelTotalSize.Text = $"{totalTarSize / 1024 } KB / {MaxTarSize / 1024} KB";
            if (totalTarSize > MaxTarSize)
            {
                labelTotalSize.ForeColor = Color.Red;
            }
            else
            {
                labelTotalSize.ForeColor = Color.Black;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(row.Index);
            }
            CalculateTarSize();
        }
    }
}