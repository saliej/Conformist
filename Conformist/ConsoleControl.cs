using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;


namespace Conformist
{
    public partial class ConsoleControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleControl"/> class.
        /// </summary>
        public ConsoleControl()
        {
            //  Initialise the component.
            InitializeComponent();
        }

        /// <summary>
        /// Runs a process.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="arguments">The arguments.</param>
        public void StartProcess(string fileName, string arguments)
        {
            var processStartInfo = new ProcessStartInfo(fileName, arguments);

            processStartInfo.UseShellExecute = false;
            processStartInfo.ErrorDialog = false;
            processStartInfo.CreateNoWindow = true;

            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;

            var process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = processStartInfo
            };
            process.Exited += Process_Exited;
            process.OutputDataReceived += (sender, e) =>
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    WriteOutput(e);
                }
            };

            try
            {
                process.Start();
                process.BeginOutputReadLine();
            }
            catch (Exception e)
            {
                Trace.WriteLine("Failed to start process " + fileName + " with arguments '" + arguments + "'");
                Trace.WriteLine(e.ToString());
            }
        }

        private void WriteOutput(DataReceivedEventArgs e)
        {
            if (richTextBoxConsole.InvokeRequired)
                richTextBoxConsole.BeginInvoke(new Action<DataReceivedEventArgs>(WriteOutput), e);
            else
            {
                richTextBoxConsole.AppendText(e.Data + Environment.NewLine);
                richTextBoxConsole.ScrollToCaret();
            }
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        public override Font Font
        {
            get {  return base.Font; }
            set
            {
                base.Font = value;
                richTextBoxConsole.Font = value;
            }
        }
    }
}