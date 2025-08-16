using static SolarSystemMapper.EphemerisObserverData;
using static SolarSystemMapper.EphemerisVectorData;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SolarSystemMapper
{

    public record ObjectEntry(string Name, int Code, string Type);

    public interface IEphemerisTableRow
    {
        static TRow stringToRow<TRow>(string data) where TRow : IEphemerisTableRow
        {
            throw new NotImplementedException();
        }
        static double? TryParseNullable(string input)
        {
            if (input == "n.a.") return null;
            if (double.TryParse(input, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double value))
                return value;
            return null;
        }
        static double[]? TryParseTriple(string[] tokens, int start)
        {
            if (tokens.Length <= start + 2) return null;

            bool success =
                double.TryParse(tokens[start], out double a) &
                double.TryParse(tokens[start + 1], out double b) &
                double.TryParse(tokens[start + 2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double c);

            return success ? new[] { a, b, c } : null;
        }
    }

    public record ObjectData(string Name, int Code, double Radius_km = double.NaN, double Density_gpcm3 = double.NaN, double Mass_kg = double.NaN, 
        double RotationPeriod_hr = double.NaN, double EquatorialGravity_mps2 = double.NaN,
        double Temperature_K = double.NaN, double Pressure_bar = double.NaN, double OrbitalPeriod_y = double.NaN, double OrbitalSpeed_kmps = double.NaN, string Type = "");

    public interface IEphemerisData<out TRow> where TRow : IEphemerisTableRow
    {
        public IEnumerable<TRow> ephemerisTable { get; }
        public ObjectData objectData { get; }
    }

    public record EphemerisTableRowObserver(DateTime? date, double[]? RA, double[]? DEC, double? dRA_dt, double? dDEC_dt, double? Azi, double? Elev) : IEphemerisTableRow
    {
        public static EphemerisTableRowObserver stringToRow(string data)
        {
            var tokens = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Datum a čas
            DateTime? date = null;
            if (tokens.Length >= 2 &&
                DateTime.TryParseExact($"{tokens[0]} {tokens[1]}", "yyyy-MMM-dd HH:mm",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                date = parsedDate;

            // RA: hh mm ss.ss
            double[]? RA = IEphemerisTableRow.TryParseTriple(tokens, 2);

            // DEC: ±dd mm ss.s
            double[]? DEC = IEphemerisTableRow.TryParseTriple(tokens, 5);

            // dRA/dt, dDEC/dt, Azi, Elev
            double? dRA_dt = IEphemerisTableRow.TryParseNullable(tokens[8]);
            double? dDEC_dt = IEphemerisTableRow.TryParseNullable(tokens[9]);
            double? azi = IEphemerisTableRow.TryParseNullable(tokens[10]);
            double? elev = IEphemerisTableRow.TryParseNullable(tokens[11]);

            return new EphemerisTableRowObserver(date, RA, DEC, dRA_dt, dDEC_dt, azi, elev);
        }


    }
    public record EphemerisObserverData(IEnumerable<EphemerisTableRowObserver> ephemerisTable, ObjectData objectData) : IEphemerisData<EphemerisTableRowObserver>
    {
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(objectData.ToString());
            builder.AppendLine("*****************************************************************************************");
            builder.AppendLine("SOE");
            foreach (var row in this.ephemerisTable) builder.AppendLine(row.ToString());
            builder.AppendLine("EOE");
            return builder.ToString();
        }
        

    }

    public record EphemerisTableRowVector(DateTime? date, double? X, double? Y, double? Z, double? VX, double? VY, double? VZ, double? LightTime, double? Range, double? RangeRate) : IEphemerisTableRow
    {
        public static EphemerisTableRowVector stringToRow(string data)
        {
            var dateMatch = Regex.Match(data, @"A\.D\.\s+(\d{4}-[A-Za-z]{3}-\d{2}\s+\d{2}:\d{2}:\d{2}\.\d+)");

            string? dateString = (dateMatch.Success) ? dateMatch.Groups[1].Value : null;
            DateTime? parsedDate = (dateString != null) ? DateTime.ParseExact(dateString, "yyyy-MMM-dd HH:mm:ss.ffff", CultureInfo.InvariantCulture) : null;

            // 2. Parsování číselných hodnot
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
    public record EphemerisVectorData(IEnumerable<EphemerisTableRowVector> ephemerisTable, ObjectData objectData) : IEphemerisData<EphemerisTableRowVector>
    {


        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(objectData.ToString());
            builder.AppendLine("*****************************************************************************************");
            builder.AppendLine("SOE");
            builder.AppendLine(this.ephemerisTable.Count().ToString());
            foreach (var row in this.ephemerisTable) builder.AppendLine(row.ToString());
            builder.AppendLine("EOE");
            return builder.ToString();
        }
    }
}
