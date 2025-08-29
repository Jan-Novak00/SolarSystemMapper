using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarMapperUI
{
    internal class SateliteMap : SolarSystemMapPanel
    {
        protected override bool _respectScaleForBodySize { get; } = true;
        public SateliteMap(List<ObjectEntry> objects, DateTime mapStartDate, NASAHorizonsDataFetcher.MapMode Mode, float scale_km = 1000) : base(objects, mapStartDate, scale_km)
        {
            this._mode = Mode;
        }



        private void _drawArrowAtTheEdge(Graphics graphics, PointF location, Color color, string name)
        {
            float length = (float)Math.Sqrt(Math.Pow(location.X, 2) + Math.Pow(location.Y, 2));
            PointF direction = new PointF(location.X/length, location.Y/length);
            PointF center = new PointF(this.ClientRectangle.Width/2f,this.ClientRectangle.Height/2f);
            float t = float.MaxValue;
            if (direction.X != 0)
            {
                float tx1 = (0 - center.X) / direction.X; // levá hranice
                float tx2 = (this.ClientRectangle.Width - center.X) / direction.X; // pravá hranice
                if (tx1 > 0) t = Math.Min(t, tx1);
                if (tx2 > 0) t = Math.Min(t, tx2);
            }

            if (direction.Y != 0)
            {
                float ty1 = (0 - center.Y) / direction.Y; // horní hranice
                float ty2 = (this.ClientRectangle.Height - center.Y) / direction.Y; // dolní hranice
                if (ty1 > 0) t = Math.Min(t, ty1);
                if (ty2 > 0) t = Math.Min(t, ty2);
            }

            PointF end = new PointF(center.X + direction.X * t, center.Y + direction.Y * t);
            PointF perpendicular = new PointF(-direction.Y, direction.X);

            float arrowSize = 15;

            PointF baseLeft = new PointF(end.X - direction.X * arrowSize + perpendicular.X * arrowSize / 2,
                                         end.Y - direction.Y * arrowSize + perpendicular.Y * arrowSize / 2);
            PointF baseRight = new PointF(end.X - direction.X * arrowSize - perpendicular.X * arrowSize / 2,
                                          end.Y - direction.Y * arrowSize - perpendicular.Y * arrowSize / 2);
            graphics.FillPolygon(new SolidBrush(color), new PointF[]{ end, baseLeft, baseRight});

        }

        private void _printDirections(PaintEventArgs e)
        {
            var objectsForArrows = this._data.Where(x => x.BodyData.objectData.Name == "Earth" || x.BodyData.objectData.Name == "Sun");

            foreach (var body in objectsForArrows)
            {
                this._drawArrowAtTheEdge(e.Graphics, 
                                        body.PixelInfos[this._pictureIndex].BodyCoordinates, 
                                        (body.BodyData.objectData.Name == "Earth") ? Color.Aquamarine : Color.Yellow, 
                                        body.BodyData.objectData.Name);
            }


        }

        public void ReturnBack()
        {
            this.InvokeMapSwitch(null,this.CurrentPictureDate,0);
        }



    }
}
