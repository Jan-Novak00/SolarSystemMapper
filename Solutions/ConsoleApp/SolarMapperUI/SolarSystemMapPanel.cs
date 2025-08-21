using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarMapperUI
{
    internal class SolarSystemMapPanel : MapPanel<EphemerisVectorData>
    {
        private List<EphemerisVectorData> testData;
        private float scale_km = 1_000_000;


        protected override List<FormBody<EphemerisVectorData>> _prepareBodyData(List<EphemerisVectorData> data)
        {
            List<FormBody<EphemerisVectorData>> result = new List<FormBody<EphemerisVectorData>>();
            Point center = new Point(this.Width / 2, this.Height / 2);
            Debug.Write($"Center: {center}");
            foreach (var info in data)
            {
                var formBody = info.ToFormBody(center, this.scale_km, this.Height, this.Width);
                result.Add(formBody);
            }
            return result;


        }

        private void SetData()
        {
            _data = _prepareBodyData(_originalData);
        }

        protected override async Task<IReadOnlyList<EphemerisVectorData>> GetHorizonsData(List<ObjectEntry> objects)
        {
            var fetcher = new NASAHorizonsDataFetcher(NASAHorizonsDataFetcher.MapMode.SolarSystem, objects, this._currentPictureDate, this._currentPictureDate.AddDays(30));
            var result = await fetcher.Fetch();
            return result.Cast<EphemerisVectorData>().ToList().AsReadOnly();
        }



        public SolarSystemMapPanel(List<ObjectEntry> objects, DateTime mapStartDate, float scale_km = 1_000_000)
        {
            this.scale_km = scale_km;
            this.objects = objects;
            this._pictureIndex = 0;
            this._currentPictureDate = mapStartDate;
            this._originalData = null;
            this._data = null;
            this.BackColor = Color.Black;
            this.Paint += PrintObjects;

        }
        private async Task InitializeDataAsync()
        {
            _originalData = (await GetHorizonsData(objects)).ToList();
            SetData();

            this.Invalidate();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            //this.BeginInvoke(new Action(() => SetData()));
            _ = InitializeDataAsync();
        }



    }
}
