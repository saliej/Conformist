using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace Conformist
{
    public partial class ConsoleControl : UserControl
    {        public bool IsFollowBottomCursor { get; set; }
        public ConsoleControl()
        {
            InitializeComponent();
        }
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
            if (dataGrid.InvokeRequired)
            {
                dataGrid.BeginInvoke(new Action<DataReceivedEventArgs>(WriteOutput), e);
            }
            else
            {
                var text = e.Data + Environment.NewLine;
                AddRow(text);
            }
        }
        private void AddRow(string text)
        {
            var row = new DataGridViewRow();
            row.Cells.Add(new DataGridViewTextBoxCell());
            row.SetValues(text);
            if (text.Contains("ERROR") || text.Contains("FATAL"))
            {
                row.DefaultCellStyle.BackColor = Color.Red;
                row.DefaultCellStyle.ForeColor = Color.White;
            }
            if (text.Contains("WARN"))
            {
                row.DefaultCellStyle.BackColor = Color.Chocolate;
                row.DefaultCellStyle.ForeColor = Color.White;
            }
            dataGrid.Rows.Add(row);
            if (IsFollowBottomCursor)
            {
                dataGrid.CurrentCell = row.Cells[0];
            }
        }
        private void Process_Exited(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                //richTextBoxConsole.Font = value;
            }
        }
    }
}