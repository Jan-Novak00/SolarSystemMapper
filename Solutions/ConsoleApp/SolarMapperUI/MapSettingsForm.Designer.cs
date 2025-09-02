namespace SolarMapperUI
{
    partial class MapSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            label1 = new Label();
            MapType_ComboBox = new ComboBox();
            Date_TimePicker = new DateTimePicker();
            label2 = new Label();
            Coordinates_Label = new Label();
            Coordinates_TextBox = new TextBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            label4 = new Label();
            ObjectTypes_CheckedListBox = new CheckedListBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            MinMass_TextBox = new TextBox();
            MaxMass_TextBox = new TextBox();
            label6 = new Label();
            label7 = new Label();
            label5 = new Label();
            WhiteList_TextBox = new TextBox();
            BlackList_TextBox = new TextBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            MaxRadius_TextBox = new TextBox();
            MinRadius_TextBox = new TextBox();
            label13 = new Label();
            label8 = new Label();
            tableLayoutPanel7 = new TableLayoutPanel();
            MaxOrbitalPeriod_TextBox = new TextBox();
            MinOrbitalPeriod_TextBox = new TextBox();
            label14 = new Label();
            label9 = new Label();
            tableLayoutPanel8 = new TableLayoutPanel();
            MaxDensity_TextBox = new TextBox();
            MinDensity_TextBox = new TextBox();
            label15 = new Label();
            label10 = new Label();
            tableLayoutPanel9 = new TableLayoutPanel();
            MaxGravity_TextBox = new TextBox();
            MinGravity_TextBox = new TextBox();
            label16 = new Label();
            label11 = new Label();
            tableLayoutPanel10 = new TableLayoutPanel();
            MaxSpeed_TextBox = new TextBox();
            label12 = new Label();
            MinSpeed_TextBox = new TextBox();
            label17 = new Label();
            label18 = new Label();
            label19 = new Label();
            label20 = new Label();
            label21 = new Label();
            label22 = new Label();
            Speed_Label = new Label();
            label24 = new Label();
            label25 = new Label();
            NextPage_Button = new Button();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            tableLayoutPanel9.SuspendLayout();
            tableLayoutPanel10.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel5, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(2004, 1136);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(MapType_ComboBox, 1, 0);
            tableLayoutPanel2.Controls.Add(Date_TimePicker, 1, 1);
            tableLayoutPanel2.Controls.Add(label2, 0, 1);
            tableLayoutPanel2.Controls.Add(Coordinates_Label, 0, 2);
            tableLayoutPanel2.Controls.Add(Coordinates_TextBox, 1, 2);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Size = new Size(996, 562);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI", 14F);
            label1.ForeColor = Color.DodgerBlue;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(325, 51);
            label1.TabIndex = 0;
            label1.Text = "Choose Map Type";
            // 
            // MapType_ComboBox
            // 
            MapType_ComboBox.FormattingEnabled = true;
            MapType_ComboBox.Items.AddRange(new object[] { "Night Sky", "Solar System" });
            MapType_ComboBox.Location = new Point(334, 3);
            MapType_ComboBox.Name = "MapType_ComboBox";
            MapType_ComboBox.Size = new Size(326, 40);
            MapType_ComboBox.TabIndex = 1;
            MapType_ComboBox.SelectedIndexChanged += MapType_ComboBox_SelectedIndexChanged;
            // 
            // Date_TimePicker
            // 
            Date_TimePicker.Location = new Point(334, 190);
            Date_TimePicker.Name = "Date_TimePicker";
            Date_TimePicker.Size = new Size(326, 39);
            Date_TimePicker.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14F);
            label2.ForeColor = Color.LimeGreen;
            label2.Location = new Point(3, 187);
            label2.Name = "label2";
            label2.Size = new Size(240, 51);
            label2.TabIndex = 3;
            label2.Text = "Choose Time";
            // 
            // Coordinates_Label
            // 
            Coordinates_Label.AutoSize = true;
            Coordinates_Label.Font = new Font("Segoe UI", 12F);
            Coordinates_Label.ForeColor = Color.IndianRed;
            Coordinates_Label.Location = new Point(3, 374);
            Coordinates_Label.Name = "Coordinates_Label";
            Coordinates_Label.Size = new Size(308, 45);
            Coordinates_Label.TabIndex = 4;
            Coordinates_Label.Text = "Choose Coordinates";
            // 
            // Coordinates_TextBox
            // 
            Coordinates_TextBox.Location = new Point(334, 377);
            Coordinates_TextBox.Name = "Coordinates_TextBox";
            Coordinates_TextBox.Size = new Size(326, 39);
            Coordinates_TextBox.TabIndex = 5;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(label4, 0, 0);
            tableLayoutPanel3.Controls.Add(ObjectTypes_CheckedListBox, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 571);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 14.24501F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 85.75499F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(996, 562);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Segoe UI", 20F);
            label4.Location = new Point(3, 0);
            label4.Name = "label4";
            label4.Size = new Size(990, 80);
            label4.TabIndex = 0;
            label4.Text = "Choose Object Types";
            // 
            // ObjectTypes_CheckedListBox
            // 
            ObjectTypes_CheckedListBox.Dock = DockStyle.Fill;
            ObjectTypes_CheckedListBox.FormattingEnabled = true;
            ObjectTypes_CheckedListBox.Items.AddRange(new object[] { "Stars", "Terrestrial Planets", "Gas Giants", "Dwarf Planets", "Asteroids", "Comets", "Spacecraft" });
            ObjectTypes_CheckedListBox.Location = new Point(3, 83);
            ObjectTypes_CheckedListBox.Name = "ObjectTypes_CheckedListBox";
            ObjectTypes_CheckedListBox.Size = new Size(990, 476);
            ObjectTypes_CheckedListBox.TabIndex = 1;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(tableLayoutPanel4, 1, 1);
            tableLayoutPanel5.Controls.Add(label5, 0, 0);
            tableLayoutPanel5.Controls.Add(WhiteList_TextBox, 1, 7);
            tableLayoutPanel5.Controls.Add(BlackList_TextBox, 1, 8);
            tableLayoutPanel5.Controls.Add(tableLayoutPanel6, 1, 2);
            tableLayoutPanel5.Controls.Add(tableLayoutPanel7, 1, 3);
            tableLayoutPanel5.Controls.Add(tableLayoutPanel8, 1, 4);
            tableLayoutPanel5.Controls.Add(tableLayoutPanel9, 1, 5);
            tableLayoutPanel5.Controls.Add(tableLayoutPanel10, 1, 6);
            tableLayoutPanel5.Controls.Add(label18, 0, 1);
            tableLayoutPanel5.Controls.Add(label19, 0, 2);
            tableLayoutPanel5.Controls.Add(label20, 0, 3);
            tableLayoutPanel5.Controls.Add(label21, 0, 4);
            tableLayoutPanel5.Controls.Add(label22, 0, 5);
            tableLayoutPanel5.Controls.Add(Speed_Label, 0, 6);
            tableLayoutPanel5.Controls.Add(label24, 0, 7);
            tableLayoutPanel5.Controls.Add(label25, 0, 8);
            tableLayoutPanel5.Controls.Add(NextPage_Button, 1, 10);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(1005, 3);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 11;
            tableLayoutPanel1.SetRowSpan(tableLayoutPanel5, 2);
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.Size = new Size(996, 1130);
            tableLayoutPanel5.TabIndex = 4;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(MinMass_TextBox, 1, 0);
            tableLayoutPanel4.Controls.Add(MaxMass_TextBox, 1, 1);
            tableLayoutPanel4.Controls.Add(label6, 0, 0);
            tableLayoutPanel4.Controls.Add(label7, 0, 1);
            tableLayoutPanel4.Location = new Point(501, 53);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 3;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(492, 102);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // MinMass_TextBox
            // 
            MinMass_TextBox.Location = new Point(249, 3);
            MinMass_TextBox.Name = "MinMass_TextBox";
            MinMass_TextBox.Size = new Size(237, 39);
            MinMass_TextBox.TabIndex = 1;
            MinMass_TextBox.Text = "0";
            // 
            // MaxMass_TextBox
            // 
            MaxMass_TextBox.Location = new Point(249, 44);
            MaxMass_TextBox.Name = "MaxMass_TextBox";
            MaxMass_TextBox.Size = new Size(237, 39);
            MaxMass_TextBox.TabIndex = 2;
            MaxMass_TextBox.Text = "Infinity";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(3, 0);
            label6.Name = "label6";
            label6.Size = new Size(118, 32);
            label6.TabIndex = 3;
            label6.Text = "Minimum";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(3, 41);
            label7.Name = "label7";
            label7.Size = new Size(121, 32);
            label7.TabIndex = 4;
            label7.Text = "Maximum";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 15F);
            label5.ForeColor = Color.BlueViolet;
            label5.Location = new Point(3, 0);
            label5.Name = "label5";
            label5.Size = new Size(276, 50);
            label5.TabIndex = 17;
            label5.Text = "General Filtres";
            // 
            // WhiteList_TextBox
            // 
            WhiteList_TextBox.Dock = DockStyle.Fill;
            WhiteList_TextBox.Location = new Point(501, 701);
            WhiteList_TextBox.Multiline = true;
            WhiteList_TextBox.Name = "WhiteList_TextBox";
            WhiteList_TextBox.Size = new Size(492, 102);
            WhiteList_TextBox.TabIndex = 18;
            // 
            // BlackList_TextBox
            // 
            BlackList_TextBox.Dock = DockStyle.Fill;
            BlackList_TextBox.Location = new Point(501, 809);
            BlackList_TextBox.Multiline = true;
            BlackList_TextBox.Name = "BlackList_TextBox";
            BlackList_TextBox.Size = new Size(492, 102);
            BlackList_TextBox.TabIndex = 19;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 2;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Controls.Add(MaxRadius_TextBox, 1, 1);
            tableLayoutPanel6.Controls.Add(MinRadius_TextBox, 1, 0);
            tableLayoutPanel6.Controls.Add(label13, 0, 1);
            tableLayoutPanel6.Controls.Add(label8, 0, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(501, 161);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 2;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Size = new Size(492, 102);
            tableLayoutPanel6.TabIndex = 20;
            // 
            // MaxRadius_TextBox
            // 
            MaxRadius_TextBox.Location = new Point(249, 54);
            MaxRadius_TextBox.Name = "MaxRadius_TextBox";
            MaxRadius_TextBox.Size = new Size(237, 39);
            MaxRadius_TextBox.TabIndex = 6;
            MaxRadius_TextBox.Text = "Infinity";
            // 
            // MinRadius_TextBox
            // 
            MinRadius_TextBox.Location = new Point(249, 3);
            MinRadius_TextBox.Name = "MinRadius_TextBox";
            MinRadius_TextBox.Size = new Size(237, 39);
            MinRadius_TextBox.TabIndex = 5;
            MinRadius_TextBox.Text = "0";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(3, 51);
            label13.Name = "label13";
            label13.Size = new Size(121, 32);
            label13.TabIndex = 5;
            label13.Text = "Maximum";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(3, 0);
            label8.Name = "label8";
            label8.Size = new Size(118, 32);
            label8.TabIndex = 4;
            label8.Text = "Minimum";
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 2;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.Controls.Add(MaxOrbitalPeriod_TextBox, 1, 1);
            tableLayoutPanel7.Controls.Add(MinOrbitalPeriod_TextBox, 1, 0);
            tableLayoutPanel7.Controls.Add(label14, 0, 1);
            tableLayoutPanel7.Controls.Add(label9, 0, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(501, 269);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 3;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.Size = new Size(492, 102);
            tableLayoutPanel7.TabIndex = 21;
            // 
            // MaxOrbitalPeriod_TextBox
            // 
            MaxOrbitalPeriod_TextBox.Location = new Point(249, 44);
            MaxOrbitalPeriod_TextBox.Name = "MaxOrbitalPeriod_TextBox";
            MaxOrbitalPeriod_TextBox.Size = new Size(237, 39);
            MaxOrbitalPeriod_TextBox.TabIndex = 8;
            MaxOrbitalPeriod_TextBox.Text = "Infinity";
            // 
            // MinOrbitalPeriod_TextBox
            // 
            MinOrbitalPeriod_TextBox.Location = new Point(249, 3);
            MinOrbitalPeriod_TextBox.Name = "MinOrbitalPeriod_TextBox";
            MinOrbitalPeriod_TextBox.Size = new Size(237, 39);
            MinOrbitalPeriod_TextBox.TabIndex = 7;
            MinOrbitalPeriod_TextBox.Text = "0";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(3, 41);
            label14.Name = "label14";
            label14.Size = new Size(121, 32);
            label14.TabIndex = 6;
            label14.Text = "Maximum";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(3, 0);
            label9.Name = "label9";
            label9.Size = new Size(118, 32);
            label9.TabIndex = 5;
            label9.Text = "Minimum";
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 2;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.Controls.Add(MaxDensity_TextBox, 1, 1);
            tableLayoutPanel8.Controls.Add(MinDensity_TextBox, 1, 0);
            tableLayoutPanel8.Controls.Add(label15, 0, 1);
            tableLayoutPanel8.Controls.Add(label10, 0, 0);
            tableLayoutPanel8.Dock = DockStyle.Fill;
            tableLayoutPanel8.Location = new Point(501, 377);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 3;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel8.Size = new Size(492, 102);
            tableLayoutPanel8.TabIndex = 22;
            // 
            // MaxDensity_TextBox
            // 
            MaxDensity_TextBox.Location = new Point(249, 44);
            MaxDensity_TextBox.Name = "MaxDensity_TextBox";
            MaxDensity_TextBox.Size = new Size(237, 39);
            MaxDensity_TextBox.TabIndex = 10;
            MaxDensity_TextBox.Text = "Infinity";
            // 
            // MinDensity_TextBox
            // 
            MinDensity_TextBox.Location = new Point(249, 3);
            MinDensity_TextBox.Name = "MinDensity_TextBox";
            MinDensity_TextBox.Size = new Size(237, 39);
            MinDensity_TextBox.TabIndex = 9;
            MinDensity_TextBox.Text = "0";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(3, 41);
            label15.Name = "label15";
            label15.Size = new Size(121, 32);
            label15.TabIndex = 7;
            label15.Text = "Maximum";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(3, 0);
            label10.Name = "label10";
            label10.Size = new Size(118, 32);
            label10.TabIndex = 6;
            label10.Text = "Minimum";
            // 
            // tableLayoutPanel9
            // 
            tableLayoutPanel9.ColumnCount = 2;
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.Controls.Add(MaxGravity_TextBox, 1, 1);
            tableLayoutPanel9.Controls.Add(MinGravity_TextBox, 1, 0);
            tableLayoutPanel9.Controls.Add(label16, 0, 1);
            tableLayoutPanel9.Controls.Add(label11, 0, 0);
            tableLayoutPanel9.Dock = DockStyle.Fill;
            tableLayoutPanel9.Location = new Point(501, 485);
            tableLayoutPanel9.Name = "tableLayoutPanel9";
            tableLayoutPanel9.RowCount = 3;
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel9.Size = new Size(492, 102);
            tableLayoutPanel9.TabIndex = 23;
            // 
            // MaxGravity_TextBox
            // 
            MaxGravity_TextBox.Location = new Point(249, 44);
            MaxGravity_TextBox.Name = "MaxGravity_TextBox";
            MaxGravity_TextBox.Size = new Size(237, 39);
            MaxGravity_TextBox.TabIndex = 12;
            MaxGravity_TextBox.Text = "Infinity";
            // 
            // MinGravity_TextBox
            // 
            MinGravity_TextBox.Location = new Point(249, 3);
            MinGravity_TextBox.Name = "MinGravity_TextBox";
            MinGravity_TextBox.Size = new Size(237, 39);
            MinGravity_TextBox.TabIndex = 11;
            MinGravity_TextBox.Text = "0";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(3, 41);
            label16.Name = "label16";
            label16.Size = new Size(121, 32);
            label16.TabIndex = 8;
            label16.Text = "Maximum";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(3, 0);
            label11.Name = "label11";
            label11.Size = new Size(118, 32);
            label11.TabIndex = 7;
            label11.Text = "Minimum";
            // 
            // tableLayoutPanel10
            // 
            tableLayoutPanel10.ColumnCount = 2;
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel10.Controls.Add(MaxSpeed_TextBox, 1, 1);
            tableLayoutPanel10.Controls.Add(label12, 0, 0);
            tableLayoutPanel10.Controls.Add(MinSpeed_TextBox, 1, 0);
            tableLayoutPanel10.Controls.Add(label17, 0, 1);
            tableLayoutPanel10.Dock = DockStyle.Fill;
            tableLayoutPanel10.Location = new Point(501, 593);
            tableLayoutPanel10.Name = "tableLayoutPanel10";
            tableLayoutPanel10.RowCount = 2;
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel10.Size = new Size(492, 102);
            tableLayoutPanel10.TabIndex = 24;
            // 
            // MaxSpeed_TextBox
            // 
            MaxSpeed_TextBox.Location = new Point(249, 54);
            MaxSpeed_TextBox.Name = "MaxSpeed_TextBox";
            MaxSpeed_TextBox.Size = new Size(237, 39);
            MaxSpeed_TextBox.TabIndex = 13;
            MaxSpeed_TextBox.Text = "Infinity";
            MaxSpeed_TextBox.TextChanged += MaxSpeed_TextBox_TextChanged;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(3, 0);
            label12.Name = "label12";
            label12.Size = new Size(118, 32);
            label12.TabIndex = 8;
            label12.Text = "Minimum";
            // 
            // MinSpeed_TextBox
            // 
            MinSpeed_TextBox.Location = new Point(249, 3);
            MinSpeed_TextBox.Name = "MinSpeed_TextBox";
            MinSpeed_TextBox.Size = new Size(237, 39);
            MinSpeed_TextBox.TabIndex = 14;
            MinSpeed_TextBox.Text = "0";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(3, 51);
            label17.Name = "label17";
            label17.Size = new Size(121, 32);
            label17.TabIndex = 9;
            label17.Text = "Maximum";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(3, 50);
            label18.Name = "label18";
            label18.Size = new Size(115, 32);
            label18.TabIndex = 25;
            label18.Text = "Mass (kg)";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(3, 158);
            label19.Name = "label19";
            label19.Size = new Size(138, 32);
            label19.TabIndex = 26;
            label19.Text = "Radius (km)";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(3, 266);
            label20.Name = "label20";
            label20.Size = new Size(226, 32);
            label20.TabIndex = 27;
            label20.Text = "Orbital Period (year)";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(3, 374);
            label21.Name = "label21";
            label21.Size = new Size(199, 32);
            label21.TabIndex = 28;
            label21.Text = "Density (g/cm^3)";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(3, 482);
            label22.Name = "label22";
            label22.Size = new Size(178, 32);
            label22.TabIndex = 29;
            label22.Text = "Gravity (m/s^2)";
            // 
            // Speed_Label
            // 
            Speed_Label.AutoSize = true;
            Speed_Label.Location = new Point(3, 590);
            Speed_Label.Name = "Speed_Label";
            Speed_Label.Size = new Size(142, 32);
            Speed_Label.TabIndex = 30;
            Speed_Label.Text = "Speed (m/s)";
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(3, 698);
            label24.Name = "label24";
            label24.Size = new Size(119, 32);
            label24.TabIndex = 31;
            label24.Text = "White List";
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(3, 806);
            label25.Name = "label25";
            label25.Size = new Size(111, 32);
            label25.TabIndex = 32;
            label25.Text = "Black List";
            // 
            // NextPage_Button
            // 
            NextPage_Button.Dock = DockStyle.Fill;
            NextPage_Button.Location = new Point(501, 1025);
            NextPage_Button.Name = "NextPage_Button";
            NextPage_Button.Size = new Size(492, 102);
            NextPage_Button.TabIndex = 33;
            NextPage_Button.Text = "Next Page";
            NextPage_Button.UseVisualStyleBackColor = true;
            NextPage_Button.Click += NextPage_Button_Click;
            // 
            // MapSettingsForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2004, 1136);
            Controls.Add(tableLayoutPanel1);
            Name = "MapSettingsForm";
            Text = "Solar System Mapper - settings";
            WindowState = FormWindowState.Maximized;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel8.PerformLayout();
            tableLayoutPanel9.ResumeLayout(false);
            tableLayoutPanel9.PerformLayout();
            tableLayoutPanel10.ResumeLayout(false);
            tableLayoutPanel10.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel5;
        private ComboBox MapType_ComboBox;
        private DateTimePicker Date_TimePicker;
        private Label label2;
        private Label Coordinates_Label;
        private TextBox Coordinates_TextBox;
        private Label label4;
        private CheckedListBox ObjectTypes_CheckedListBox;
        private TableLayoutPanel tableLayoutPanel4;
        private TextBox MinMass_TextBox;
        private TextBox MaxMass_TextBox;
        private Label label6;
        private Label label7;
        private Label label5;
        private TextBox WhiteList_TextBox;
        private TextBox BlackList_TextBox;
        private TableLayoutPanel tableLayoutPanel6;
        private Label label8;
        private TableLayoutPanel tableLayoutPanel7;
        private TableLayoutPanel tableLayoutPanel8;
        private TableLayoutPanel tableLayoutPanel9;
        private TextBox MaxSpeed_TextBox;
        private TextBox MinSpeed_TextBox;
        private Label label17;
        private Label label12;
        private TextBox MaxRadius_TextBox;
        private TextBox MinRadius_TextBox;
        private Label label13;
        private TextBox MaxOrbitalPeriod_TextBox;
        private TextBox MinOrbitalPeriod_TextBox;
        private Label label14;
        private Label label9;
        private TextBox MaxDensity_TextBox;
        private TextBox MinDensity_TextBox;
        private Label label15;
        private Label label10;
        private TextBox MaxGravity_TextBox;
        private TextBox MinGravity_TextBox;
        private Label label16;
        private Label label11;
        private TableLayoutPanel tableLayoutPanel10;
        private Label label18;
        private Label label19;
        private Label label20;
        private Label label21;
        private Label label22;
        private Label Speed_Label;
        private Label label24;
        private Label label25;
        private Button NextPage_Button;
    }
}