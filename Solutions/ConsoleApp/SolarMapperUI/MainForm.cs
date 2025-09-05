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
    /**
     * Represents type of the main map
     */
    internal enum MapType
    {
        NightSky, SolarSystem
    }
    /**
     * Controls maps.
     */
    public partial class SolarMapperMainForm<TData> : Form
        where TData : IEphemerisData<IEphemerisTableRow>
    {
        /**
         * Main map.
         */
        private Panel _mainMapPanel;

        /**
         * Moon map
         */

        private SateliteMapPanel _sateliteMap = null;

        /**
         * ObejctEntry buffer - object don't have to be filtered again and again.
         */
        private List<ObjectEntry> ObjectEntries;
        

        private MapType mainMapType;

        private ControlForm _controlForm;

        private GeneralMapSettings _mapSettings;

        private IEnumerable<Func<IEnumerable<IFormBody<TData>>, IEnumerable<IFormBody<TData>>>> _typeFilters;


        internal SolarMapperMainForm(GeneralMapSettings generalMapSettings, IEnumerable<TypeSettings<TData>> typeSettings, Panel panel)
        {
            InitializeComponent();


            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.Bounds = Screen.PrimaryScreen.Bounds;

            this.mainMapType = generalMapSettings.MapType;
            
            _mapSettings = generalMapSettings;
            _typeFilters = typeSettings.Select(x=>x.linqFilter);
            _mainMapPanel = panel;
            if (_mainMapPanel is IMap map)
            {
                this._controlForm = new ControlForm(map); //registering control form
                map.MapSwitch += ShowMoonPanel;
            }
            this.Controls.Add(_mainMapPanel);
            this.Load += this.MainMap_Load;
            this.KeyPreview = true; 
            this.KeyDown += this.SolarMapperUI_KeyDown;


        }

        [Obsolete]
        /**
         * For testing purposes
         */
        public SolarMapperMainForm()
        {
            InitializeComponent();
            

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.mainMapType = (typeof(TData) == typeof(EphemerisObserverData)) ? MapType.NightSky : MapType.SolarSystem;

            ObjectEntries = DataTables.TerrestrialPlanets.ToList();
            ObjectEntries = ObjectEntries.Union(DataTables.Stars).ToList();
            ObjectEntries = ObjectEntries.Union(DataTables.DwarfPlanets).ToList();


            this._setUpMainMapPanel(ObjectEntries.ToList(), DateTime.Today);

            this.Load += this.MainMap_Load;
            this.KeyPreview = true; 
            this.KeyDown += this.SolarMapperUI_KeyDown;

        }
        /**
         * Used when switching form Moon map to main map
         */
        private void _setUpMainMapPanel(List<ObjectEntry> entries, DateTime date)
        {
            
            _mainMapPanel = (this.mainMapType == MapType.NightSky) ? new NightSkyMapPanel(entries.Where(x => x.Name != "Earth").ToList(), date) : new SolarSystemMapPanel(entries.Where(x => x.Type != "Moon").ToList(), date);
            

            if (_mainMapPanel is IMap map)
            {
                this._controlForm = new ControlForm(map);
                map.MapSwitch += ShowMoonPanel;
            }
            this.Controls.Add(_mainMapPanel);
        }


        /**
         * Used when exiting main map
         */
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
        /**
         * Used when opening moon map
         */
        private void _setUpMoonMapPanel(List<ObjectEntry> entries, DateTime date, NASAHorizonsDataFetcher.MapMode mode, string centerName)
        {
            this._sateliteMap = new SateliteMapPanel(entries, date, mode, centerName);
            _controlForm = new ControlForm(_sateliteMap);
            this.Controls.Add( _sateliteMap);
            _sateliteMap.MapSwitch += ShowMainPanel;

        }
        /**
         * Used if exiting moon map
         */
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

        private void MainMap_Load(object sender, EventArgs e)
        {
            this._showClosingMessage();
        }

        /**
         * Showing control panel
         */
        private void SolarMapperUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab) 
            {
                if (_controlForm != null)
                {
                   
                    if (!_controlForm.Visible)
                    {
                        _controlForm.Show(this); 
                    }
                    _controlForm.BringToFront();
                }

            }
        }
        /**
         * Opening moon panel
         */
        private void ShowMoonPanel(object sender, SwitchViewRequestedEvent e)
        {
            this._destroyMainMapPanel();
            this._setUpMoonMapPanel(e.ObjectEntries, e.Date, e.MapMode, e.CenterName);
        }
        /**
         * Opening main panel from moon panel
         */
        private void ShowMainPanel(object sender, SwitchViewRequestedEvent e)
        {
            this._destroyMoonPanel();   
            this._setUpMainMapPanel(this.ObjectEntries, e.Date);
        }




    }
}
