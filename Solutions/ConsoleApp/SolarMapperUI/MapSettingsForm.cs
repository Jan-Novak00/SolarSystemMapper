using SolarMapperUI;
using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolarMapperUI
{

    /**
     * Stores data about user preferences
     */
    internal record GeneralMapSettings(MapType MapType, DateTime StartDate, List<string> ObjectTypes, List<string> WhiteList, List<string> BlackList, Predicate<ObjectData> GeneralFilter, 
        double minSpeed = 0, double maxSpeed = double.PositiveInfinity, double minDistance = 0, double maxDistance = double.PositiveInfinity, double? latitude = null, double? longitude = null);


    public partial class MapSettingsForm : Form
    {



        internal GeneralMapSettings GeneralMapSettings { get; private set; }
        public MapSettingsForm()
        {
            InitializeComponent();
            MapType_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MapType_ComboBox.SelectedIndex = 1;
        }

        /**
         * Bundles up collected info
         */
        private void NextPage_Button_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you wish to continue? You can not return to this window afterward.",
                                                    "Are you sure?",
                                                    MessageBoxButtons.OKCancel
                                                    );
            if (result == DialogResult.Cancel) return;

            MapType mapType = (MapType_ComboBox.SelectedItem?.ToString() == "Night Sky") ? MapType.NightSky : MapType.SolarSystem;
            DateTime date = Date_TimePicker.Value;
            List<string> objectTypes = ObjectTypes_CheckedListBox.CheckedItems.Cast<string>().ToList();
            List<string> whiteList = WhiteList_TextBox.Text.Split(',').ToList();
            List<string> blackList = BlackList_TextBox.Text.Split(',').ToList();
            double latitude = double.NaN;
            double longitude = double.NaN;
            double minSpeed = double.NaN;
            double maxSpeed = double.NaN;
            double minDistance = double.NaN;
            double maxDistance = double.NaN;
            Predicate<ObjectData> filter = null;
            try
            {
                bool minSpeedSuccess = double.TryParse(MinSpeed_TextBox.Text, out minSpeed);
                bool maxSpeedSuccess = double.TryParse(MaxSpeed_TextBox.Text, out maxSpeed);

                if (!minSpeedSuccess)
                {
                    if (MinSpeed_TextBox.Text.Trim() == "-Infinity") minSpeed = double.NegativeInfinity;
                    else throw new ArgumentException("Min speed value was entered incorrectly.");
                }
                if (!maxSpeedSuccess)
                {
                    if (MaxSpeed_TextBox.Text.Trim() == "Infinity") maxSpeed = double.PositiveInfinity;
                    else throw new ArgumentException("Max speed value was entered incorrectly.");
                }

                bool minDistanceSuccess = double.TryParse(MinDistance_TextBox.Text, out minDistance);
                bool maxDistanceSuccess = double.TryParse(MaxDistance_TextBox.Text, out maxDistance);

                if (!minDistanceSuccess)
                {
                    if (MinDistance_TextBox.Text.Trim() == "-Infinity") minDistance = double.NegativeInfinity;
                    else throw new ArgumentException("Min distance value was entered incorrectly.");
                }
                if (!maxDistanceSuccess)
                {
                    if (MaxDistance_TextBox.Text.Trim() == "Infinity") maxDistance = double.PositiveInfinity;
                    else throw new ArgumentException($"Max distance value was entered incorrectly.");
                }

                if (mapType == MapType.NightSky)
                {
                    string coorString = Coordinates_TextBox.Text;
                    if (coorString == "") throw new ArgumentException("Please, fill in coordinates. Two numbers, separated by comma.");
                    string[] coors = coorString.Split(',');
                    if (coors.Length != 2) throw new ArgumentException("Coordinates must have two components - latitude, followed by longitude.");
                    bool latitudeSuccess = double.TryParse(coors[0], out latitude);
                    bool longitudeSuccess = double.TryParse(coors[1], out longitude);

                    if (!latitudeSuccess) throw new ArgumentException("Latitude is not in correct format.");
                    if (!longitudeSuccess) throw new ArgumentException("Longitude is not in correct format.");

                }
                filter = this._makeFilter();
            }
            catch (ArgumentException ae)
            {
                MessageBox.Show(ae.Message);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }


            this.GeneralMapSettings = new GeneralMapSettings(mapType, date, objectTypes, whiteList, blackList, filter, minSpeed, maxSpeed, minDistance, maxDistance, (double.IsNormal(latitude)) ? latitude : null, (double.IsNormal(longitude)) ? longitude : null);

            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        /**
         * Maeks filter for Where method 
         */
        private Predicate<ObjectData> _makeFilter()
        {


            Dictionary<string, string> rawValues = new Dictionary<string, string>()
            {
                {"Min Mass", MinMass_TextBox.Text},
                {"Max Mass", MaxMass_TextBox.Text},
                {"Min Radius", MinRadius_TextBox.Text},
                {"Max Radius", MaxRadius_TextBox.Text},
                {"Min Orbital Period", MinOrbitalPeriod_TextBox.Text},
                {"Max Orbital Period", MaxOrbitalPeriod_TextBox.Text},
                {"Min Density", MinDensity_TextBox.Text},
                {"Max Density", MaxDensity_TextBox.Text},
                {"Min Gravity", MinGravity_TextBox.Text},
                {"Max Gravity", MaxGravity_TextBox.Text},
                {"Min Speed", MinSpeed_TextBox.Text},
                {"Max Speed", MaxSpeed_TextBox.Text}
            };

            Dictionary<string, double> parsedValues = new Dictionary<string, double>();

            foreach (var pair in rawValues)
            {
                double value;
                bool parseSuccess = double.TryParse(pair.Value.Trim(), out value);

                if (!parseSuccess)
                {
                    if (pair.Value == "Infinity") value = double.PositiveInfinity;
                    else if (pair.Value == "-Infinity") value = double.NegativeInfinity;
                    else throw new ArgumentException($"{pair.Key} value was entered incorrectly.");
                }
                parsedValues[pair.Key] = value;
            }
            Predicate<ObjectData> massPredicate = x => (x.Mass_kg > parsedValues["Min Mass"] && x.Mass_kg < parsedValues["Max Mass"]) || double.IsNaN(x.Mass_kg);

            Predicate<ObjectData> radiusPredicate = x => (x.Radius_km > parsedValues["Min Radius"] && x.Radius_km < parsedValues["Max Radius"]) || double.IsNaN(x.Radius_km);

            Predicate<ObjectData> densityPredicate = x => (x.Density_gpcm3 > parsedValues["Min Density"] && x.Density_gpcm3 < parsedValues["Max Density"]) || double.IsNaN(x.Density_gpcm3);

            Predicate<ObjectData> gravityPredicate = x => (x.EquatorialGravity_mps2 > parsedValues["Min Gravity"] && x.EquatorialGravity_mps2 < parsedValues["Max Gravity"]) || double.IsNaN(x.EquatorialGravity_mps2);

            Predicate<ObjectData> orbitalPeriodPredicate = x => (x.OrbitalPeriod_y > parsedValues["Min Orbital Period"] && x.OrbitalPeriod_y < parsedValues["Max Orbital Period"]) || double.IsNaN(x.OrbitalPeriod_y);



            Predicate<ObjectData> allObjectDataPredicates = x => massPredicate(x) && radiusPredicate(x) && densityPredicate(x) && gravityPredicate(x) && orbitalPeriodPredicate(x);
            return allObjectDataPredicates;
        }

        private void MapType_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MapType_ComboBox.SelectedIndex == 0)
            {
                Coordinates_Label.Enabled = true;
                Coordinates_TextBox.Enabled = true;
                MinSpeed_TextBox.Enabled = false;
                MaxSpeed_TextBox.Enabled = false;
                Speed_Label.Enabled = false;
                Distance_label.Enabled = false;
                MinDistance_TextBox.Enabled = false;
                MaxDistance_TextBox.Enabled = false;
                return;
            }
            Coordinates_Label.Enabled = false;
            Coordinates_TextBox.Enabled = false;
            MinSpeed_TextBox.Enabled = true;
            MaxSpeed_TextBox.Enabled = true;
            Speed_Label.Enabled = true;
            Distance_label.Enabled = true;
            MinDistance_TextBox.Enabled = true;
            MaxDistance_TextBox.Enabled = true;

        }

        private void MaxSpeed_TextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Coordinates_TextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Speed_Label_Click(object sender, EventArgs e)
        {

        }
    }
}
