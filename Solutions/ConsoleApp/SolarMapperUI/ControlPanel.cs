using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarMapperUI
{
        /**
         * Used to control map.
         */
        internal class ControlForm : Form
        {
            private IMap _map { get; init; }
            private Label _dateLabel { get; set; } = new Label();
            private System.Windows.Forms.Button _singleStepButton { get; set; } = new Button();

            private System.Windows.Forms.Timer _autoTimer { get; set; } = new System.Windows.Forms.Timer();

            private bool _timerRunning { get; set; } = false;

            private System.Windows.Forms.Button _autoButton { get; set; } = new Button();

            private System.Windows.Forms.Label _scaleLabel { get; set; } = new Label();


            /**
             * This method ensures automatic map advance won't break when control form is closed
             */
            private void PreventDisposeOnClose()
                {
                    this.FormClosing += (s, e) =>
                    {
                        if (e.CloseReason == CloseReason.UserClosing)
                        {
                            e.Cancel = true;
                            this.Hide();
                        }
                    };
                }

            public ControlForm(IMap mapPanel)
            {
                _map = mapPanel;

                this.Text = "Control";
                this.StartPosition = FormStartPosition.Manual;
                this.BackColor = Color.Black;
                this.ForeColor = Color.White;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.ShowInTaskbar = false;
                this.AutoSize = true;
                this.AutoSizeMode = AutoSizeMode.GrowAndShrink;


                _dateLabel.AutoSize = true;
                _dateLabel.Location = new Point(10, 10);
                _dateLabel.ForeColor = Color.LimeGreen;
                this.Controls.Add(_dateLabel);
                this._setDateLabelText();

                _singleStepButton.Text = ">>";
                _singleStepButton.AutoSize = true;
                _singleStepButton.Location = new Point(10, 40);
                _singleStepButton.Click += (s, e) => this._changeMap();
                this.Controls.Add(this._singleStepButton);

                _autoTimer.Interval = 500;

                _autoButton.Text = "Start";
                _autoButton.AutoSize = true;
                _autoButton.Location = new Point(10, 80);
                _autoButton.Click += (s, e) => this._autoButtonClick();
                this.Controls.Add(_autoButton);


                if (_map is SateliteMapPanel sateliteMap) //back button for moon map
                {
                    Button backButton = new Button();

                    backButton.Text = "Back";
                    backButton.AutoSize = true;
                    backButton.Location = new Point(_autoButton.Right + 10, _autoButton.Top);
                    backButton.Click += (s, e) => sateliteMap.ReturnBack();
                    this.Controls.Add(backButton);
                }

                if (_map is SolarSystemMapPanel systemMap) // zoom buttons
                {   

                
                    Button zoomInButton = new Button();
                    zoomInButton.Text = "Zoom out";
                    zoomInButton.AutoSize = true;
                    zoomInButton.Location = new Point(10, 120);
                    zoomInButton.Click += (s, e) =>
                    {
                        systemMap.InvokeScaleSwitchEvent(systemMap.Scale_km * 2);
                        this._scaleLabel.Text = $"Scale: 1px = {systemMap.Scale_km.ToString()} km";
                    };
                    this.Controls.Add(zoomInButton);

                    Button zoomOutButton = new Button();
                    zoomOutButton.Text = "Zoom in";
                    zoomOutButton.AutoSize = true;
                    zoomOutButton.Location = new Point(5 + zoomInButton.Right, 120);
                    zoomOutButton.Click += (s, e) =>
                    {
                        systemMap.InvokeScaleSwitchEvent(systemMap.Scale_km / 2);
                        this._scaleLabel.Text = $"Scale: 1px = {systemMap.Scale_km.ToString()} km";
                    };

                    this.Controls.Add(zoomOutButton);
                    
                    this._scaleLabel.AutoSize = true;
                    this._scaleLabel.Location = new Point(10,160);
                    this._scaleLabel.Text = $"Scale: 1px = {systemMap.Scale_km.ToString()} km";
                    this.Controls.Add(_scaleLabel);
                    
                    
                }

                PreventDisposeOnClose();

            }



            private void _setDateLabelText()
            {
                _dateLabel.Text = this._map.CurrentPictureDate.ToString();
            }

            private void _changeMap()
            {
                this._map.AdvanceMap();
                this._setDateLabelText();
            }

            private void _autoButtonClick()
            {
                if (!_timerRunning)
                {
                    _autoTimer.Tick += (ts, te) =>
                    {
                        this._changeMap();
                    };
                    _autoTimer.Start();
                    _autoButton.Text = "Stop";
                    _timerRunning = true;
                }
                else
                {
                    _autoTimer.Stop();
                    _autoButton.Text = "Start";
                    _timerRunning = false;
                    _autoTimer.Tick -= null;
                }
            }




        }






    
}
