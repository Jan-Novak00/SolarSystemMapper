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
    internal abstract class MapPanel<TData> : Panel where TData : IEphemerisData<IEphemerisTableRow>
    {
        protected List<TData> _originalData;

        protected List<FormBody<TData>> _data;

        protected int _pictureIndex { get; set; } = 0;
        protected DateTime _currentPictureDate { get; set; }

        protected MapPanel()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.Black;

            
            this.InitializeHandlers();

            
        }

        protected virtual void InitializeHandlers()
        {

            this.MouseClick += BodyClick;
            this.Paint += PrintObjects;
        }

        protected abstract List<FormBody<TData>> _prepareBodyData(List<TData> data);

        public virtual void AdvanceMap()
        {
            this._pictureIndex = (this._pictureIndex + 1 == _data[0].BodyData.ephemerisTable.Count()) ? 0 : this._pictureIndex + 1;
            this._currentPictureDate = _data[0].BodyData.ephemerisTable[this._pictureIndex].date.Value;
            this.Invalidate();
        }


        protected abstract void PrintObjects(object sender, PaintEventArgs e);
        protected abstract void BodyClick(object sender, MouseEventArgs e);

        protected virtual void drawFormBody(PixelBodyInfo pixelInfo, PaintEventArgs e, string? name)
        {
            var brush = new SolidBrush(pixelInfo.Color);
            var leftCornerX = pixelInfo.BodyCoordinates.X;
            var leftCornerY = pixelInfo.BodyCoordinates.Y;
            var diameter = pixelInfo.Diameter;
            e.Graphics.FillEllipse(brush, leftCornerX, leftCornerY, diameter, diameter);
            var textSize = e.Graphics.MeasureString(name, DefaultFont);
            float textX = leftCornerX - textSize.Width / 2 + diameter / 2;
            float textY = leftCornerY + diameter / 2 + 10;
            e.Graphics.DrawString(name, DefaultFont, Brushes.White, textX, textY);
        }

        protected void ShowBodyReport(string report)
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

            var button = new System.Windows.Forms.Button();
            button.Text = "start";
            button.AutoSize = true;
            button.Location = new Point(10, label.Bottom + 10); // pod labelem
            button.Click += (s, e) => this.TestClick(); // výpis do debug konzole
            reportForm.Controls.Add(button);


            // spočítání velikosti formuláře podle labelu
            int width = Math.Max(label.Right, button.Right) + 10;
            int height = Math.Max(label.Bottom, button.Bottom) + 10;
            reportForm.ClientSize = new Size(width, height);

            // pozice v pravém dolním rohu panelu
            var panelArea = this.ClientRectangle;
            reportForm.Location = this.PointToScreen(new Point(
                panelArea.Right - reportForm.Width - 20,
                panelArea.Bottom - reportForm.Height - 20
            ));

            reportForm.Show(this); // zobrazí okno nad hlavním formulářem
        }

        private void TestClick() => Debug.WriteLine("click");
        





    }
}
