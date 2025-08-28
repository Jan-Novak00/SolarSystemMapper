using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static SolarSystemMapper.NASAHorizonsDataFetcher;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SolarMapperUI
{
    internal class NightSkyMapPanel : MapPanel<EphemerisObserverData>
    {

        protected override  NASAHorizonsDataFetcher.MapMode _mode { get; init; } = MapMode.NightSky;


        protected override List<FormBody<EphemerisObserverData>> _prepareBodyData(List<EphemerisObserverData> data)
        {
            List<FormBody<EphemerisObserverData>> result = new List<FormBody<EphemerisObserverData>>();
            Point center = new Point(this.DisplayRectangle.Width / 2, this.Height / 2);
            object lockObj = new object();
            Parallel.ForEach(data, info =>
            {
                var formBody = info.ToFormBody(center, (int)Math.Min((this.Width * 0.8), (this.Height * 0.8)) / 2);
                lock (lockObj)
                {
                    result.Add(formBody);
                }
            });

            return result;


        }

        public NightSkyMapPanel(List<ObjectEntry> objects, DateTime mapStartDate)
        {
            this.objects = objects;
            this._pictureIndex = 0;
            this.CurrentPictureDate = mapStartDate;
            this._originalData = null;
            this._data = null;
            this.BackColor = Color.DarkBlue;
            this.Paint += PrintObjects;
            
        }

        

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            //this.BeginInvoke(new Action(() => SetData()));
            _ = SettingDataAsync();
        }

        protected override void InitializeHandlers()
        {
            this.Paint += this.PrintMapBackground;
            base.InitializeHandlers();
        }

        private void PrintMapBackground(object sender, PaintEventArgs e)
        {
            int diameter = (int)Math.Min((this.Width * 0.8), (this.Height * 0.8));
            PointF center = new Point(this.Width / 2, this.Height / 2);
            int radius = diameter / 2;

            // vykreslení pozadí kruhu
            e.Graphics.FillEllipse(Brushes.Black, center.X - radius, center.Y - radius, diameter, diameter);
            e.Graphics.DrawEllipse(Pens.White, center.X - radius, center.Y - radius, diameter, diameter);

            Font font = new Font("Segoe UI", 12, FontStyle.Bold);
            Brush brush = Brushes.LimeGreen;

            e.Graphics.DrawString("N", font, brush, center.X - 10, center.Y - radius - 45);
            e.Graphics.DrawString("S", font, brush, center.X - 10, center.Y + radius);
            e.Graphics.DrawString("E", font, brush, center.X + radius + 5, center.Y);
            e.Graphics.DrawString("W", font, brush, center.X - radius - 45, center.Y );
                //Thread.Sleep(10000);

                // Úhly po 30°
            for (int angle = 0; angle < 360; angle += 30)
            {
                if (angle % 90 == 0) continue;
                double radians = Math.PI - ((angle + 90) * Math.PI / 180.0);
                int x = (int)(center.X + (int)((radius + 15) * Math.Cos(radians))) - ((angle < 180) ? 45 : 0);
                int y = (int)(center.Y - (int)((radius + 15) * Math.Sin(radians)));
                string text = angle.ToString();
                SizeF textSize = e.Graphics.MeasureString(text, font);
                e.Graphics.DrawString(text, font, brush, x - textSize.Width / 2, y - textSize.Height / 2);
            }
        }


       
    }
}
