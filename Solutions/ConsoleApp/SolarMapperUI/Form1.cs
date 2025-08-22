using SolarSystemMapper;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SolarMapperUI
{
    public partial class SolarMapperUI : Form
    {


        private Panel _mapPanel;

        private enum MapType
        {
            NightSky, SolarSystem
        }

        private MapType mapType = MapType.SolarSystem;

        private ControlForm _controlForm;

        public SolarMapperUI()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.Bounds = Screen.PrimaryScreen.Bounds;

            // Vytvoøení panelu, který vyplní celé okno
            var entries = new HashSet<ObjectEntry>(DataTables.Planets);
            entries.UnionWith(DataTables.Stars);
            _mapPanel =  (this.mapType == MapType.NightSky) ? new NightSkyMapPanel(entries.ToList(),DateTime.Today) : new SolarSystemMapPanel(entries.ToList(), DateTime.Today);
            //_mapPanel.Paint += DrawPanel_Paint; // pøipojení události Paint

            if (_mapPanel is IMap map) _controlForm = new ControlForm(map);


            this.Controls.Add(_mapPanel);
            this.Load += this.SolarSystemMap_Load;
            this.KeyPreview = true; // pøeposílá klávesy na form
            this.KeyDown += SolarMapperUI_KeyDown;

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

        //protected void ShowFloatingControl()
        //{
        //    if (_mapPanel is IMap map)
        //    {


        //        var overlayForm = new Form();
        //        overlayForm.Text = "Advance Control";
        //        overlayForm.StartPosition = FormStartPosition.Manual;
        //        overlayForm.BackColor = Color.Black;
        //        overlayForm.ForeColor = Color.White;
        //        overlayForm.FormBorderStyle = FormBorderStyle.FixedDialog;
        //        overlayForm.MaximizeBox = false;
        //        overlayForm.MinimizeBox = false;
        //        overlayForm.ShowInTaskbar = false;
        //        overlayForm.AutoSize = true;
        //        overlayForm.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        //        var label = new Label();
        //        label.AutoSize = true;
        //        label.Location = new Point(10, 10);
        //        label.ForeColor = Color.LimeGreen;

        //        label.Text = map.CurrentPictureDate.ToString();

        //        overlayForm.Controls.Add(label);

        //        // tlaèítko pro posun mapy
        //        var oneStepButton = new System.Windows.Forms.Button();
        //        oneStepButton.Text = ">>";
        //        oneStepButton.AutoSize = true;
        //        oneStepButton.Location = new Point(10, 40);
        //        oneStepButton.Click += (s, e) =>
        //        {
        //            map.AdvanceMap();
        //            label.Text = map.CurrentPictureDate.ToString();
        //        };
        //        overlayForm.Controls.Add(oneStepButton);

        //        System.Windows.Forms.Timer autoTimer = new System.Windows.Forms.Timer();
        //        autoTimer.Interval = 500; 
        //        bool timerRunning = false;

        //        var autoButton = new Button();
        //        autoButton.Text = "Start";
        //        autoButton.AutoSize = true;
        //        autoButton.Location = new Point(10, 80); // vedle oneStepButton
        //        autoButton.Click += (s, e) =>
        //        {
        //            if (!timerRunning)
        //            {
        //                autoTimer.Tick += (ts, te) =>
        //                {
        //                    map.AdvanceMap();
        //                    label.Text = map.CurrentPictureDate.ToString();
        //                };
        //                autoTimer.Start();
        //                autoButton.Text = "Stop";
        //                timerRunning = true;
        //            }
        //            else
        //            {
        //                autoTimer.Stop();
        //                autoButton.Text = "Start";
        //                timerRunning = false;
        //                autoTimer.Tick -= null; // odpojit všechny Tick handlery
        //            }
        //        };
        //        overlayForm.Controls.Add(autoButton);



        //        // pozice overlay nad MapPanel
        //        var panelArea = this.ClientRectangle;
        //        overlayForm.Location = this.PointToScreen(new Point(
        //            panelArea.Right - overlayForm.Width - 20,
        //            panelArea.Top + 20
        //        ));


               
        //        overlayForm.Show(this);
        //    }
        //}
        private void SolarMapperUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab) // stisk Tab spustí overlay
            {
                if (_controlForm != null)
                {
                    // pokud už byl skrytý, zobrazíme ho
                    if (!_controlForm.Visible)
                    {
                        _controlForm.Show(this); // zobrazí nad hlavním formuláøem
                    }
                    _controlForm.BringToFront();
                }

            }
        }




    }
}
