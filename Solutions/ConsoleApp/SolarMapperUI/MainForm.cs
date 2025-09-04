using SolarSystemMapper;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SolarMapperUI
{
   
    public partial class SolarMapperMainForm : Form
    {


        private Panel _mainMapPanel;

        private SateliteMap _sateliteMap = null;
        private List<ObjectEntry> ObjectEntries;
        internal enum MapType
        {
            NightSky, SolarSystem
        }

        private MapType mainMapType = MapType.SolarSystem;

        private ControlForm _controlForm;

        private GeneralMapSettings _mapSettings;

        private IEnumerable<Func<IEnumerable<IFormBody<IEphemerisData<IEphemerisTableRow>>>, IEnumerable<IFormBody<IEphemerisData<IEphemerisTableRow>>>>> _typeFilters;
        internal SolarMapperMainForm(GeneralMapSettings generalMapSettings, IEnumerable<TypeSettings> typeSettings)
        {
            InitializeComponent();


            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.Bounds = Screen.PrimaryScreen.Bounds;

            this.mainMapType = generalMapSettings.MapType;
            
            _mapSettings = generalMapSettings;
            _typeFilters = typeSettings.Select(x=>x.linqFilter);
            _setUpMainPanel(_mapSettings);
            this.Load += this.SolarSystemMap_Load;
            this.KeyPreview = true; // pøeposílá klávesy na form
            this.KeyDown += this.SolarMapperUI_KeyDown;


        }


        public SolarMapperMainForm()
        {
            InitializeComponent();
            

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.Bounds = Screen.PrimaryScreen.Bounds;

            ObjectEntries = DataTables.TerrestrialPlanets.ToList();
            ObjectEntries = ObjectEntries.Union(DataTables.Stars).ToList();
            ObjectEntries = ObjectEntries.Union(DataTables.DwarfPlanets).ToList();


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

        private void _setUpMainPanel(GeneralMapSettings mapSettings)
        {
            _mainMapPanel = (this.mainMapType == MapType.NightSky)
                            ? new NightSkyMapPanel(
                                mapSettings,
                                _typeFilters.Select(f => new Func<IEnumerable<IFormBody<EphemerisObserverData>>,
                                                              IEnumerable<IFormBody<EphemerisObserverData>>>(data =>
                                    f(data.Cast<IFormBody<IEphemerisData<IEphemerisTableRow>>>())
                                     .Cast<IFormBody<EphemerisObserverData>>()
                                ))
                            )
                            : new SolarSystemMapPanel(
                                mapSettings,
                                _typeFilters.Select(f => new Func<IEnumerable<IFormBody<EphemerisVectorData>>,
                                                              IEnumerable<IFormBody<EphemerisVectorData>>>(data =>
                                    f(data.Cast<IFormBody<IEphemerisData<IEphemerisTableRow>>>())
                                     .Cast<IFormBody<EphemerisVectorData>>()
                                ))
                            );



            if (_mainMapPanel is IMap map)
            {
                this._controlForm = new ControlForm(map);
                map.MapSwitch += ShowMoonPanel;
            }
            this.Controls.Add(_mainMapPanel);
        }
       

        private void _destroyMainMapPanel()
        {
            if (_mainMapPanel != null && _mainMapPanel is IMap map)
            {
                this.ObjectEntries = map.ObjectEntries;
                map.CleanAndDispose();
            }
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
