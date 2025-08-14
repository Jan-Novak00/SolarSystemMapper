namespace SolarMapperUI
{
    public partial class SolarMapperUI : Form
    {

        private Panel _solarSystemPanel;
        public SolarMapperUI()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;

            // Vytvoøení panelu, který vyplní celé okno
            _solarSystemPanel = new Panel();
            _solarSystemPanel.Dock = DockStyle.Fill;
            _solarSystemPanel.BackColor = Color.Black;
            _solarSystemPanel.Paint += DrawPanel_Paint; // pøipojení události Paint
            this.Controls.Add(_solarSystemPanel);
            _solarSystemPanel.MouseClick += drawPanel_MouseClick;
            _solarSystemPanel.Paint += g_color;
            _solarSystemPanel.Paint += drawPanel_Paint;

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
        private void DrawPanel_Paint(object sender, PaintEventArgs e)
        {
            int diameter = 30;
            int x = (_solarSystemPanel.Width - diameter) / 2;
            int y = (_solarSystemPanel.Height - diameter) / 2;

            e.Graphics.FillEllipse(Brushes.White, x, y, diameter, diameter);
        }

        private void drawPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int centerX = _solarSystemPanel.Width / 2;
            int centerY = _solarSystemPanel.Height / 2;
            int radius = 15; // polomìr 30px kruhu

            // Spoèítáme vzdálenost kliknutí od støedu
            int dx = e.X - centerX;
            int dy = e.Y - centerY;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance <= radius)
            {
                g(); // volání vaší metody
            }
        }
        private void g()
        {
            centerDiameter = 30;
            _solarSystemPanel.Invalidate();
        }

        Color centerColor = Color.Blue;
        int centerDiameter;

        private void g_color(object sender, PaintEventArgs e)
        {
            int x = (_solarSystemPanel.Width - centerDiameter) / 2;
            int y = (_solarSystemPanel.Height - centerDiameter) / 2;
            using (Brush brush = new SolidBrush(centerColor))
            {
                e.Graphics.FillEllipse(brush, x, y, centerDiameter, centerDiameter);
            }
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


        private PointF KmToPixels(double xKm, double yKm)
        {
            //float scale = 1_000_000f; // 1 pixel = 1 milion km
            float scale = 10_000_000f;
            float xPx = (float)(xKm / scale);
            float yPx = (float)(yKm / scale);
            return new PointF(xPx, yPx);
        }
        private void DrawPlanet(Graphics g, double xKm, double yKm)
        {
            // pøepoèet na pixely
            PointF planetPx = KmToPixels(xKm, yKm);

            // centrum formuláøe
            float centerX = _solarSystemPanel.Width / 2f;
            float centerY = _solarSystemPanel.Height / 2f;

            // skuteèná pozice pro kreslení
            float drawX = centerX + planetPx.X;
            float drawY = centerY + planetPx.Y;

            float diameter = 6f; // velikost planety v pixelech
            g.FillEllipse(Brushes.White, drawX - diameter / 2, drawY - diameter / 2, diameter, diameter);
        }
        private void drawPanel_Paint(object sender, PaintEventArgs e)
        {
            // pøíklad: Zemì 150 milionù km od Slunce ve smìru X
            DrawPlanet(e.Graphics, 57_000, 0);
            DrawPlanet(e.Graphics, 108_000_000, 0);
            DrawPlanet(e.Graphics, 150_000_000, 0);
            DrawPlanet(e.Graphics, 227_000_000, 0);
            DrawPlanet(e.Graphics, 778_000_000, 0);
            DrawPlanet(e.Graphics, 1_434_000_000, 0);
            DrawPlanet(e.Graphics, 2_871_000_000, 0);
            DrawPlanet(e.Graphics, 4_495_000_000, 0);
        }
    }
}
