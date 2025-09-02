using SolarSystemMapper;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SolarMapperUI
{
   
    public partial class SolarMapperMainForm : Form
    {


        private Panel _mainMapPanel;

        private SateliteMap _sateliteMap = null;
        private List<ObjectEntry> ObjectEntries = new List<ObjectEntry>(DataTables.Planets);
        internal enum MapType
        {
            NightSky, SolarSystem
        }

        private MapType mainMapType = MapType.NightSky  ;

        private ControlForm _controlForm;

        public SolarMapperMainForm()
        {
            InitializeComponent();
            

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.Bounds = Screen.PrimaryScreen.Bounds;


            ObjectEntries = ObjectEntries.Union(DataTables.Stars).ToList();
            ObjectEntries = ObjectEntries.Union(DataTables.DwarfPlanets).ToList();
            foreach (ObjectEntry entry in ObjectEntries) Debug.WriteLine(entry);


            this._setUpMainMapPanel(ObjectEntries.ToList(), DateTime.Today);

            this.Load += this.SolarSystemMap_Load;
            this.KeyPreview = true; // pøeposílá klávesy na form
            this.KeyDown += this.SolarMapperUI_KeyDown;

        }

        private void _setUpMainMapPanel(List<ObjectEntry> entries, DateTime date)
        {
            //entries.UnionWith(DataTables.Spacecrafts);
            _mainMapPanel = (this.mainMapType == MapType.NightSky) ? new NightSkyMapPanel(entries.Where(x => x.Name != "Earth").ToList(), date) : new SolarSystemMapPanel(entries.Where(x => x.Type != "Moon").ToList(), date);
            //_mainMapPanel.Paint += DrawPanel_Paint; // pøipojení události Paint

            if (_mainMapPanel is IMap map)
            {
                this._controlForm = new ControlForm(map);
                map.MapSwitch += ShowMoonPanel;
            }
            this.Controls.Add(_mainMapPanel);
        }

        private void _destroyMainMapPanel()
        {
            if (_mainMapPanel != null && _mainMapPanel is IMap map) map.CleanAndDispose();
            this._controlForm.Close();
            this._controlForm.Dispose();
            this.Controls.Remove(_mainMapPanel);

        }

        private void setUpMoonMapPanel(List<ObjectEntry> entries, DateTime date, NASAHorizonsDataFetcher.MapMode mode, string centerName)
        {
            this._sateliteMap = new SateliteMap(entries, date, mode, centerName);
            _controlForm = new ControlForm(_sateliteMap);
            this.Controls.Add( _sateliteMap);
            _sateliteMap.MapSwitch += ShowMainPanel;

        }

        private void _destroyMoonPanel() 
        {
            if (this._sateliteMap == null) return;
            this._sateliteMap.CleanAndDispose();
            this._controlForm.Close();
            this._controlForm.Dispose();
            this.Controls.Remove(_sateliteMap);
            
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
                (_mainMapPanel.Width - closingLabel.PreferredWidth) / 2,
                (closingLabel.PreferredHeight)
            );
            var hideTimer = new System.Windows.Forms.Timer();
            hideTimer.Interval = 3000;
            _mainMapPanel.Controls.Add(closingLabel);
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

        private void ShowMoonPanel(object sender, SwitchViewRequestedEvent e)
        {
            this._destroyMainMapPanel();
            this.setUpMoonMapPanel(e.ObjectEntries, e.Date, e.MapMode, e.CenterName);
        }

        private void ShowMainPanel(object sender, SwitchViewRequestedEvent e)
        {
            this._destroyMoonPanel();   
            this._setUpMainMapPanel(this.ObjectEntries, e.Date);
        }




    }
}
