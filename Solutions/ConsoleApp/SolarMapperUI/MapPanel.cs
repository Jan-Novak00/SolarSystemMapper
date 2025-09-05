using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SolarMapperUI
{

    internal class SwitchViewRequestedEvent : EventArgs
    {
        public List<ObjectEntry> ObjectEntries;
        public DateTime Date;
        public NASAHorizonsDataFetcher.MapMode MapMode;
        public string CenterName;

        public SwitchViewRequestedEvent(List<ObjectEntry> objectEntries, DateTime date, NASAHorizonsDataFetcher.MapMode mapMode, string centerName)
        {
            ObjectEntries = objectEntries;
            Date = date;
            MapMode = mapMode;
            CenterName = centerName;
        }
    }


    internal interface IMap : IDisposable
    {
        public void AdvanceMap();
        public DateTime CurrentPictureDate { get; }

        public event EventHandler<SwitchViewRequestedEvent> MapSwitch;
        public List<ObjectEntry> ObjectEntries { get; }
        public void CleanAndDispose();


    }

    internal abstract class MapPanel<TData> : Panel, IMap
        where TData : IEphemerisData<IEphemerisTableRow>
    {
        protected List<TData> _originalData;

        protected List<IFormBody<TData>> _data;

        public List<ObjectEntry> ObjectEntries { get; protected set; }

        public event EventHandler<SwitchViewRequestedEvent> MapSwitch;

        protected virtual NASAHorizonsDataFetcher.MapMode _mode { get; init; }

        private HashSet<string> _objectsWithVisibleName = new HashSet<string>();

        protected int _pictureIndex { get; set; } = 0;
        public DateTime CurrentPictureDate { get; protected set; }


        private Label _loadingLabel = new Label
        {
            Text = "Loading...",
            ForeColor = Color.LimeGreen,
            AutoSize = true,
            Visible = false,
        };

        protected abstract List<IFormBody<TData>> _prepareBodyData(List<TData> data);

        private bool _initialFetchDone = false;

        protected virtual void _updateNameVisibilityBaseOnUserInteraction()
        {
            if (this._data == null) return;
            foreach (var body in this._data)
            {
                body.SetNameVisibility(_objectsWithVisibleName.Contains(body.BodyData.objectData.Name));
            }
        }

        protected readonly int _numberOfDaysToPrefetch = 30;
        protected virtual async Task<IReadOnlyList<TData>> GetHorizonsData(List<ObjectEntry> objects)
        {
            var fetcher = new NASAHorizonsDataFetcher(_mode, objects, this.CurrentPictureDate, this.CurrentPictureDate.AddDays(this._numberOfDaysToPrefetch));
            var result = await fetcher.Fetch();
            return result.Cast<TData>().Where(x => x.ephemerisTable.Count == this._numberOfDaysToPrefetch + 1).ToList().AsReadOnly();
        }
        private void SetData()
        {
            _data = _prepareBodyData(_originalData);
            if (_initialFetchDone) _updateNameVisibilityBaseOnUserInteraction();
            else
            {
                _registerVisibleObjects();
                if (_doFilter) _filter();
            }
        }

        protected async Task SettingDataAsync()
        {
            _loadingLabel.Location = new Point(this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2);
            _loadingLabel.Visible = true;
            _originalData = (await GetHorizonsData(ObjectEntries)).ToList();
            SetData();
            _initialFetchDone = true;
            _loadingLabel.Visible = false;

            this.Invalidate();
        }

        protected IEnumerable<Func<IEnumerable<IFormBody<TData>>, IEnumerable<IFormBody<TData>>>> _typeFilters;

        protected void _filter()
        {
            
            if (_data == null) return;
            var generallyFilteredData = _data.Where(x => _generalFilter(x.BodyData.objectData)); //uses general filter

            IEnumerable<IFormBody<TData>> typeFilteredData = new List<IFormBody<TData>>();

            foreach (var filter in _typeFilters)
            {
                var filteredForThisType = filter(generallyFilteredData);
                typeFilteredData = typeFilteredData.Union(filteredForThisType);
            }

            _data = typeFilteredData.Where(x => !this._blackList.Any(name => name == x.BodyData.objectData.Name))
                .Union(_data.Where(x=> this._whiteList.Any(name => name == x.BodyData.objectData.Name)))
                .ToList();

            Debug.WriteLine($"_data.Count = {_data.Count}, ObjectEntries.count = {ObjectEntries.Count}");
            ObjectEntries = ObjectEntries.Where(x=> _data.Any(formBody => formBody.BodyData.objectData.Name == x.Name)).ToList();

        }
        protected bool _doFilter;
        protected Predicate<ObjectData> _generalFilter { get; set; }
        protected List<string> _whiteList {  get; set; }
        protected List<string> _blackList { get; set; }
        public MapPanel(GeneralMapSettings generalMapSettings, IEnumerable<Func<IEnumerable<IFormBody<TData>>, IEnumerable<IFormBody<TData>>>> typeFilters)
        {
            _typeFilters = typeFilters;
            _generalFilter = generalMapSettings.GeneralFilter;
            _whiteList = generalMapSettings.WhiteList;
            _blackList = generalMapSettings.BlackList;
            CurrentPictureDate = generalMapSettings.StartDate;
            this._doFilter = true;

            IEnumerable<ObjectEntry> objectsEnumerable = new List<ObjectEntry>();
            foreach (var typeName in generalMapSettings.ObjectTypes)
            {
                objectsEnumerable = objectsEnumerable.Union(DataTables.GiveEntries(typeName));
            }
            ObjectEntries = objectsEnumerable.ToList();

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.Black;
            this.Controls.Add(_loadingLabel);

            this.InitializeHandlers();


        }

        protected MapPanel()
        {
            this._doFilter = false;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.Black;
            this.Controls.Add(_loadingLabel);


            this.InitializeHandlers();

            
        }

        protected virtual void InitializeHandlers()
        {

            this.MouseClick += BodyClick;
            this.Paint += PrintObjects;
            this.MouseMove += _mouseMoveAcrossBody;
            this.MouseLeave += _mouseMoveOutOfBody;
        }

        protected void _registerVisibleObjects()
        {
            IEnumerable<string> visibleObjects = this._data.Where(formBody => formBody.PixelInfos[this._pictureIndex].ShowName).Select(x => x.BodyData.objectData.Name);
            _objectsWithVisibleName = visibleObjects.ToHashSet();
        }

        public virtual async void AdvanceMap()
        {
            if (this._data == null || this._data.Count == 0) return;

            if (this._pictureIndex + 1 == _data[0].BodyData.ephemerisTable.Count)
            {
                _registerVisibleObjects();
                this._data = null;
                this._pictureIndex = 0;
                this.CurrentPictureDate = this.CurrentPictureDate.AddDays(1);
                await SettingDataAsync();
            }
            else
            {
                _pictureIndex++;
                CurrentPictureDate = _data[0].BodyData.ephemerisTable[_pictureIndex].date.Value;
            }
            this.Invalidate();

        }


        protected virtual bool _otherVisibilityConditions(IFormBody<TData> body) => true;

        protected virtual void PrintObjects(object sender, PaintEventArgs e)
        {
            if (_data == null) return;
            foreach (var formBody in this._data)
            {   
                if (formBody.PixelInfos.Count == 0) continue;
                if (formBody.PixelInfos[this._pictureIndex].Visible && _otherVisibilityConditions(formBody)) 
                    drawFormBody(formBody.PixelInfos[this._pictureIndex], 
                    e, 
                    (formBody.PixelInfos[this._pictureIndex].ShowName && 
                     _visibleNameInThisLocation(
                         formBody.PixelInfos[this._pictureIndex].CenterCoordinates, 
                         formBody.PixelInfos[this._pictureIndex].BodyCoordinates, 
                         formBody.BodyData.objectData.Name)) 
                                                            ? formBody.BodyData.objectData.Name 
                                                            : null);
            }
        }
        protected virtual void BodyClick(object sender, MouseEventArgs e)
        {
            if (_data == null) return;
            foreach (var formBody in _data)
            {
                if (!_otherVisibilityConditions(formBody) || !formBody.PixelInfos[_pictureIndex].Visible) continue;
                var centerX = formBody.PixelInfos[this._pictureIndex].BodyCoordinates.X + formBody.PixelInfos[this._pictureIndex].Diameter / 2;
                var centerY = formBody.PixelInfos[this._pictureIndex].BodyCoordinates.Y + formBody.PixelInfos[this._pictureIndex].Diameter / 2;
                var distance = Math.Sqrt((centerX - e.X) * (centerX - e.X) + (centerY - e.Y) * (centerY - e.Y));
                if (distance < formBody.PixelInfos[this._pictureIndex].Diameter / 2 + 5 + ((formBody.PixelInfos[this._pictureIndex].Diameter < 5) ? 10 : 0)) ShowBodyReport(formBody);
            }
        }

        protected virtual void drawFormBody(PixelBodyInfo pixelInfo, PaintEventArgs e, string? name)
        {
            var brush = new SolidBrush(pixelInfo.Color);
            var leftCornerX = pixelInfo.BodyCoordinates.X;
            var leftCornerY = pixelInfo.BodyCoordinates.Y;
            var diameter = pixelInfo.Diameter;
            if (diameter < 4) diameter = 4;
                e.Graphics.FillEllipse(brush, leftCornerX, leftCornerY, diameter, diameter);
            var textSize = e.Graphics.MeasureString(name, DefaultFont);
            float textX = leftCornerX - textSize.Width / 2 + diameter / 2;
            float textY = leftCornerY + diameter / 2 + 10;
            e.Graphics.DrawString(name, DefaultFont, Brushes.White, textX, textY);
        }

        protected void ShowBodyReport(IFormBody<TData> formBody)
        {

            string report = formBody.BodyReport(this.CurrentPictureDate);
            string bodyName = formBody.BodyData.objectData.Name;

            var reportForm = new Form();
            reportForm.Text = "Body Information";
            reportForm.StartPosition = FormStartPosition.Manual;
            reportForm.BackColor = Color.Black;
            reportForm.ForeColor = Color.White;
            reportForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            reportForm.MaximizeBox = false;
            reportForm.MinimizeBox = false;
            reportForm.ShowInTaskbar = false;

            // vytvoření labelu s textem
            var label = new Label();
            label.Text = report;
            label.ForeColor = Color.White;
            label.AutoSize = true;
            label.MaximumSize = new Size(700, 0); // maximální šířka
            label.Location = new Point(10, 10);

            reportForm.Controls.Add(label);


            int width = label.Right;
            int height = label.Bottom;


            System.Windows.Forms.Button? viewMoonsButton = null;
            if (DataTables.ObjectsWithSatelites.Contains(bodyName))
            {
                viewMoonsButton = new System.Windows.Forms.Button();
                viewMoonsButton.Text = "Moon view";
                viewMoonsButton.AutoSize = true;
                viewMoonsButton.Location = new Point(10, label.Bottom + 10); // pod labelem


                viewMoonsButton.Click += (s, e) => this.ShowMoonsButtonClick(bodyName); 
                reportForm.Controls.Add(viewMoonsButton);
            }

            System.Windows.Forms.Button switchNameVisibilityButton = new System.Windows.Forms.Button();
            switchNameVisibilityButton.Text = (!formBody.PixelInfos[this._pictureIndex].ShowName) ? "Track" : "Untrack";
            switchNameVisibilityButton.AutoSize = true;
            switchNameVisibilityButton.Location = (viewMoonsButton != null) ? new Point(viewMoonsButton.Width + 15, label.Bottom + 10) : new Point(10, label.Bottom + 10);
            switchNameVisibilityButton.Click += (s, e) =>
            {
                if (switchNameVisibilityButton.Text == "Track")
                {
                    switchNameVisibilityButton.Text = "Untrack";
                    this._objectsWithVisibleName.Remove(bodyName);
                }
                else
                {
                    switchNameVisibilityButton.Text = "Track";
                    this._objectsWithVisibleName.Add(bodyName);
                }

                formBody.SwitchNameVisibility();
                this.Invalidate();
            };
            reportForm.Controls.Add(switchNameVisibilityButton);

            if (viewMoonsButton != null)
            { 
                width = Math.Max(width, viewMoonsButton.Right);
                height = Math.Max(height, viewMoonsButton.Bottom);
            }
            width = Math.Max(width, switchNameVisibilityButton.Right) + 10;
            height = Math.Max(width, switchNameVisibilityButton.Bottom) + 10;


            reportForm.ClientSize = new Size(width, height);

           
            reportForm.Location = this.PointToScreen(new Point(
                this.ClientRectangle.Right - reportForm.Width - 20,
                this.ClientRectangle.Bottom - reportForm.Height - 20
            ));

            reportForm.Show(this); // zobrazí okno nad hlavním formulářem
        }

        private void ShowMoonsButtonClick(string planetName)
        {
            var objectEntries = DataTables.GiveSatelitesToPlanet(planetName);
            var currentObject = this.ObjectEntries.Where(x => x.Name == planetName);
            objectEntries.UnionWith(currentObject);
            this.InvokeMapSwitch(objectEntries.ToList(),this.CurrentPictureDate, NASAHorizonsDataFetcher.ObjectToMapMode(planetName), planetName);
        }

        public virtual void CleanAndDispose()
        {
            this.MapSwitch = null;
            this.Dispose();
        }

        protected virtual void InvokeMapSwitch(List<ObjectEntry> objectEntries, DateTime date, NASAHorizonsDataFetcher.MapMode mode, string centerName)
        {
            MapSwitch?.Invoke(this, new SwitchViewRequestedEvent(objectEntries, date, mode, centerName));
        }

        protected virtual bool _visibleNameInThisLocation(Point center, Point location, string Name) => true;


        private System.Windows.Forms.ToolTip _toolTip = new System.Windows.Forms.ToolTip();
        private IFormBody<TData>? _currentHoveredBody = null;

        private void _mouseMoveAcrossBody(object sender, MouseEventArgs e)
        {
            if (_data == null) return;

            IFormBody<TData>? hovered = null;
            foreach (var formBody in _data)
            {
                if (!_otherVisibilityConditions(formBody) || !formBody.PixelInfos[_pictureIndex].Visible) continue;
                var pixel = formBody.PixelInfos[this._pictureIndex];
                var centerX = pixel.BodyCoordinates.X + pixel.Diameter / 2;
                var centerY = pixel.BodyCoordinates.Y + pixel.Diameter / 2;
                var distance = Math.Sqrt((centerX - e.X) * (centerX - e.X) + (centerY - e.Y) * (centerY - e.Y));
                if (distance < pixel.Diameter / 2 + 5 + ((pixel.Diameter < 5) ? 10 : 0))
                {
                    hovered = formBody;
                    break;
                }
            }
            

            if (hovered != _currentHoveredBody)
            {
                _currentHoveredBody = hovered;
                if (hovered != null)
                {
                    _toolTip.Show(hovered.BodyData.objectData.Name, this, e.Location.X + 10, e.Location.Y + 10, 2000);
                }
                else
                {
                    _toolTip.Hide(this);
                }
            }
            this.Cursor = (hovered != null) ? Cursors.Hand : Cursors.Default;
        }
        private void _mouseMoveOutOfBody(object sender, EventArgs e)
        {
            _currentHoveredBody = null;
            _toolTip.Hide(this);
        }

    }
}
