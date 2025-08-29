using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarMapperUI
{
    public partial class SolarMapperUI : Form
    {

        private class ControlForm : Form
        {
            private IMap _map { get; init; }
            private Label _dateLabel { get; set; } = new Label();
            private System.Windows.Forms.Button _singleStepButton { get; set; } = new Button();

            private System.Windows.Forms.Timer _autoTimer { get; set; } = new System.Windows.Forms.Timer();

            private bool _timerRunning { get; set; } = false;

            private System.Windows.Forms.Button _autoButton { get; set; } = new Button();


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

                
                if (_map is SateliteMap)
                {
                    Button backButton = new Button
                    {
                        Text = "Back",
                        AutoSize = true,
                        Location = new Point(_autoButton.Right + 10, _autoButton.Top) 
                    };
                    backButton.Click += (s, e) => this._returnBackClicked();
                    this.Controls.Add(backButton);
                }

                PreventDisposeOnClose();

            }

            private void _returnBackClicked()
            {
                if (_map is SateliteMap map) 
                {
                    map.ReturnBack();
                }
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
                if(!_timerRunning)
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
}
