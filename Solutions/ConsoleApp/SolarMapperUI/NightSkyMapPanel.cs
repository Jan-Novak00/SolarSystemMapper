using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SolarMapperUI
{
    internal class NightSkyMapPanel : MapPanel
    {
        private List<EphemerisObserverData> _originalData;

        private List<FormBody<EphemerisObserverData>> _data;

        private List<FormBody<EphemerisObserverData>> _prepareBodyData(List<EphemerisObserverData> data)
        {
            List<FormBody<EphemerisObserverData>> result = new List<FormBody<EphemerisObserverData>>();
            Point center = new Point(this.DisplayRectangle.Width / 2, this.Height / 2);
            Debug.Write($"Center: {center}");
            foreach (var info in data)
            {
                var formBody = info.ToFormBody(center, (int)Math.Min((this.Width * 0.8), (this.Height * 0.8)) / 2);
                result.Add(formBody);
            }
            return result;


        }

        private void SetData()
        {
            _data = _prepareBodyData(_originalData);
        }


        public NightSkyMapPanel(List<EphemerisObserverData> Data)
        {
            this._originalData = Data;
            this._data = null;
            this.BackColor = Color.DarkBlue;
            this.Paint += PrintObjects;
            
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetData();
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


        protected override void PrintObjects(object sender, PaintEventArgs e)
        {
            foreach (var formBody in _data)
            {
                var showName = formBody.BodyData.objectData.Type == "Planet" || formBody.BodyData.objectData.Type == "Star";
                if (formBody.PixelInfos[0].Visible) drawFormBody(formBody.PixelInfos, e, (showName) ? formBody.BodyData.objectData.Name : null);
                Debug.WriteLine(formBody.PixelInfos[0].BodyCoordinates);
                Debug.WriteLine((int)Math.Min((this.Width * 0.8), (this.Height * 0.8)));
            }
        }
        protected override void BodyClick(object sender, MouseEventArgs e)
        {
            foreach (var formBody in _data)
            {
                var centerX = formBody.PixelInfos[0].BodyCoordinates.X + formBody.PixelInfos[0].Diameter / 2;
                var centerY = formBody.PixelInfos[0].BodyCoordinates.Y + formBody.PixelInfos[0].Diameter / 2;
                var distance = Math.Sqrt((centerX - e.X) * (centerX - e.X) + (centerY - e.Y) * (centerY - e.Y));
                if (distance < formBody.PixelInfos[0].Diameter / 2) ShowBodyReport(formBody.BodyReport(DateTime.Today));
            }
        }
    }
}
