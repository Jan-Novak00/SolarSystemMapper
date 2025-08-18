using SolarSystemMapper;
using System.Diagnostics;
using System.Windows.Forms;

namespace SolarMapperUI
{
    public partial class SolarMapperUI : Form
    {

        private MapPanel _mapPanel;

        private enum MapType
        {
            NightSky, SolarSystem
        }

        private MapType mapType = MapType.NightSky;

        public SolarMapperUI()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.Bounds = Screen.PrimaryScreen.Bounds;

            // Vytvoøení panelu, který vyplní celé okno
            _mapPanel = (mapType == MapType.SolarSystem) ? new SolarSystemMapPanel() : new NightSkyMapPanel();
            //_mapPanel.Paint += DrawPanel_Paint; // pøipojení události Paint
            this.Controls.Add(_mapPanel);
            this.Load += this.SolarSystemMap_Load;

        }

        private void _showClosingMessage()
        {
            var closingLabel = new Label
            {
                Text = "The app may be closed by pressing Esc key.",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Umístìní labelu do støedu panelu
            closingLabel.Location = new Point(
                (_mapPanel.Width - closingLabel.PreferredWidth) / 2,
                (closingLabel.PreferredHeight)
            );
            var hideTimer = new System.Windows.Forms.Timer();
            hideTimer.Interval = 3000;
            _mapPanel.Controls.Add(closingLabel);
            hideTimer.Tick += (s, e) =>
            {
                closingLabel.Visible = false;
                hideTimer.Stop();
            };
            hideTimer.Start();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SolarSystemMap_Load(object sender, EventArgs e)
        {
            this._showClosingMessage();
        }

        


    }
}
