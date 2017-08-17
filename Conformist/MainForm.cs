using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using Conformist.Configuration;

namespace Conformist
{
    public partial class MainForm : Form
    {
        public bool Followcursor { get; set; } = true;
        private List<ConsoleControl> _consoleControls { get; set; }
        public MainForm()
        {
            InitializeComponent();
            DisplayBuildInformation();
            StartProcesses();
        }
        private void DisplayBuildInformation()
        {
            this.Text += $" v{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}";
            this.Text += $" Build: {Metadata.BuildNumber} ({Metadata.BuildDate.ToString("yyyy-MM-dd HH:mm")})";
        }

        private void StartProcesses()
        {
            var processConfigs = ProcessConfigSection.GetSection("Processes");
            var displayConfig = DisplayConfigSection.GetSection("ConsoleDisplay");
            _consoleControls = new List<ConsoleControl>();
            foreach (var processConfig in processConfigs)
            {                tabControl.TabPages.Add(processConfig.Name, processConfig.Name);
                var tabPage = tabControl.TabPages[processConfig.Name];
                var console = new ConsoleControl
                {
                    Name = processConfig.Name,
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Top |
                             AnchorStyles.Left | AnchorStyles.Right,
                    Dock = DockStyle.Fill,
                    IsFollowBottomCursor = true
                };
                tabPage.Controls.Add(console);
                console.StartProcess(processConfig.Path, null);
                _consoleControls.Add(console);
            }
        }
        private void btnCursor_Click(object sender, EventArgs e)
        {
            Followcursor = !Followcursor;
            _consoleControls.ForEach(c => c.IsFollowBottomCursor = Followcursor);
            if (Followcursor) btnCursor.Text = @"Following Bottom";
            else btnCursor.Text = @"Cursor following your dreams and desires";
        }
    }
}
