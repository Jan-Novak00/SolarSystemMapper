using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarMapperUI
{
    internal abstract class MapPanel : Panel
    {
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

        protected abstract void PrintObjects(object sender, PaintEventArgs e);
        protected abstract void BodyClick(object sender, MouseEventArgs e);

        protected virtual void drawFormBody(List<PixelBodyInfo> pixelInfos, PaintEventArgs e, string? name)
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
