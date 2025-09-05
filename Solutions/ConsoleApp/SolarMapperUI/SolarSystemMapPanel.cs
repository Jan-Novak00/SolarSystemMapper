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

    /**
     * Used for scale change
     */

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

        protected double _minSpeed = double.NegativeInfinity;
        protected double _maxSpeed = double.PositiveInfinity;
        protected double _minDistance = double.NegativeInfinity;
        protected double _maxDistance = double.PositiveInfinity;

        protected override bool _otherVisibilityConditions(IFormBody<EphemerisVectorData> body)
        {
            double speed = Math.Sqrt(Math.Pow(body.BodyData.ephemerisTable[this._pictureIndex].VX ?? 0, 2) + Math.Pow(body.BodyData.ephemerisTable[this._pictureIndex].VY ?? 0, 2) + Math.Pow(body.BodyData.ephemerisTable[this._pictureIndex].VZ ?? 0, 2));
            double distance = Math.Sqrt(Math.Pow(body.BodyData.ephemerisTable[this._pictureIndex].X ?? 0, 2) + Math.Pow(body.BodyData.ephemerisTable[this._pictureIndex].Y ?? 0, 2) + Math.Pow(body.BodyData.ephemerisTable[this._pictureIndex].Z ?? 0, 2));
            return speed <= _maxSpeed && speed >= _minSpeed && distance <= _maxDistance && distance >= _minDistance;
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
            this._minSpeed = generalMapSettings.minSpeed;
            this._maxSpeed = generalMapSettings.maxSpeed;
            this._minDistance = generalMapSettings.minDistance;
            this._maxDistance = generalMapSettings.maxDistance;
            this.Scale_km = scale_km;
            this._pictureIndex = 0;
            this._originalData = null;
            this._data = null;
            this.BackColor = Color.Black;
            this.Paint += PrintObjects;
            this.ScaleChange += OnChangeScale;

        }




        /**
         * Changing scale
         */
        protected void OnChangeScale(object sender, ChangeScaleEvent e)
        {
            if (this._data == null) return;
            this.Scale_km = e.Scale_Km;
            Parallel.ForEach(_data, formBody =>
            {
                formBody.ChangeScale(this.Scale_km, this.ClientRectangle.Height, this.ClientRectangle.Width, this._respectScaleForBodySize);
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
