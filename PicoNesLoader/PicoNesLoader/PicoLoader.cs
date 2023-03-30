using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Formats.Tar;
using System.Net.NetworkInformation;
using System.Security.Policy;

namespace PicoNesLoader
{

    public partial class PicoLoader : Form
    {
        private enum PicoStatus { OK, NoBootSel, NeedDriver, UnknownError };

        bool tasksRunning = false;

        private PicoStatus picoStatus;
        private string outputOfPicoTool = string.Empty;
        private string outputOfPicoToolFlash = string.Empty;
        const long MaxTarSize = 12 * 1024 * 1024;
        const string NoDriverInstalled = "You may need to install a driver.";
        const string BootSelNotEnabled = "No accessible RP2040 devices in BOOTSEL mode were found.";

        string infoLabelText = @"This is a helper tool for adding multiple NES roms to the PicoSystem_InfoNes NES emulator running on the Pimoroni PicoSystem handheld.";
        private long totalTarSize;

        public string appFolder { get { return Path.GetDirectoryName(Application.ExecutablePath); } }
        public SortableBindingList<NesRom> romList = new SortableBindingList<NesRom>();

        private string _tempDir = string.Empty;

        private string tempDir
        {
            get
            {
                if (string.IsNullOrEmpty(_tempDir))
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
            //Debug.Print("{0}", process.ExitCode);
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
            //labelInfo.Text = infoLabelText;
            growLabel1.Text = infoLabelText;
            toolStripStatusLabelLinkToDriver.Visible = false;
            nesRomBindingSource.DataSource = romList;
            CalculateTarSize();
            checkPicoSystem();
            displayPicoStatus();
            timerCheckPico.Enabled = true;
            toolStripProgressBar1.Visible = false;  
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
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "DeleteRow")
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);
                CalculateTarSize();
            }
        }

        private async void button_AddRoms_Click(object sender, EventArgs e)
        {
            timerCheckPico.Enabled = false;
           
            if (openFileDialogNES.ShowDialog() == DialogResult.OK && openFileDialogNES.FileNames.Length > 0)
            {
                dataGridView1.DataSource = null;
                panelButtons.Enabled = false;
                toolStripProgressBar1.Visible = true;
                toolStripStatusLabelCheckPico.Text = "Loading files.";
                Progress<int> progress = new Progress<int>(value =>
                {
                    try
                    {
                        toolStripProgressBar1.ProgressBar.Value = value;
                    }
                    catch
                    {
                    }
                });
                tasksRunning = true;
                await Task.Run(
                    () => UploadFile(progress));
                tasksRunning = false;
                dataGridView1.DataSource = romList;
                // dataGridView1.Sort(dataGridView1.Columns["dataGridViewTextBoxColumnName"], ListSortDirection.Ascending);
                CalculateTarSize();
                toolStripStatusLabelCheckPico.Text = "Done.";
                panelButtons.Enabled = true;
            }
           
            timerCheckPico.Enabled = true;
        }

        private void UploadFile(IProgress<int> progress)
        {
            List<NesRom> list = new List<NesRom>();
            int i = 0;
            foreach (var file in openFileDialogNES.FileNames)
            {
                list.Add(new NesRom(file));
                i++;
                var complete = (i * 100) / openFileDialogNES.FileNames.Length;
                progress.Report(complete);
            }
           
            var distinctList = list.Distinct().ToList();
            distinctList.Sort();


            foreach (var item in distinctList)
            {
                romList.Add((NesRom)item);
            }
        }
        private void CalculateTarSize()
        {
            int paxHeaderSize = 1024;
            totalTarSize = romList.Where(x => x.ValidRom == NesRom.RomType.Valid).Select(y => ((y.SizeInBytes + 512 + 511) & ~511) + paxHeaderSize).Sum();
            if (totalTarSize > 0)
            {
                totalTarSize += 1024;
            }
            // labelTotalSize.Text = $"{totalTarSize / 1024 / 1024} MB / {MaxTarSize / 1024 / 1024} MB";
            labelTotalSize.Text = $"{totalTarSize / 1024} KB / {MaxTarSize / 1024} KB";
            // labelTotalSize.Text = $"{totalTarSize} / {MaxTarSize} ";
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
            if (romList.Count > 0)
            {
                timerCheckPico.Enabled = false;
                toolStripProgressBar1.Visible = true;
                Progress<ProgressReport> progress = new Progress<ProgressReport>(value =>
                {
                    toolStripProgressBar1.ProgressBar.Value = value.Complete;
                    toolStripStatusLabelCheckPico.Text = value.info;
                });
                tasksRunning = true;
                await Task.Run(() => ProcessToPicoSystem(progress));
                tasksRunning = false;
                if (!string.IsNullOrEmpty(outputOfPicoToolFlash))
                {
                    MessageBox.Show(outputOfPicoToolFlash, "Error flashing PicoSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                timerCheckPico.Enabled = true;
            }

        }

        private void ProcessToPicoSystem(IProgress<ProgressReport> Progress)
        {
            outputOfPicoToolFlash = string.Empty;
            var tarFileName = GetTemporaryFileName(".tar");
            
            var files = romList.Where(x => x.ValidRom == NesRom.RomType.Valid).Select(x => x.FullpathName);
            
            if (files.Count() > 0)
            {

                ProgressReport report = new ProgressReport() { Complete = 0, info = "Copying Files" };
                try
                {
                   
                    Directory.CreateDirectory(tempDir);
                    int i = 0;
                    foreach (var file in files)
                    {
                        var destFile = Path.Combine(tempDir, Path.GetFileName(file));
                        File.Copy(file, destFile, true);
                        report.Complete = (i * 100) / files.Count();
                        Progress.Report(report);
                        i++;
                    }
                    report.Complete = 100;
                    Progress.Report(report);
                    report.Complete = 0;
                    report.info = "Creating archive";
                    Progress.Report(report);
                    TarFile.CreateFromDirectory(tempDir, tarFileName, false);
                    report.Complete = 100;
                    Progress.Report(report);
                    //FileInfo info = new FileInfo(tarFileName);
                    //Debug.Print("{0}", labelTotalSize.Text);
                    //Debug.Print("Tarfile size: {0}", info.Length);
                    //new TarInspector(tarFileName); 
                    report.Complete = 0;
                    report.info = "Uploading to PicoSystem";
                    Progress.Report(report);
                    // picotool load rom.nes -t bin -o 0x10110000
                    var executable = Path.Combine(appFolder, "PicoTool\\picotool.exe");
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = executable,
                            Arguments = $"load {tarFileName} -t bin -o 0x10110000",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };
                    process.Start();
                    string output = string.Empty;
                    // This is the output
                    // Loading into Flash: [==============================]  100%
                    while (!process.StandardOutput.EndOfStream)
                    {
                        var line = process.StandardOutput.ReadLine();
                        Debug.Print(line);
                        output += line;
                    }
                    process.WaitForExit();
                    Debug.Print("{0}", process.ExitCode);
                    if (process.ExitCode != 0)
                    {
                        outputOfPicoToolFlash = output;
                    }
                    report.Complete = 100;
                    report.info = "Done";

                }
                catch (Exception ex)
                {
                    outputOfPicoToolFlash = $"{ex}";
                }
                finally
                {
                    Directory.Delete(tempDir, true);
                    File.Delete(tarFileName);
                    Progress.Report(report);
                }
            }
           
        }
        private void timerCheckPico_Tick(object sender, EventArgs e)
        {
            toolStripProgressBar1.Visible = false;
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
            if (romList.Count > 0)
            {
                if (MessageBox.Show("Are you sure to clear the entire list?", "Clear list", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    romList.Clear();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(linkLabel1.Text) { UseShellExecute = true });
        }

        private void PicoLoader_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tasksRunning)
            {
                if (MessageBox.Show("There are still running tasks. Close anyway?", "Running tasks",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}