using Newtonsoft.Json.Linq;
using PicoSystemInfoNesLoader;
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
        #region constants
       
        private const string flashProgramName = "PicoSystem_InfoNes";
        private const string uiProgramName = "PicoSystemInfoNesLoader";
        private const long defaultMaxTarSize = 12 * 1024 * 1024;
        private const long defaultFlashStart = 0x10110000;
        const string NoDriverInstalled = "You may need to install the USB driver.";
        const string BootSelNotEnabled = "No accessible RP2040 devices in BOOTSEL mode were found.";
        private const string infoLabelText = @"This is a helper tool for adding multiple NES roms to the PicoSystem_InfoNes NES emulator running on the Pimoroni PicoSystem handheld.";
        #endregion

        #region fields
        private string programVersion = VersionInfo.CurrentVersion;
        private string latestUiVersion;
        private string latestUf2Version;
        private long MaxTarSize = defaultMaxTarSize;  // Maximum size of archive containing roms
        private long totalTarSize;
        private enum PicoSystemStatus { OK, NoBootSel, NeedDriver, UnknownError };
        private PicoSystemStatus picoSystemStatus;

        private bool tasksRunning = false;
        private bool alreadyAskedForUpdate = false;
        private bool alreadyAskedForUiUpdate = false;
        private string outputOfPicoTool = string.Empty;
        private string outputOfPicoToolFlash = string.Empty;
        private SortableBindingList<NesRom> romList = new SortableBindingList<NesRom>();
        private RP2040 picoSystemInfo;
        private GitHub gh = new GitHub("fhoedemakers", "PicoSystem_InfoNes");
        private GitHub ghUI = new GitHub("fhoedemakers", "PicoSystemInfoNesLoader");
        private string? latestPicoSystem_InfoNesReleaseUrl;
        private string? latestUi_ReleaseUrl;
        private Progress<ProgressReport> progress;
        #endregion

        #region properties
        private string _tempDir = string.Empty;
        /// <summary>
        /// Get a temporary directoy name
        /// </summary>
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

        /// <summary>
        /// Get path of PicoTool executable.
        /// </summary>
        public string picoTool
        {
            get
            {
                return Path.Combine(appFolder, "PicoTool\\picotool.exe");
            }
        }
        /// <summary>
        /// Get location of the folder the executable is in.
        /// </summary>
        public string appFolder { 
            get { 
                return Path.GetDirectoryName(Application.ExecutablePath); 
            } 
        }
        #endregion
        public PicoLoader()
        {
            InitializeComponent();

        }
        #region Form Events
        private void PicoLoader_Load(object sender, EventArgs e)
        {
            labelAppVersion.Text = programVersion;
            linkLabelUpdateUf2.Visible = false;
            ResetPicoSystemInfoLabels();
            growLabel1.Text = infoLabelText;
            toolStripStatusLabelLinkToDriver.Visible = false;
            nesRomBindingSource.DataSource = romList;
            CheckPicoSystemStatus();
            DisplayPicoStatus();
            CalculateTarSize();
            timerCheckPico.Enabled = true;
            toolStripProgressBar1.Visible = false;
            progress = new Progress<ProgressReport>(ReportProgress);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this?.Close();
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

            if (openFileDialogNES?.ShowDialog() == DialogResult.OK && openFileDialogNES.FileNames.Length > 0)
            {
                dataGridView1.DataSource = null;
                panelButtons.Enabled = false;
                tasksRunning = true;
                await Task.Run(
                    () => LoadFilesIntoGridView(progress));
                tasksRunning = false;
                dataGridView1.DataSource = romList;
                // dataGridView1.Sort(dataGridView1.Columns["dataGridViewTextBoxColumnName"], ListSortDirection.Ascending);
                CalculateTarSize();
                toolStripStatusLabelCheckPico.Text = "Done.";
                panelButtons.Enabled = true;
            }

            timerCheckPico.Enabled = true;
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
                    var result = MessageBox.Show("Some roms on the list cannot be run by the emulator and will be skipped.", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (result != DialogResult.OK)
                    {
                        return;
                    }
                }
                groupBoxList.Enabled= false;
                timerCheckPico.Enabled = false;
                tasksRunning = true;
                panelButtons.Enabled = false;
                await Task.Run(() => CreateArchiveAndFlash(progress));
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
       
        /// <summary>
        /// timer call back for checking wheter a PicoSystem is connected
        /// Checks also for new updates of the emulator.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void timerCheckPico_Tick(object sender, EventArgs e)
        {
            toolStripProgressBar1.Visible = false;
            timerCheckPico.Enabled = false;
            CheckPicoSystemStatus();
            DisplayPicoStatus();
            CalculateTarSize();
            // check only once for update
            if ( ! alreadyAskedForUiUpdate)
            {
                alreadyAskedForUiUpdate = true;
                if (await IsUpdateAvailableForUIAsync())
                {
                    var dResult = MessageBox.Show($"A new version ({latestUiVersion}) for this program is available.\r\nClose program and open download page?", "New version available.", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (dResult == DialogResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo("https://github.com/fhoedemakers/PicoSystemInfoNesLoader/releases/latest") { UseShellExecute = true });
                        Application.Exit();
                    }
                };
            }
            if (!alreadyAskedForUpdate && picoSystemStatus == PicoSystemStatus.OK)
            {
                try
                {
                    alreadyAskedForUpdate = true;
                   
                    linkLabelUpdateUf2.Visible = await IsUpdateAvailableForEmulatorAsync();
                    linkLabelUpdateUf2.Text = $"Update available ({latestUf2Version})";
                    if (picoSystemInfo.ProgramName != flashProgramName)
                    {
                        await AskforFlashNewUf2Async();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Cannot connect to GitHub: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            timerCheckPico.Enabled = true;

        }
       
        private void toolStripStatusLabelLinkToDriver_Click(object sender, EventArgs e)
        {
            timerCheckPico.Enabled = false;
            FormUSBDriver driverForm = new FormUSBDriver();
            driverForm?.ShowDialog();
            timerCheckPico.Enabled = true;
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            if (romList.Count > 0)
            {
                if (MessageBox.Show("Are you sure to clear the entire list?", "Clear list", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    romList?.Clear();
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

        private async void linkLabelUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            timerCheckPico.Enabled = false;
            try
            {
                if (await AskforFlashNewUf2Async())
                {
                    linkLabelUpdateUf2.Visible = false;
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot connect to GitHub: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            timerCheckPico.Enabled = true;
        }
        #endregion

        #region private functions
        /// <summary>
        /// Download a file from a given url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pathToSave">Filename to save</param>
        /// <returns></returns>
        public async Task DownloadFileAsync(string url, string pathToSave)
        {
            File.Delete(pathToSave);
            HttpClient downloadClient = new HttpClient();
            var httpResult = await downloadClient.GetAsync(url);
            using (var resultStream = await httpResult.Content.ReadAsStreamAsync())
            {
                using (var fileStream = System.IO.File.Create(pathToSave))
                {
                    resultStream?.CopyTo(fileStream);
                }
            }
        }
        /// <summary>
        /// Calculates the size of the archive to be created, 
        /// based on  the valid .nes entries in the gridview. The size cannot exceed the available flashe memory
        /// of the PicoSystem.
        /// </summary>
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
        /// <summary>
        /// Checks whether the PicoSystem is connected to the computer, and
        /// whether a driver install is needed.
        /// When connected, the object picoSystemInfo will contain info about the
        /// system.
        /// </summary>
        private void CheckPicoSystemStatus()
        {
            PicoSystemStatus rval = PicoSystemStatus.OK;

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = picoTool,
                    Arguments = "info -a",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            process.Start();
            outputOfPicoTool = process.StandardOutput.ReadToEnd();

            process.WaitForExit();
            ResetPicoSystemInfoLabels();
            //Debug.Print("{0}", process.ExitCode);
            if (process.ExitCode == 0)
            {
                try
                {
                    var lines = outputOfPicoTool.Split(Environment.NewLine);
                    picoSystemInfo = new RP2040(lines);
                    MaxTarSize = picoSystemInfo.ProgramBinaryStart + picoSystemInfo.FlashSizeBytes - defaultFlashStart + 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error parsing PicoSystem info: {ex.Message}\nApplication will exit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                    Application.Exit();
                }
                SetPicoSystemInfoLabels();

            }
            else
            {
                if (outputOfPicoTool.Contains(NoDriverInstalled))
                {
                    rval = PicoSystemStatus.NeedDriver;
                }
                else
                {
                    if (outputOfPicoTool.Contains(BootSelNotEnabled))
                    {
                        rval = PicoSystemStatus.NoBootSel;
                    }
                }
            }
            if (picoSystemStatus != PicoSystemStatus.OK && rval == PicoSystemStatus.OK)
            {
                alreadyAskedForUpdate = false; // Trigger new update check
            }
            picoSystemStatus = rval;
            return;
        }

        /// <summary>
        /// Create a tar archive containing .nes roms.
        /// This archive will be flashed to the PicoSystem
        /// </summary>
        /// <param name="progress"></param>
        private void CreateArchiveAndFlash(IProgress<ProgressReport> progress)
        {
            outputOfPicoToolFlash = string.Empty;
            var tarFileName = GetTemporaryFileName(".tar");

            var files = romList.Where(x => x.ValidRom == NesRom.RomType.Valid).Select(x => x.FullpathName);

            if (files?.Count() > 0)
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
                        progress?.Report(report);
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
                    progress?.Report(report);
                }
            }

        }
        /// <summary>
        /// Displays the status of the connected PicoSystem on the toolstrip
        /// </summary>
        private void DisplayPicoStatus()
        {
            toolStripStatusLabelLinkToDriver.Visible = false;
            buttonCreateTar.Enabled = (totalTarSize > 0 && totalTarSize <= MaxTarSize);
            switch (picoSystemStatus)
            {
                case PicoSystemStatus.OK:
                    toolStripStatusLabelCheckPico.Text = "PicoSystem connected!";
                    break;
                case PicoSystemStatus.NoBootSel:
                    buttonCreateTar.Enabled = false;
                    linkLabelUpdateUf2.Visible = false;
                    toolStripStatusLabelCheckPico.Text = "PicoSystem not connected! Please connect device to an USB port, then press X and power on device.";
                    break;
                case PicoSystemStatus.NeedDriver:
                    buttonCreateTar.Enabled = false;
                    toolStripStatusLabelCheckPico.Text = "Cannot connect to PicoSystem! Please install USB driver!";
                    toolStripStatusLabelLinkToDriver.Visible = true;
                    linkLabelUpdateUf2.Visible = false;
                    break;
                default:
                    buttonCreateTar.Enabled = false;
                    MessageBox.Show(outputOfPicoTool, "PicoSystem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    linkLabelUpdateUf2.Visible = false;
                    break;
            }
        }
        /// <summary>
        /// Asks user to update the PicoSystem with a new version of the emulator
        /// </summary>
        /// <returns></returns>
        private async Task<bool> AskforFlashNewUf2Async()
        {
            var result = MessageBox.Show($"Install latest version ({latestUf2Version}) of InfoNes emulator on Pimoroni PicoSystem?", "PicoSystem_InfoNes not installed.", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                tasksRunning = true;
                panelButtons.Enabled = false;
                await FlashUf2ToPicoSystemAsync(progress);
                panelButtons.Enabled = true;
                tasksRunning = false;
                linkLabelUpdateUf2.Visible = false;
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// Download and flash the emulator to the PicoSystem.
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        private async Task FlashUf2ToPicoSystemAsync(IProgress<ProgressReport> progress)
        {
            ProgressReport report = new ProgressReport() { Complete = 0, info = "Downloading .uf2" };
            progress.Report(report);
            string filename = $"{Path.GetTempFileName()}.uf2";
            await DownloadFileAsync(latestPicoSystem_InfoNesReleaseUrl, filename);
            await Task.Run(() =>
            {
                PicoFlash(filename, "uf2", picoSystemInfo.ProgramBinaryStart, progress);
            });
            if (!string.IsNullOrEmpty(outputOfPicoToolFlash))
            {
                MessageBox.Show(outputOfPicoToolFlash, "Error flashing PicoSystem",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Get a name for a temporary directory
        /// </summary>
        /// <returns></returns>
        public string GetTemporaryDirectoryName()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            return tempDirectory;
        }

        /// <summary>
        /// Get a name for a temporary filename with the given extension.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public string GetTemporaryFileName(string extension)
        {
            string filename = Path.Combine(Path.GetTempPath(), $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{extension}");
            return filename;
        }

        /// <summary>
        /// Check whether a new Version of the emulator is available on GitHub
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsUpdateAvailableForEmulatorAsync()
        {
            JObject release = await gh.GetLatestReleaseAsync();
            latestPicoSystem_InfoNesReleaseUrl = (from asset in release?["assets"]
                                                  where asset?["name"]?.Value<string>() == $"{flashProgramName?.ToLower()}.uf2"
                                                  select asset?["browser_download_url"]?.Value<string>())?.FirstOrDefault();
            latestUf2Version = release?["tag_name"]?.Value<string>();
            if (latestUf2Version?.CompareTo(picoSystemInfo.ProgramVersion) > 0 || picoSystemInfo.ProgramVersion == "0.1")
            { 
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check whether a new Version of the UI is available on GitHub
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsUpdateAvailableForUIAsync()
        {
            JObject release = await ghUI.GetLatestReleaseAsync();
            latestUi_ReleaseUrl = (from asset in release?["assets"]
                                                  where asset?["name"]?.Value<string>() == $"{uiProgramName}.zip"
                                                  select asset?["browser_download_url"]?.Value<string>())?.FirstOrDefault();
            latestUiVersion = release?["tag_name"]?.Value<string>();
            if (latestUiVersion?.CompareTo(programVersion) > 0 && programVersion != "DEVVERSION")
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Loads the gridview with the information of the selected .nes roms from the open file dialog.
        /// </summary>
        /// <param name="progress"></param>
        private void LoadFilesIntoGridView(IProgress<ProgressReport> progress)
        {
            ProgressReport report = new ProgressReport() { Complete = 0, info = "Loading files." };
            List<NesRom> list = new List<NesRom>();
            int i = 0;
            list.AddRange(romList);
            foreach (var file in openFileDialogNES.FileNames)
            {
                list?.Add(new NesRom(file));
                i++;
                report.Complete = (i * 100) / openFileDialogNES.FileNames.Length;
                progress?.Report(report);
            }
            // remove duplicates
            var distinctList = list.Distinct().ToList();
            distinctList.Sort();

            romList.Clear();

            foreach (var item in distinctList)
            {
                romList?.Add((NesRom)item);
            }
        }
        /// <summary>
        /// Flash a file to the PicoSystem using PicoTool.exe
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="binaryType">bin, uf2</param>
        /// <param name="address"></param>
        /// <param name="progress"></param>
        public void PicoFlash(string filename, string binaryType, long address, IProgress<ProgressReport> progress)
        {
            outputOfPicoToolFlash = string.Empty;
            ProgressReport report = new ProgressReport();
            report.Complete = 0;
            report.info = "Flashing PicoSystem";
            progress.Report(report);
            // picotool load rom.nes -t bin -o 0x10110000
            string offsetArg = string.Empty;
            if (binaryType == "bin")
            {
                offsetArg = $" -o 0x{address.ToString("X")}";
            }
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = picoTool,
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
            // Get the precentages from stdout and report back.
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                try
                {
                    var array = line?.Split(']');
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
            progress?.Report(report);
        }
        /// <summary>
        /// Report Progress of an async task.
        /// </summary>
        /// <param name="value"></param>
        void ReportProgress(ProgressReport value)
        {
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.ProgressBar.Value = value.Complete;
            toolStripStatusLabelCheckPico.Text = value.info;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ResetPicoSystemInfoLabels()
        {
            labelProgramName.Text =
                labelProgramVersion.Text =
                labelFlashSize.Text =
                labelFlashBinaryStart.Text =
                labelFlashBinaryEnd.Text = "N/A";
            MaxTarSize = defaultMaxTarSize;
        }
        /// <summary>
        /// Set info labels text with info from the picoSystemInfo object
        /// </summary>
        private void SetPicoSystemInfoLabels()
        {
            labelProgramName.Text = picoSystemInfo.ProgramName;
            labelProgramVersion.Text = picoSystemInfo.ProgramVersion;
            labelFlashSize.Text = $"{picoSystemInfo.FlashSizeInKBytes}K";
            labelFlashBinaryStart.Text = picoSystemInfo.ProgramBinaryStartHex;
            labelFlashBinaryEnd.Text = picoSystemInfo.ProgramBinaryEndHex;
        }
       
        #endregion
    }


}