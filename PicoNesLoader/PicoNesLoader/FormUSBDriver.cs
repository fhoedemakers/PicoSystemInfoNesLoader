using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicoNesLoader
{
    public partial class FormUSBDriver : Form
    {
        public string appFolder { get { return Path.GetDirectoryName(Application.ExecutablePath); } }
        public FormUSBDriver()
        {
            InitializeComponent();
        }

        private void FormUSBDriver_Load(object sender, EventArgs e)
        {

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonInstall_Click(object sender, EventArgs e)
        {
            var driverDir = Path.Combine(appFolder, "driver");
            var executable = Path.Combine(driverDir, "zadig-2.8.exe");

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = executable,
                        UseShellExecute = true,
                        RedirectStandardOutput = false,
                        CreateNoWindow = false,
                        Verb = "runas",
                        WorkingDirectory = driverDir
                    }
                };
                process.Start();
                //var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cannot start Driver Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }
    }
}
