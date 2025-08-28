using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SolarSystemMapper.NASAHorizonsDataFetcher;

namespace SolarMapperUI
{
    internal class SolarSystemMapPanel : MapPanel<EphemerisVectorData>
    {
        protected float scale_km { get; set; }

        protected override NASAHorizonsDataFetcher.MapMode _mode { get; init; } = MapMode.SolarSystem;

        protected virtual bool _respectScaleForBodySize { get; } = false;

        protected override List<FormBody<EphemerisVectorData>> _prepareBodyData(List<EphemerisVectorData> data)
        {
            List<FormBody<EphemerisVectorData>> result = new List<FormBody<EphemerisVectorData>>();
            Point center = new Point(this.Width / 2, this.Height / 2);
            object lockObj = new object();
            Parallel.ForEach(data, info =>
            {
                var formBody = info.ToFormBody(center, this.scale_km, this.Height, this.Width, this._respectScaleForBodySize);
                lock (lockObj)
                {
                    result.Add(formBody);
                }
            });
            return result;


        }

       
        public SolarSystemMapPanel(List<ObjectEntry> objects, DateTime mapStartDate, float scale_km = 1_000_000)
        {
            this.scale_km = scale_km;
            this.objects = objects;
            this._pictureIndex = 0;
            this.CurrentPictureDate = mapStartDate;
            this._originalData = null;
            this._data = null;
            this.BackColor = Color.Black;
            this.Paint += PrintObjects;

        }
        

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            
            _ = SettingDataAsync();
        }



    }
}
