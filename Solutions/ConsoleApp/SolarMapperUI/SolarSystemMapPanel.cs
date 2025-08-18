using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarMapperUI
{
    internal class SolarSystemMapPanel : MapPanel
    {
        private List<EphemerisVectorData> testData;
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
                var showName = formBody.BodyData.objectData.Type == "Planet" || formBody.BodyData.objectData.Type == "Star";
                if (formBody.PixelInfos[0].Visible) drawFormBody(formBody.PixelInfos, e, (showName) ? formBody.BodyData.objectData.Name : null);
            }
        }

        private void drawFormBody(List<PixelBodyInfo> pixelInfos, PaintEventArgs e, string? name)
        {
            var brush = new SolidBrush(pixelInfos[0].Color);
            var leftCornerX = pixelInfos[0].BodyCoordinates.X;
            var leftCornerY = pixelInfos[0].BodyCoordinates.Y;
            var diameter = pixelInfos[0].Diameter;
            e.Graphics.FillEllipse(brush, leftCornerX, leftCornerY, diameter, diameter);
            var textSize = e.Graphics.MeasureString(name, DefaultFont);
            float textX = leftCornerX - textSize.Width / 2 + diameter / 2;
            float textY = leftCornerY + diameter / 2 + 10;
            e.Graphics.DrawString(name, DefaultFont, Brushes.White, textX, textY);
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
                if (distance < formBody.PixelInfos[0].Diameter / 2) ShowBodyReport(formBody.BodyReport(DateTime.Today));
            }

        }
        private void ShowBodyReport(string report)
        {
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

            // spočítání velikosti formuláře podle labelu
            reportForm.ClientSize = new Size(label.Width + 20, label.Height + 20);

            // pozice v pravém dolním rohu panelu
            var panelArea = this.ClientRectangle;
            reportForm.Location = this.PointToScreen(new Point(
                panelArea.Right - reportForm.Width - 20,
                panelArea.Bottom - reportForm.Height - 20
            ));

            reportForm.Show(this); // zobrazí okno nad hlavním formulářem
        }
    }
}
