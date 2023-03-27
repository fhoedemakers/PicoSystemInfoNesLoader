using System.ComponentModel;
using System.Diagnostics;
using System.Formats.Tar;
using System.Net.NetworkInformation;

namespace PicoNesLoader
{

    public partial class PicoLoader : Form
    {
        private enum PicoStatus { OK, NoBootSel, NeedDriver, UnknownError };


        private PicoStatus picoStatus;
        private string outputOfPicoTool = string.Empty;
        private string outputOfPicoToolFlash = string.Empty;
        const long MaxTarSize = 12 * 1024 * 1024;
        const string NoDriverInstalled = "You may need to install a driver.";
        const string BootSelNotEnabled = "No accessible RP2040 devices in BOOTSEL mode were found.";


        private long totalTarSize;

        public string appFolder { get { return Path.GetDirectoryName(Application.ExecutablePath); } }
        public SortableBindingList<NesRom> romList = new SortableBindingList<NesRom>();

        private string _tempDir = string.Empty;

        private string tempDir { get
            {
                if ( string.IsNullOrEmpty(_tempDir))
                {
                    _tempDir = GetTemporaryDirectoryName();
                }
                return _tempDir;
            } 
        }
        public PicoLoader()
        {
            InitializeComponent();

        }
        private void checkPicoSystem()
        {


            PicoStatus rval = PicoStatus.OK;
            var executable = Path.Combine(appFolder, "PicoTool\\picotool.exe");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = executable,
                    Arguments = "info -a",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            process.Start();
            outputOfPicoTool = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            Debug.Print("{0}", process.ExitCode);
            if (process.ExitCode != 0)
            {
                if (outputOfPicoTool.Contains(NoDriverInstalled))
                {
                    rval = PicoStatus.NeedDriver;
                }
                else
                {
                    if (outputOfPicoTool.Contains(BootSelNotEnabled))
                    {
                        rval = PicoStatus.NoBootSel;
                    }
                }
            }
            picoStatus = rval;
            return;
        }

        private void PicoLoader_Load(object sender, EventArgs e)
        {
            toolStripStatusLabelLinkToDriver.Visible = false;
            nesRomBindingSource.DataSource = romList;
            CalculateTarSize();
            checkPicoSystem();
            displayPicoStatus();
            timerCheckPico.Enabled = true;
        }

        private void displayPicoStatus()
        {
            toolStripStatusLabelLinkToDriver.Visible = false;          
            buttonCreateTar.Enabled = (totalTarSize > 0 && totalTarSize <= MaxTarSize);
            switch (picoStatus)
            {
                case PicoStatus.OK:
                    toolStripStatusLabelCheckPico.Text = "PicoSystem connected!";                
                    break;
                case PicoStatus.NoBootSel:
                    buttonCreateTar.Enabled = false;
                    toolStripStatusLabelCheckPico.Text = "PicoSystem not connected! Please connect, then press X and power on device.";
                    break;
                case PicoStatus.NeedDriver:
                    buttonCreateTar.Enabled = false;
                    toolStripStatusLabelCheckPico.Text = "Cannot connect to PicoSystem! Please install USB driver!";
                    toolStripStatusLabelLinkToDriver.Visible = true;
                    break;
                default:
                    buttonCreateTar.Enabled = false;
                    MessageBox.Show(outputOfPicoTool, "PicoSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
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
                foreach (var file in openFileDialogNES.FileNames)
                {
                    list.Add(new NesRom(file));
                }
                foreach (var item in romList)
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
            labelTotalSize.Text = $"{totalTarSize / 1024} KB / {MaxTarSize / 1024} KB";
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
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(row.Index);
            }
            CalculateTarSize();
        }

        private async void buttonCreateTar_Click(object sender, EventArgs e)
        {
            timerCheckPico.Enabled = false;
            Progress<int> progress = new Progress<int>(value =>
            {
                toolStripProgressBar1.ProgressBar.Value= value;
            });
            await Task.Run(() => ProcessToPicoSystem(progress));
            if (!string.IsNullOrEmpty(outputOfPicoToolFlash)) {
                MessageBox.Show(outputOfPicoToolFlash, "Error flashing PicoSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);   
            }
            timerCheckPico.Enabled = true;
           
        }

        private void ProcessToPicoSystem(IProgress<int>Progress)
        {
            outputOfPicoToolFlash = string.Empty;
            var tarFileName = GetTemporaryFileName(".tar");
            Progress.Report(0);
            var files = romList.Where(x => x.ValidRom == NesRom.RomType.Valid).Select(x => x.FullpathName);
            if (files.Count() > 0)
            {
                try
                {
                    Directory.CreateDirectory(tempDir);
                    foreach (var file in files)
                    {
                        var destFile = Path.Combine(tempDir, Path.GetFileName(file));
                        File.Copy(file, destFile, true);
                    }
                    Progress.Report(33);
                    TarFile.CreateFromDirectory(tempDir, tarFileName, false);
                   
                    Progress.Report(66);
                    // picotool load rom.nes -t bin -o 0x10090000
                    var executable = Path.Combine(appFolder, "PicoTool\\picotool.exe");
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = executable,
                            Arguments = $"load {tarFileName} -t bin -o 0x10090000",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };
                    process.Start();
                    var output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    Debug.Print("{0}", process.ExitCode);
                    if (process.ExitCode != 0)
                    {
                        outputOfPicoToolFlash = output;
                    }
                    
                }
                catch (Exception ex)
                {
                    outputOfPicoToolFlash = $"{ex}";
                }
                finally
                {
                    Directory.Delete(tempDir, true);
                    File.Delete(tarFileName);
                }
            }
            Progress.Report(100);
        }
        private void timerCheckPico_Tick(object sender, EventArgs e)
        {
            timerCheckPico.Enabled = false;
            checkPicoSystem();
            displayPicoStatus();
            timerCheckPico.Enabled = true;

        }

        private void toolStripStatusLabelLinkToDriver_Click(object sender, EventArgs e)
        {
            timerCheckPico.Enabled = false;
            FormUSBDriver driverForm = new FormUSBDriver();
            driverForm.ShowDialog();
            timerCheckPico.Enabled = true;
        }

        public string GetTemporaryDirectoryName()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            return tempDirectory;
        }

        public string GetTemporaryFileName(string extension)
        {
            string filename = Path.Combine(Path.GetTempPath(), $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}.tar");
            return filename;
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            buttonCreateTar_Click(sender, e);
        }
    }
}