using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Conformist.Configuration;

namespace Conformist
{
    public partial class MainForm : Form
    {
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
            var defaultFont = GetConsoleFont(displayConfig);

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
                var consoleFont = GetConsoleFont(processConfig.Display, defaultFont);
                consoleControl.Font = consoleFont;
                consoleControl.StartProcess(processConfig.Path, null);
            }
        }

        private static Font GetConsoleFont(DisplayConfig displayConfig, Font defaultFont = null)
        {
            // This is the default
            var defaultSize = 9;
            var font = default(Font);

            if (!String.IsNullOrWhiteSpace(displayConfig?.FontName))
            {
                var fontSize = defaultSize;
                if (displayConfig.FontSize > 0)
                    fontSize = displayConfig.FontSize;

                font = new Font(displayConfig.FontName, fontSize, FontStyle.Regular);
            }

            return font ?? defaultFont ?? new Font(FontFamily.GenericMonospace, defaultSize, FontStyle.Regular);
        }
    }
}
