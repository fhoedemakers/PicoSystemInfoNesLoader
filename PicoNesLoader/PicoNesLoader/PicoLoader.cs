using System.Diagnostics;

namespace PicoNesLoader
{
    public partial class PicoLoader : Form
    {
        public List<NesRom> romList = new List<NesRom>();
        public PicoLoader()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var NesRom = new NesRom(@"roms\CVIII.nes");
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
    }
}