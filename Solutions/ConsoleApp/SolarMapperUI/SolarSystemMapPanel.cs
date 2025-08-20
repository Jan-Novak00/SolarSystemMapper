using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarMapperUI
{
    internal class SolarSystemMapPanel : MapPanel<EphemerisVectorData>
    {
        private List<EphemerisVectorData> testData;


        protected override List<FormBody<EphemerisVectorData>> _prepareBodyData(List<EphemerisVectorData> data)
        {
            throw new NotImplementedException();
        }

        public SolarSystemMapPanel()
        {
            testData = new List<EphemerisVectorData>()
            {
                new EphemerisVectorData(new List<EphemerisTableRowVector>()
                    {
                        new EphemerisTableRowVector(DateTime.Today, 0, 0)
                    },
                    new ObjectData("Sun", 10, 500000, 3, double.PositiveInfinity, 1, 50, 3000, 0, 0, 0, "Star")
                ),
                new EphemerisVectorData(new List<EphemerisTableRowVector>()
                    {
                        new EphemerisTableRowVector(DateTime.Today, 150_000_000, 0)
                    },
                    new ObjectData("Earth", 10, 500000, 3, double.PositiveInfinity, 1, 50, 3000, 0, 0, 0, "Planet")
                )
            };

            this.BackColor = Color.Black;
            
        }
        protected override void PrintObjects(object sender, PaintEventArgs e)
        {
            foreach (var data in testData)
            {
                Point center = new Point(this.DisplayRectangle.Width / 2, this.Height / 2);
                var formBody = data.ToFormBody(center, 1000_000, this.Height, this.Width);
                if (formBody.PixelInfos[0].Visible) drawFormBody(formBody.PixelInfos[this._pictureIndex], e, (formBody.PixelInfos[0].ShowName) ? formBody.BodyData.objectData.Name : null);
            }
        }

        


        protected override void BodyClick(object sender, MouseEventArgs e)
        {
            foreach (var data in testData)
            {
                Point center = new Point(this.DisplayRectangle.Width / 2, this.Height / 2);
                var formBody = data.ToFormBody(center, 1000_000, this.Height, this.Width);
                var centerX = formBody.PixelInfos[0].BodyCoordinates.X + formBody.PixelInfos[0].Diameter / 2;
                var centerY = formBody.PixelInfos[0].BodyCoordinates.Y + formBody.PixelInfos[0].Diameter / 2;
                var distance = System.Math.Sqrt((centerX - e.X) * (centerX - e.X) + (centerY - e.Y) * (centerY - e.Y));
                if (distance < formBody.PixelInfos[0].Diameter / 2) ShowBodyReport(formBody.BodyReport(formBody.BodyData.ephemerisTable[this._pictureIndex].date.Value));
            }

        }
        
    }
}
