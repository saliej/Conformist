using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Conformist.Configuration;

namespace Conformist
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            StartProcesses();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void StartProcesses()
        {
            var processConfigs = ProcessConfigSection.GetSection("Processes");

            foreach (var processConfig in processConfigs)
            {
                tabControl.TabPages.Add(processConfig.Name, processConfig.Name);

                var tabPage = tabControl.TabPages[processConfig.Name];
                var consoleControl = new ConsoleControl
                {
                    Name = processConfig.Name,
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Top |
                             AnchorStyles.Left | AnchorStyles.Right,
                    Dock = DockStyle.Fill
                };

                tabPage.Controls.Add(consoleControl);
                var consoleFont = new Font(FontFamily.GenericMonospace, 9, FontStyle.Regular);
                consoleControl.Font = consoleFont;
                consoleControl.StartProcess(processConfig.Path, null);
            }
        }
    }
}
