using SolarMapperUI ;
using SolarSystemMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolarMapperUI
{
    /**
     * Stores data baout filters
     */
    internal record TypeSettings<TData>(string TypeName, Func<IEnumerable<IFormBody<TData>>, IEnumerable<IFormBody<TData>>> linqFilter) 
        where TData : IEphemerisData<IEphemerisTableRow>;

    public partial class TypeFilterForm<TData> : Form
        where TData : IEphemerisData<IEphemerisTableRow>
    {
        public string TypeName { get; init; }
        internal TypeSettings<TData>? TypeSettings { get; private set; }
        public TypeFilterForm(string typeName)
        {
            InitializeComponent();
            TypeName = typeName;
            this.TypeName_Label.Text = typeName;
        }



        private Predicate<IFormBody<TData>> _makeRangeFilter()
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
                    else throw new ArgumentException($"{pair.Key} value was entered incorrectly. {value}");
                }
                parsedValues[pair.Key] = value;
            }

            bool allowNaN = !FilterNaN_CheckBox.Checked;

            Predicate<ObjectData> massPredicate = x => (x.Mass_kg > parsedValues["Min Mass"] && x.Mass_kg < parsedValues["Max Mass"]) || (double.IsNaN(x.Mass_kg) && allowNaN);

            Predicate<ObjectData> radiusPredicate = x => (x.Radius_km > parsedValues["Min Radius"] && x.Radius_km < parsedValues["Max Radius"]) || (double.IsNaN(x.Radius_km) && allowNaN);

            Predicate<ObjectData> densityPredicate = x => (x.Density_gpcm3 > parsedValues["Min Density"] && x.Density_gpcm3 < parsedValues["Max Density"]) || (double.IsNaN(x.Density_gpcm3) && allowNaN);

            Predicate<ObjectData> gravityPredicate = x => (x.EquatorialGravity_mps2 > parsedValues["Min Gravity"] && x.EquatorialGravity_mps2 < parsedValues["Max Gravity"]) || (double.IsNaN(x.EquatorialGravity_mps2) && allowNaN);

            Predicate<ObjectData> orbitalPeriodPredicate = x => (x.OrbitalPeriod_y > parsedValues["Min Orbital Period"] && x.OrbitalPeriod_y < parsedValues["Max Orbital Period"]) || (double.IsNaN(x.OrbitalPeriod_y) && allowNaN);


            Predicate<ObjectData> allObjectDataPredicates = x =>
            {


                return (x.Type == this.TypeName.Trim()) && (massPredicate(x) && radiusPredicate(x) && densityPredicate(x) && gravityPredicate(x) && orbitalPeriodPredicate(x)); //filteres out bodies of different type
            }
            ;

            return x => allObjectDataPredicates(x.BodyData.objectData);
        }
        /**
         * Enumerable filtering
         */
        private Func<IEnumerable<IFormBody<TData>>, IEnumerable<IFormBody<TData>>> _makeLINQQuerry() 
        {

            var rangeFilter = _makeRangeFilter();
            bool sortByCategory = TopCategory_ComboBox.SelectedItem != null && AscendingDescending_ComboBox.SelectedItem != null;
            int numberOfTopItems = (int)TopNumber_NumericUpDown.Value;
            string sortDirection = AscendingDescending_ComboBox.Text;
            string sortCategory = TopCategory_ComboBox.Text;

            double averageSpan = 0;
            bool averageSpanParseSuccess = double.TryParse(AvarageSpan_TextBox.Text, out averageSpan);
            string averageCategory = Avarage_Category.Text;
            Debug.WriteLine(averageSpan);
            Debug.WriteLine(averageCategory);

            bool aroundAverage = Avarage_Category.Text != "" && averageSpanParseSuccess;
            bool allowNaN = !FilterNaN_CheckBox.Checked;

            Func<IEnumerable<IFormBody<TData>>, IEnumerable<IFormBody<TData>>> result = x =>
            {
                var collection = from formBody in x //filteres based on predicate from _makeRangeFilter
                                 where rangeFilter(formBody)
                                 select formBody;
                Dictionary<string, Func<IFormBody<TData>, double>> fieldDictionary = new Dictionary<string, Func<IFormBody<TData>, double>>()
                    {
                        {"Mass", x=>x.BodyData.objectData.Mass_kg },
                        {"Radius", x=>x.BodyData.objectData.Radius_km },
                        {"Orbital Period", x=>x.BodyData.objectData.OrbitalPeriod_y },
                        {"Gravity", x=>x.BodyData.objectData.EquatorialGravity_mps2 },
                        {"Density", x=>x.BodyData.objectData.Density_gpcm3 }
                    };


                if (sortByCategory) //sorting and taking top x
                {


                    Func<IEnumerable<IFormBody<TData>>,
                    Func<IFormBody<TData>, double>,
                    IOrderedEnumerable<IFormBody<TData>>> orderMethod = (sortDirection == "Ascending") ? Enumerable.OrderBy : Enumerable.OrderByDescending;

                    collection = orderMethod(collection, fieldDictionary[sortCategory]).Take(numberOfTopItems);
                }

                if (aroundAverage) // taking only objects around average
                {
                    double average = collection.Average(fieldDictionary[averageCategory]);
                    collection = from item in collection
                                 where (fieldDictionary[averageCategory](item) > average - averageSpan && fieldDictionary[averageCategory](item) < average + averageSpan)
                                 || (double.IsNaN(fieldDictionary[averageCategory](item)) && allowNaN)
                                 select item;

                }



                return collection;

            };





            return result;
        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }
        /**
         * Stores collected data
         */
        private void NextPage_Button_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you wish to continue? You can not return to this window afterward.",
                                                    "Are you sure?",
                                                    MessageBoxButtons.OKCancel
                                                    );
            if (result == DialogResult.Cancel) return;
            try
            {
                this.TypeSettings = new TypeSettings<TData>(this.TypeName, this._makeLINQQuerry());
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

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void TopCategory_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Avarage_Category_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
