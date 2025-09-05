using static SolarSystemMapper.EphemerisObserverData;
using static SolarSystemMapper.EphemerisVectorData;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SolarSystemMapper
{
    /**
     * Stores data about ephemeris for fetching.
     * @param Name Name of the ephemeris.
     * @param Code Code of the ephemeris for fetching.
     * @param Type Type of ephemeris.
     */
    public record ObjectEntry(string Name, int Code, string Type);

    /**
     * Represents one row in coordinate table
     */
    public interface IEphemerisTableRow
    {
        public DateTime? date { get; }
        /** Must be implemented in a derived class.
         * @param data Text representation of one row of coordinate table
        */
        static TRow stringToRow<TRow>(string data) where TRow : IEphemerisTableRow
        {
            throw new NotImplementedException();
        }
        /**
         * @return  If input is "n.a." or if the input can not be parsed into double, then returns null, else returns parsed value to double.
         * 
         */
        static double? TryParseNullable(string input)
        {
            if (input == "n.a.") return null;
            if (double.TryParse(input, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double value))
                return value;
            return null;
        }

        [Obsolete]
        static double[]? TryParseTriple(string[] tokens, int start)
        {
            if (tokens.Length <= start + 2) return null;

            bool success =
                double.TryParse(tokens[start], out double a) &
                double.TryParse(tokens[start + 1], out double b) &
                double.TryParse(tokens[start + 2], out double c);
                //double.TryParse(tokens[start + 2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double c);

            return success ? new[] { a, b, c } : null;
        }
    }
    /**
     * Stores properties of given object.
    */
    public record ObjectData(string Name, int Code, string Type, double Radius_km = double.NaN, double Density_gpcm3 = double.NaN, double Mass_kg = double.NaN, 
        double RotationPeriod_hr = double.NaN, double EquatorialGravity_mps2 = double.NaN,
        double Temperature_K = double.NaN, double Pressure_bar = double.NaN, double OrbitalPeriod_y = double.NaN)
    {
        /**
         * @return Human readable information about the object. Excludes inofrmation set to double.NaN
        */
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Object name: {Name.ToUpper()}    Object type: {Type}");
            if (Double.IsNormal(Radius_km)) stringBuilder.AppendLine($"Radius: {Radius_km} km");
            if (Double.IsNormal(Mass_kg)) stringBuilder.AppendLine($"Mass: {Mass_kg} kg");
            if (Double.IsNormal(Density_gpcm3)) stringBuilder.AppendLine($"Density: {Density_gpcm3} g/(cm^3)");
            if (Double.IsNormal(EquatorialGravity_mps2)) stringBuilder.AppendLine($"Gravity: {EquatorialGravity_mps2} m/(s^2)");
            if (Double.IsNormal(Temperature_K)) stringBuilder.AppendLine($"Surface temperature: {Temperature_K - 273.15} kg");
            if (Double.IsNormal(Pressure_bar)) stringBuilder.AppendLine($"Surface pressure: {Pressure_bar*0.98693267} hPa");
            if (Double.IsNormal(RotationPeriod_hr)) stringBuilder.AppendLine($"Rotation period: {RotationPeriod_hr} h");
            if (Double.IsNormal(OrbitalPeriod_y)) stringBuilder.AppendLine($"Orbital period: {OrbitalPeriod_y} year");
            return stringBuilder.ToString();
        }
    }
    /**
     * Stores data about object position and properties
    */
    public interface IEphemerisData<out TRow> where TRow : IEphemerisTableRow
    {
        /**
         * Object position data.
        */
        public IReadOnlyList<TRow> ephemerisTable { get; }
        /**
         * Object properties data
        */
        public ObjectData objectData { get; }
    }
    /**
     * Stores positional information for Night Sky map.
    */
    public record EphemerisTableRowObserver(DateTime? date, double? Azi, double? Elev) : IEphemerisTableRow
    {
        public static EphemerisTableRowObserver stringToRow(string data)
        {
            var tokens = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            
            DateTime? date = null;
            if (tokens.Length >= 2 &&
                DateTime.TryParseExact($"{tokens[0]} {tokens[1]}", "yyyy-MMM-dd HH:mm",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                date = parsedDate;

           
           
            // AZI a ELEV
            double? azi = tokens.Length > 2 ? IEphemerisTableRow.TryParseNullable(tokens[tokens.Length - 2]) : null;
            double? elev = tokens.Length > 1 ? IEphemerisTableRow.TryParseNullable(tokens[tokens.Length - 1]) : null;

            return new EphemerisTableRowObserver(date, azi, elev);
        }


    }
    /**
     * Stores data abou ephemeris for Night Sky map.
    */
    public record EphemerisObserverData(IReadOnlyList<EphemerisTableRowObserver> ephemerisTable, ObjectData objectData) : IEphemerisData<EphemerisTableRowObserver>
    {
        public override string ToString()
        {
            return objectData.ToString();
        }
        

    }
    /**
     * Stores positional data for carthesian coordinate system
    */
    public record EphemerisTableRowVector(DateTime? date = null, double? X = null, double? Y = null, double? Z = null, double? VX = null, double? VY = null, double? VZ = null, double? LightTime = null, double? Range = null, double? RangeRate = null) : IEphemerisTableRow
    {
        public static EphemerisTableRowVector stringToRow(string data)
        {
            var dateMatch = Regex.Match(data, @"A\.D\.\s+(\d{4}-[A-Za-z]{3}-\d{2}\s+\d{2}:\d{2}:\d{2}\.\d+)");

            string? dateString = (dateMatch.Success) ? dateMatch.Groups[1].Value : null;
            DateTime? parsedDate = (dateString != null) ? DateTime.ParseExact(dateString, "yyyy-MMM-dd HH:mm:ss.ffff", CultureInfo.InvariantCulture) : null;

            double? x = GetDouble(data, @"X\s*=\s*([-+Ee\d\.]+)");
            double? y = GetDouble(data, @"Y\s*=\s*([-+Ee\d\.]+)");
            double? z = GetDouble(data, @"Z\s*=\s*([-+Ee\d\.]+)");
            double? vx = GetDouble(data, @"VX\s*=\s*([-+Ee\d\.]+)");
            double? vy = GetDouble(data, @"VY\s*=\s*([-+Ee\d\.]+)");
            double? vz = GetDouble(data, @"VZ\s*=\s*([-+Ee\d\.]+)");
            double? lt = GetDouble(data, @"LT\s*=\s*([-+Ee\d\.]+)");
            double? rg = GetDouble(data, @"RG\s*=\s*([-+Ee\d\.]+)");
            double? rr = GetDouble(data, @"RR\s*=\s*([-+Ee\d\.]+)");

            return new EphemerisTableRowVector(parsedDate, x, y, z, vx, vy, vz, lt, rg, rr);
        }

        private static double? GetDouble(string input, string pattern)
        {
            var match = Regex.Match(input, pattern);
            if (!match.Success) return null;
            return double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        }


    }
    /**
     * Stores data about object for Solar System map or Moon view
    */
    public record EphemerisVectorData(IReadOnlyList<EphemerisTableRowVector> ephemerisTable, ObjectData objectData) : IEphemerisData<EphemerisTableRowVector>
    {


        public override string ToString()
        {
            return objectData.ToString();
        }
    }
}
