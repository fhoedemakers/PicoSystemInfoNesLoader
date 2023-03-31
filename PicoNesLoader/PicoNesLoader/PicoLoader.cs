using Newtonsoft.Json.Linq;
using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Formats.Tar;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace PicoNesLoader
{

    public partial class PicoLoader : Form
    {
        private const string flashProgramName = "PicoSystem_InfoNes";
        private const long defaultMaxTarSize = 12 * 1024 * 1024;
        private const long defaultFlashStart = 0x10110000;
        private enum PicoStatus { OK, NoBootSel, NeedDriver, UnknownError };

        bool tasksRunning = false;
        bool alreadyAsked = false;

        private PicoStatus picoStatus;
        private string outputOfPicoTool = string.Empty;
        private string outputOfPicoToolFlash = string.Empty;
        private long MaxTarSize = defaultMaxTarSize;
        const string NoDriverInstalled = "You may need to install a driver.";
        const string BootSelNotEnabled = "No accessible RP2040 devices in BOOTSEL mode were found.";

        string infoLabelText = @"This is a helper tool for adding multiple NES roms to the PicoSystem_InfoNes NES emulator running on the Pimoroni PicoSystem handheld.";
        private long totalTarSize;

        public string appFolder { get { return Path.GetDirectoryName(Application.ExecutablePath); } }
        public SortableBindingList<NesRom> romList = new SortableBindingList<NesRom>();

        RP2040 picoInfo;
        private GitHub gh = new GitHub("fhoedemakers", "PicoSystem_InfoNes");
        private string _tempDir = string.Empty;
        private string latestPicoSystem_InfoNesReleaseUrl;

        Progress<ProgressReport> progress;
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
            ResetInfoLabels();
            //Debug.Print("{0}", process.ExitCode);
            if (process.ExitCode == 0)
            {
                var lines = outputOfPicoTool.Split(Environment.NewLine);
                picoInfo = new RP2040(lines);
                MaxTarSize = picoInfo.ProgramBinaryStart + picoInfo.FlashSizeBytes - defaultFlashStart + 1;
                SetInfoLabels();

            }
            else
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
            if (picoStatus!=PicoStatus.OK && rval == PicoStatus.OK) {
                alreadyAsked = false; // Trigger new update check
            }
            picoStatus = rval;
            return;
        }

        private void SetInfoLabels()
        {
            labelProgramName.Text = picoInfo.ProgramName;
            labelProgramVersion.Text = picoInfo.ProgramVersion;
            labelFlashSize.Text = $"{picoInfo.FlashSizeInKBytes}K";
            labelFlashBinaryStart.Text = picoInfo.ProgramBinaryStartHex;
            labelFlashBinaryEnd.Text = picoInfo.ProgramBinaryEndHex;
        }

        private void PicoLoader_Load(object sender, EventArgs e)
        {
            //labelInfo.Text = infoLabelText;
            linkLabelUpdate.Visible = false;
            ResetInfoLabels();
            growLabel1.Text = infoLabelText;
            toolStripStatusLabelLinkToDriver.Visible = false;
            nesRomBindingSource.DataSource = romList;
            checkPicoSystem();
            displayPicoStatus();
            CalculateTarSize();
            timerCheckPico.Enabled = true;
            toolStripProgressBar1.Visible = false;
            progress = new Progress<ProgressReport>(ReportProgress);
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
                    linkLabelUpdate.Visible = false;
                    toolStripStatusLabelCheckPico.Text = "PicoSystem not connected! Please connect device to an USB port, then press X and power on device.";
                    break;
                case PicoStatus.NeedDriver:
                    buttonCreateTar.Enabled = false;
                    toolStripStatusLabelCheckPico.Text = "Cannot connect to PicoSystem! Please install USB driver!";
                    toolStripStatusLabelLinkToDriver.Visible = true;
                    linkLabelUpdate.Visible = false;
                    break;
                default:
                    buttonCreateTar.Enabled = false;
                    MessageBox.Show(outputOfPicoTool, "PicoSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    linkLabelUpdate.Visible = false;
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
                tasksRunning = true;
                await Task.Run(
                    () => LoadFiles(progress));
                tasksRunning = false;
                dataGridView1.DataSource = romList;
                // dataGridView1.Sort(dataGridView1.Columns["dataGridViewTextBoxColumnName"], ListSortDirection.Ascending);
                CalculateTarSize();
                toolStripStatusLabelCheckPico.Text = "Done.";
                panelButtons.Enabled = true;
            }

            timerCheckPico.Enabled = true;
        }

        private void LoadFiles(IProgress<ProgressReport> progress)
        {
            ProgressReport report = new ProgressReport() { Complete = 0, info = "Loading files." };
            List<NesRom> list = new List<NesRom>();
            int i = 0;
            list.AddRange(romList);
            foreach (var file in openFileDialogNES.FileNames)
            {
                list.Add(new NesRom(file));
                i++;
                report.Complete = (i * 100) / openFileDialogNES.FileNames.Length;
                progress.Report(report);
            }

            var distinctList = list.Distinct().ToList();
            distinctList.Sort();

            romList.Clear();

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
            var invalidRoms = romList.Count(x => x.ValidRom != NesRom.RomType.Valid);

            if (romList.Count - invalidRoms > 0)
            {
                if (invalidRoms > 0)
                {
                    var result = MessageBox.Show("Roms the emulator cannot run will be skipped.", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (result != DialogResult.OK)
                    {
                        return;
                    }
                }
                groupBoxList.Enabled= false;
                timerCheckPico.Enabled = false;
                tasksRunning = true;
                panelButtons.Enabled = false;
                await Task.Run(() => ProcessToPicoSystem(progress));
                tasksRunning = false;
                if (!string.IsNullOrEmpty(outputOfPicoToolFlash))
                {
                    MessageBox.Show(outputOfPicoToolFlash, "Error flashing PicoSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                panelButtons.Enabled = true;
                timerCheckPico.Enabled = true;
                groupBoxList.Enabled = true;
            }

        }

        private void ProcessToPicoSystem(IProgress<ProgressReport> progress)
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
                        progress.Report(report);
                        i++;
                    }
                    report.Complete = 100;
                    progress.Report(report);
                    report.Complete = 0;
                    report.info = "Creating archive";
                    progress.Report(report);
                    TarFile.CreateFromDirectory(tempDir, tarFileName, false);
                    report.Complete = 100;
                    progress.Report(report);
                    //FileInfo info = new FileInfo(tarFileName);
                    //Debug.Print("{0}", labelTotalSize.Text);
                    //Debug.Print("Tarfile size: {0}", info.Length);
                    //new TarInspector(tarFileName); 
                    PicoFlash(tarFileName, "bin", defaultFlashStart, progress);
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
                    progress.Report(report);
                }
            }

        }
        private async void timerCheckPico_Tick(object sender, EventArgs e)
        {
            toolStripProgressBar1.Visible = false;
            timerCheckPico.Enabled = false;
            checkPicoSystem();
            displayPicoStatus();
            CalculateTarSize();

            if (!alreadyAsked && picoStatus == PicoStatus.OK)
            {
                try
                {
                    alreadyAsked = true;
                    linkLabelUpdate.Visible = await isUpdateAvailableAsync();
                    if (picoInfo.ProgramName != flashProgramName)
                    {
                        await FlashNewUf2ToPico();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Cannot connect to GitHub: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            timerCheckPico.Enabled = true;

        }
        private async Task<bool> FlashNewUf2ToPico()
        {
            var result = MessageBox.Show("Install InfoNes emulator on Pimoroni PicoSystem?", "PicoSystem_InfoNes not installed.", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                tasksRunning = true;
                panelButtons.Enabled = false;
                await FlashUf2ToPicoSystemAsync(progress);
                panelButtons.Enabled = true;
                tasksRunning = false;
                linkLabelUpdate.Visible = false;
                return true;
            } else { return false; }
        }

        private async Task FlashUf2ToPicoSystemAsync(IProgress<ProgressReport> progress)
        {
            ProgressReport report = new ProgressReport() { Complete = 0, info = "Downloading .uf2" };
            progress.Report(report);
            string filename = $"{Path.GetTempFileName()}.uf2";
            await DownloadFileAsync(latestPicoSystem_InfoNesReleaseUrl, filename);
            await Task.Run(() =>
            {
                PicoFlash(filename, "uf2", picoInfo.ProgramBinaryStart, progress);
            });
            if (!string.IsNullOrEmpty(outputOfPicoToolFlash))
            {
                MessageBox.Show(outputOfPicoToolFlash, "Error flashing PicoSystem",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


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

        private void ResetInfoLabels()
        {
            labelProgramName.Text =
                labelProgramVersion.Text =
                labelFlashSize.Text =
                labelFlashBinaryStart.Text =
                labelFlashBinaryEnd.Text = "N/A";
            MaxTarSize = defaultMaxTarSize;
        }

        private async Task<bool> isUpdateAvailableAsync()
        {
            JObject release = await gh.GetLatestReleaseAsync();

            latestPicoSystem_InfoNesReleaseUrl = (from asset in release["assets"]
                                                  where asset["name"].Value<string>() == "PicoSystem_InfoNes.uf2"
                                                  select asset["browser_download_url"].Value<string>()).FirstOrDefault();
            string version = release["tag_name"].Value<string>();
            if (version != picoInfo.ProgramVersion)
            {
                return true;
            }
            return false;
        }

        void ReportProgress(ProgressReport value)
        {
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.ProgressBar.Value = value.Complete;
            toolStripStatusLabelCheckPico.Text = value.info;
        }

        public async Task DownloadFileAsync(string url, string pathToSave)
        {
            File.Delete(pathToSave);
            HttpClient downloadClient = new HttpClient();
            var httpResult = await downloadClient.GetAsync(url);
            using (var resultStream = await httpResult.Content.ReadAsStreamAsync())
            {
                using (var fileStream = System.IO.File.Create(pathToSave))
                {
                    resultStream.CopyTo(fileStream);
                }
            }
        }

        public void PicoFlash(string filename, string binaryType, long address, IProgress<ProgressReport> progress)
        {
            outputOfPicoToolFlash = string.Empty;
            ProgressReport report = new ProgressReport();
            report.Complete = 0;
            report.info = "Uploading to PicoSystem";
            progress.Report(report);
            // picotool load rom.nes -t bin -o 0x10110000
            var executable = Path.Combine(appFolder, "PicoTool\\picotool.exe");
            string offsetArg = string.Empty;
            if (binaryType == "bin")
            {
                offsetArg = $" -o 0x{address.ToString("X")}";
            }
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = executable,
                    Arguments = $"load {filename} -t {binaryType}{offsetArg}",
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
                try
                {
                    var array = line.Split(']');
                    if (array.Length == 2)
                    {
                        report.Complete = int.Parse(array[1].Substring(0, array[1].Length - 1));
                        progress.Report(report);
                    }
                }
                catch { };
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
            progress.Report(report);
        }

        private async void linkLabelUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            timerCheckPico.Enabled = false;
            try
            {
                if (await FlashNewUf2ToPico())
                {
                    linkLabelUpdate.Visible = false;
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot connect to GitHub: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            timerCheckPico.Enabled = true;
        }
    }


}