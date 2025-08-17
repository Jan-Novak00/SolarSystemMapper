using SolarSystemMapper;
using System.Diagnostics;
using System.Windows.Forms;

namespace SolarMapperUI
{
    public partial class SolarMapperUI : Form
    {

        private Panel _solarSystemPanel;
        private List<EphemerisVectorData> testData;
        public SolarMapperUI()
        {
            InitializeComponent();


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



            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.BackColor = Color.Black;

            // Vytvoøení panelu, který vyplní celé okno
            _solarSystemPanel = new Panel();
            _solarSystemPanel.Dock = DockStyle.Fill;
            _solarSystemPanel.BackColor = Color.Black;
            //_solarSystemPanel.Paint += DrawPanel_Paint; // pøipojení události Paint
            this.Controls.Add(_solarSystemPanel);
            _solarSystemPanel.Paint += PrintObjects;
            _solarSystemPanel.MouseClick += PlanetClick;

        }

        private void _showClosingMessage()
        {
            var closingLabel = new Label
            {
                Text = "The app may be closed by pressing Esc key.",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Umístìní labelu do støedu panelu
            closingLabel.Location = new Point(
                (_solarSystemPanel.Width - closingLabel.PreferredWidth) / 2,
                (closingLabel.PreferredHeight)
            );
            var hideTimer = new System.Windows.Forms.Timer();
            hideTimer.Interval = 3000;
            _solarSystemPanel.Controls.Add(closingLabel);
            hideTimer.Tick += (s, e) =>
            {
                closingLabel.Visible = false;
                hideTimer.Stop();
            };
            hideTimer.Start();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SolarSystemMap_Load(object sender, EventArgs e)
        {
            this._showClosingMessage();
        }

        private void PrintObjects(object sender, PaintEventArgs e)
        {
            foreach (var data in testData)
            {
                Point center = new Point(_solarSystemPanel.DisplayRectangle.Width / 2, _solarSystemPanel.Height / 2);
                var formBody = data.ToFormBody(center, 1000_000, _solarSystemPanel.Height, _solarSystemPanel.Width);
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
            float textX = leftCornerX - textSize.Width / 2 + diameter/2;
            float textY = leftCornerY + diameter / 2 + 10;
            e.Graphics.DrawString(name, DefaultFont, Brushes.White,textX,textY);
        }


        private void PlanetClick(object sender, MouseEventArgs e)
        {
            foreach (var data in testData)
            {
                Point center = new Point(_solarSystemPanel.DisplayRectangle.Width / 2, _solarSystemPanel.Height / 2);
                var formBody = data.ToFormBody(center, 1000_000, _solarSystemPanel.Height, _solarSystemPanel.Width);
                var centerX = formBody.PixelInfos[0].BodyCoordinates.X + formBody.PixelInfos[0].Diameter / 2;
                var centerY = formBody.PixelInfos[0].BodyCoordinates.Y + formBody.PixelInfos[0].Diameter / 2;
                var distance = System.Math.Sqrt((centerX - e.X)*(centerX - e.X) + (centerY - e.Y)* (centerY - e.Y));
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

            // vytvoøení labelu s textem
            var label = new Label();
            label.Text = report;
            label.ForeColor = Color.White;
            label.AutoSize = true;
            label.MaximumSize = new Size(700, 0); // maximální šíøka
            label.Location = new Point(10, 10);

            reportForm.Controls.Add(label);

            // spoèítání velikosti formuláøe podle labelu
            reportForm.ClientSize = new Size(label.Width + 20, label.Height + 20);

            // pozice v pravém dolním rohu panelu
            var panelArea = _solarSystemPanel.ClientRectangle;
            reportForm.Location = _solarSystemPanel.PointToScreen(new Point(
                panelArea.Right - reportForm.Width - 20,
                panelArea.Bottom - reportForm.Height - 20
            ));

            reportForm.Show(this); // zobrazí okno nad hlavním formuláøem
        }


    }
}
