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

    internal interface IMap
    {
        public void AdvanceMap();
        public DateTime CurrentPictureDate { get; }
    }

    internal abstract class MapPanel<TData> : Panel, IMap
        where TData : IEphemerisData<IEphemerisTableRow>
    {
        protected List<TData> _originalData;

        protected List<FormBody<TData>> _data;

        protected List<ObjectEntry> objects;

        protected virtual NASAHorizonsDataFetcher.MapMode _mode { get; }

        protected int _pictureIndex { get; set; } = 0;
        public DateTime CurrentPictureDate { get; protected set; }


        private Label _loadingLabel = new Label
            {
                Text = "Loading...",
                ForeColor = Color.LimeGreen,
                AutoSize = true,
                Visible = false,
            };

        protected abstract List<FormBody<TData>> _prepareBodyData(List<TData> data);
        protected virtual async Task<IReadOnlyList<TData>> GetHorizonsData(List<ObjectEntry> objects)
        {
            var fetcher = new NASAHorizonsDataFetcher(_mode, objects, this.CurrentPictureDate, this.CurrentPictureDate.AddDays(10));
            var result = await fetcher.Fetch();
            return result.Cast<TData>().ToList().AsReadOnly();
        }
        private void SetData()
        {
            _data = _prepareBodyData(_originalData);
        }

        protected async Task SettingDataAsync()
        {
            _loadingLabel.Location = new Point(this.Width/2, this.Height/2);
            _loadingLabel.Visible = true;
            _originalData = (await GetHorizonsData(objects)).ToList();
            SetData();
            _loadingLabel.Visible = false;

            this.Invalidate();
        }


        protected MapPanel()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.Black;
            this.Controls.Add(_loadingLabel);

            this.InitializeHandlers();

            
        }

        protected virtual void InitializeHandlers()
        {

            this.MouseClick += BodyClick;
            this.Paint += PrintObjects;
        }

        public virtual async void AdvanceMap()
        {
            if (this._data == null || this._data.Count == 0) return;

            if (this._pictureIndex + 1 == _data[0].BodyData.ephemerisTable.Count)
            {
                this._data = null;
                this._pictureIndex = 0;
                this.CurrentPictureDate = this.CurrentPictureDate.AddDays(1);
                await SettingDataAsync();
            }
            else
            {
                _pictureIndex++;
                CurrentPictureDate = _data[0].BodyData.ephemerisTable[_pictureIndex].date.Value;
            }
            this.Invalidate();

        }


        protected virtual void PrintObjects(object sender, PaintEventArgs e)
        {
            if (_data == null) return;
            foreach (var formBody in this._data)
            {
                if (formBody.PixelInfos[this._pictureIndex].Visible) drawFormBody(formBody.PixelInfos[this._pictureIndex], e, (formBody.PixelInfos[this._pictureIndex].ShowName) ? formBody.BodyData.objectData.Name : null);
            }
        }
        protected virtual void BodyClick(object sender, MouseEventArgs e)
        {
            if (_data == null) return;
            foreach (var formBody in _data)
            {
                var centerX = formBody.PixelInfos[this._pictureIndex].BodyCoordinates.X + formBody.PixelInfos[this._pictureIndex].Diameter / 2;
                var centerY = formBody.PixelInfos[this._pictureIndex].BodyCoordinates.Y + formBody.PixelInfos[this._pictureIndex].Diameter / 2;
                var distance = Math.Sqrt((centerX - e.X) * (centerX - e.X) + (centerY - e.Y) * (centerY - e.Y));
                if (distance < formBody.PixelInfos[this._pictureIndex].Diameter / 2) ShowBodyReport(formBody.BodyReport(formBody.BodyData.ephemerisTable[this._pictureIndex].date.Value));
            }
        }

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
