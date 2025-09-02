using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void CleanAndDispose();


    }

    internal abstract class MapPanel<TData> : Panel, IMap
        where TData : IEphemerisData<IEphemerisTableRow>
    {
        protected List<TData> _originalData;

        protected List<FormBody<TData>> _data;

        protected List<ObjectEntry> _objectEntries;

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

        protected abstract List<FormBody<TData>> _prepareBodyData(List<TData> data);

        private bool _initialFetchDone = false;

        protected virtual void _updateNameVisibilityBaseOnUserInteraction()
        {
            if (this._data == null) return;
            foreach (var body in this._data) 
            {
               body.SetNameVisibility(_objectsWithVisibleName.Contains(body.BodyData.objectData.Name));
            }
        }


        protected virtual void _updateNameVisibilityBasedOnLocation() 
        {
            foreach (var body in this._data)
            {
                foreach (var pixelInfo in body.PixelInfos)
                {
                    if (body.BodyData.objectData.Type != "Star") 
                    { 
                        if (Math.Sqrt(Math.Pow(pixelInfo.CenterCoordinates.X - pixelInfo.BodyCoordinates.X, 2) + Math.Pow(pixelInfo.CenterCoordinates.Y - pixelInfo.BodyCoordinates.Y, 2)) < 10) pixelInfo.ShowName = false; 
                        else if (Math.Sqrt(Math.Pow(pixelInfo.CenterCoordinates.X - pixelInfo.BodyCoordinates.X, 2) + Math.Pow(pixelInfo.CenterCoordinates.Y - pixelInfo.BodyCoordinates.Y, 2)) >= 10 && this._objectsWithVisibleName.Contains(body.BodyData.objectData.Name)) pixelInfo.ShowName = true;
                    }

                }
            }
        }

        protected readonly int _numberOfDaysToPrefetch = 30;
        protected virtual async Task<IReadOnlyList<TData>> GetHorizonsData(List<ObjectEntry> objects)
        {
            var fetcher = new NASAHorizonsDataFetcher(_mode, objects, this.CurrentPictureDate, this.CurrentPictureDate.AddDays(this._numberOfDaysToPrefetch));
            var result = await fetcher.Fetch();
            return result.Cast<TData>().Where(x => x.ephemerisTable.Count == this._numberOfDaysToPrefetch+1).ToList().AsReadOnly();
        }
        private void SetData()
        {
            _data = _prepareBodyData(_originalData);
            if (_initialFetchDone) _updateNameVisibilityBaseOnUserInteraction();
            else
            {
                _registerVisibleObjects();
                _filter();
            }
        }

        protected async Task SettingDataAsync()
        {
            _loadingLabel.Location = new Point(this.ClientRectangle.Width/2, this.ClientRectangle.Height /2);
            _loadingLabel.Visible = true;
            _originalData = (await GetHorizonsData(_objectEntries)).ToList();
            SetData();
            _initialFetchDone = true;
            _loadingLabel.Visible = false;

            this.Invalidate();
        }
        //GeneralMapSettings(SolarMapperMainForm.MapType MapType,
        //DateTime StartDate,
        //List<string> ObjectTypes, List<string> WhiteList,
        //List<string> BlackList, Predicate<FormBody<IEphemerisData<IEphemerisTableRow>>> GeneralFilter,
        //double? latitude = null,
        //double? longitude = null);
        

        protected void _filter()
        {
            if (_data == null) return;
            var filteredData = _data.Where(x => _generalFilter(x.BodyData.objectData)); //uses general filter
            //typove filtrovani
            //....
            _data = filteredData.Union(from x in _data
                               where this._whiteList.Any(name => name == x.BodyData.objectData.Name) // _objectEntries which are on whitelist
                               && !this._blackList.Any(name => name == x.BodyData.objectData.Name) // _objectEntries which are not on blackList
                               select x).ToList();
            _objectEntries = (from entry in _objectEntries                                                        // no filtering required in future fetches
                      where _data.Any(x => x.BodyData.objectData.Name == entry.Name)
                      || _data.Any(x => x.BodyData.objectData.Code == entry.Code)
                      select entry).ToList();

        }

        protected Predicate<ObjectData> _generalFilter { get; set; }
        protected List<string> _whiteList {  get; set; }
        protected List<string> _blackList { get; set; }
        public MapPanel(GeneralMapSettings generalMapSettings)
        {
            
            _generalFilter = generalMapSettings.GeneralFilter;
            _whiteList = generalMapSettings.WhiteList;
            _blackList = generalMapSettings.BlackList;

            IEnumerable<ObjectEntry> objectsEnumerable = new List<ObjectEntry>();
            foreach (var typeName in generalMapSettings.ObjectTypes)
            {
                objectsEnumerable = objectsEnumerable.Union(DataTables.GiveEntries(typeName));
            }
            _objectEntries = objectsEnumerable.ToList();

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.Black;
            this.Controls.Add(_loadingLabel);

            this.InitializeHandlers();


        }

        protected MapPanel()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.Black;
            this.Controls.Add(_loadingLabel);


            this.InitializeHandlers();

            
        }

        protected virtual void InitializeHandlers()
        {

            this.MouseClick += BodyClick;
            this.Paint += PrintObjects;
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
                foreach (var obj in _objectsWithVisibleName) Debug.WriteLine(obj);
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


        protected virtual void PrintObjects(object sender, PaintEventArgs e)
        {
            if (_data == null) return;
            foreach (var formBody in this._data)
            {   
                if (formBody.PixelInfos.Count == 0) continue;
                if (formBody.PixelInfos[this._pictureIndex].Visible) 
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
                
                var centerX = formBody.PixelInfos[this._pictureIndex].BodyCoordinates.X + formBody.PixelInfos[this._pictureIndex].Diameter / 2;
                var centerY = formBody.PixelInfos[this._pictureIndex].BodyCoordinates.Y + formBody.PixelInfos[this._pictureIndex].Diameter / 2;
                var distance = Math.Sqrt((centerX - e.X) * (centerX - e.X) + (centerY - e.Y) * (centerY - e.Y));
                if (distance < formBody.PixelInfos[this._pictureIndex].Diameter / 2 + 5) ShowBodyReport(formBody);
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

        protected void ShowBodyReport(FormBody<TData> formBody)
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
            var currentObject = this._objectEntries.Where(x => x.Name == planetName);
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




    }
}
