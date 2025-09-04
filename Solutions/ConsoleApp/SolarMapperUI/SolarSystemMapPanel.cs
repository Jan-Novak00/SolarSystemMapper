using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static SolarSystemMapper.NASAHorizonsDataFetcher;

namespace SolarMapperUI
{

    internal class ChangeScaleEvent : EventArgs
    {
        public ChangeScaleEvent(float scale_Km)
        {
            Scale_Km = scale_Km;
        }

        public float Scale_Km { get; init; }
    }



    internal class SolarSystemMapPanel : MapPanel<EphemerisVectorData>
    {
        public float Scale_km { get; private set; }

        protected override NASAHorizonsDataFetcher.MapMode _mode { get; init; } = MapMode.SolarSystem;

        protected virtual bool _respectScaleForBodySize { get; } = false;

        public event EventHandler<ChangeScaleEvent> ScaleChange;

        public virtual string CenterName { get; protected init; } = "Sun";

        protected override List<IFormBody<EphemerisVectorData>> _prepareBodyData(List<EphemerisVectorData> data)
        {
            List<IFormBody<EphemerisVectorData>> result = new List<IFormBody<EphemerisVectorData>>();
            Point center = new Point(this.Width / 2, this.Height / 2);
            object lockObj = new object();
            Parallel.ForEach(data, info =>
            {
                var formBody = info.ToFormBody(center, this.Scale_km, this.Height, this.Width, this._respectScaleForBodySize);
                lock (lockObj)
                {
                    result.Add(formBody);
                }
            });
            return result;


        }

       
        public SolarSystemMapPanel(List<ObjectEntry> objects, DateTime mapStartDate, float scale_km = 1_000_000)
        {
            this.Scale_km = scale_km;
            this.Scale_km = scale_km;
            this.ObjectEntries = objects;
            this._pictureIndex = 0;
            this.CurrentPictureDate = mapStartDate;
            this._originalData = null;
            this._data = null;
            this.BackColor = Color.Black;
            this.Paint += PrintObjects;
            this.ScaleChange += OnChangeScale;

        }

        public SolarSystemMapPanel(GeneralMapSettings generalMapSettings, IEnumerable<Func<IEnumerable<IFormBody<EphemerisVectorData>>, IEnumerable<IFormBody<EphemerisVectorData>>>> typeFilters,
            float scale_km = 1_000_000) : base(generalMapSettings, typeFilters)
        {
            this.Scale_km = scale_km;
            this._pictureIndex = 0;
            this._originalData = null;
            this._data = null;
            this.BackColor = Color.Black;
            this.Paint += PrintObjects;
            this.ScaleChange += OnChangeScale;

        }



        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            
            _ = SettingDataAsync();
        }

        protected void OnChangeScale(object sender, ChangeScaleEvent e)
        {
            if (this._data == null) return;
            this.Scale_km = e.Scale_Km;
            Parallel.ForEach(_data, formBody =>
            {
                formBody.ChangeScale(this.Scale_km, this.ClientRectangle.Height, this.ClientRectangle.Height, this._respectScaleForBodySize);
            });
            this.Invalidate();

        }

        public void InvokeScaleSwitchEvent(float scale_km)
        {
            ScaleChange?.Invoke(this, new ChangeScaleEvent(scale_km));
        }

        protected override bool _visibleNameInThisLocation(Point center, Point location, string Name)
        {
            if (Name == this.CenterName) return true;
            else return (Math.Sqrt(Math.Pow(center.X - location.X, 2) + Math.Pow(center.Y - location.Y, 2)) > 10);
        }


    }
}
