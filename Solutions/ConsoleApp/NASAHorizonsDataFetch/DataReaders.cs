using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using static SolarSystemMapper.EphemerisObserverData;
using static SolarSystemMapper.EphemerisVectorData;
using System.Text;
using System.Diagnostics;


namespace SolarSystemMapper
{

    /**
     * Interface for data parsers.
    */
    public interface IHorizonsResponseReader<out TData> where TData : IEphemerisData<IEphemerisTableRow>
    {
        TData Read();
        
    }
    /**
     * Extracts data about ephemeris from data sent from the NASA Horizon API. Only extracts object property data in protected method.
    */
    public abstract class ObjectReader
    {
        protected string? _data { get; set; }
        protected string _objectName { get; set; } = "";
        protected string _objectType { get; set; } = "";
        protected int _objectCode { get; set; }
        protected const string _StartOfTableRegex = @".*\$\$SOE.*";
        protected const string _EndOfTableRegex = @".*\$\$EOE.*";



        /**
         * @return Object property infrmation.
       */

        protected ObjectData createObjectInfo()
        {
            double? radius = null; 
            double? density = null;
            double? mass = null;
            double? rotationPeriod = null;
            double? gravity = null;
            double? temperature = null;
            double? pressure = null;
            double? orbitalPeriod = null;

            var dataReader = new StringReader(this._data!);
            while (true)
            {
                var line = dataReader.ReadLine();
                if (line == null) break;

                { 
                    if (radius == null)
                    {
                        radius = _findRadius(line);
                        if (radius != null) continue;
                    }
                    if (mass == null)
                    {
                        mass = _findMass(line);
                        if (mass != null) continue;
                    }
                    if (density == null)
                    {
                        density = _findDenisity(line);
                        if (density != null) continue;
                    }
                    if (rotationPeriod == null)
                    {
                        rotationPeriod = _findRotationPeriod(line);
                        if (rotationPeriod != null) continue;
                    }
                    if (gravity == null)
                    {
                        gravity = _findGravity(line);
                        if (gravity != null) continue;
                    }
                    if (temperature == null)
                    {
                        temperature = _findTemperature(line);
                        if (temperature != null) continue;
                    }
                    if (pressure == null)
                    {
                        pressure = _findPressure(line);
                        if (pressure != null) continue;
                    }
                    if (orbitalPeriod == null)
                    {
                        orbitalPeriod = _findOrbitalPeriod(line);
                        if (orbitalPeriod != null) continue;
                    }
                    
                }

            }
            if (rotationPeriod == -1) rotationPeriod = orbitalPeriod;

            return new ObjectData(
                this._objectName, 
                this._objectCode, 
                this._objectType,
                radius ?? double.NaN,
                density ?? double.NaN,
                mass ?? double.NaN,
                rotationPeriod ?? double.NaN,
                gravity ?? double.NaN,
                temperature ?? double.NaN,
                pressure ?? double.NaN,
                orbitalPeriod ?? double.NaN
                );


        }

        /**
         * @param line Line in data string.
         * @return If successful, returns orbital period of the object
        */
        public static double? _findOrbitalPeriod(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;


            
            var pattern = @"(?i)(?:Sidereal\s+orbit\s+period|Sidereal\s+orb\.?\s*per\.?|Mean\s+sidereal\s+orb\s*per|Orbital\s+period)[^0-9\-+]*([-+]?\d*\.?\d+)\s*(y|d)?";


            var matches = System.Text.RegularExpressions.Regex.Matches(line, pattern);

            double? periodInYears = null;

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                if (match.Success)
                {
                    double value = double.Parse(match.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
                    string unit = match.Groups[2].Value?.ToLower() ?? "";

                    if (unit == "y" || string.IsNullOrEmpty(unit)) //if inuput is in years
                    {
                        
                        periodInYears = value;
                        break; 
                    }

                    if (unit == "d") // if input is in days
                    {
                        
                        if (!periodInYears.HasValue)
                            periodInYears = value / 365.25;
                    }
                }
            }

            return periodInYears;
        }

        /**
         * @param line Line in data string.
         * @return If successful, returns surface pressure of the object
        */
        public static double? _findPressure(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;


            
            var pattern = @"(?i)(?:Atmos\.?\s*pressure|Atm\.?\s*pressure|Atmos\.?\s*press)\s*(?:\(?bar\)?)?\s*=?\s*([-+]?\d*\.?\d+)";

            var match = System.Text.RegularExpressions.Regex.Match(line, pattern);
            if (match.Success)
            {
                return double.Parse(match.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
            }

            return null;
        }

        /**
         * @param line Line in data string.
         * @return If successful, returns surface temperature of the object
        */
        public static double? _findTemperature(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;


            
            var pattern = @"(?i)(?:Mean\s+Temperature|Mean\s+surface\s+temp|Effective\s+temp|Atmos\.?\s*temp).*?=\s*([-+]?\d*\.?\d+)";

            var match = System.Text.RegularExpressions.Regex.Match(line, pattern);
            if (match.Success)
            {
                return double.Parse(match.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
            }

            return null;
        }

        /**
         * @param line Line in data string.
         * @return If successful, returns gravity of the object
        */
        public static double? _findGravity(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;


            
            var gravityRegexes = new[] // many different patterns
            {
                @"Equ\.?\s*gravity\s*m\s*/\s*s\^2\s*=\s*([-+]?\d*\.?\d+)",
                @"g_e\s*,?\s*m\s*/\s*s\^2\s*\(equatorial\)\s*=\s*([-+]?\d*\.?\d+)",
                @"Equ\.?\s*grav\s*,?\s*ge\s*\(m\s*/\s*s\^2\)\s*=\s*([-+]?\d*\.?\d+)",
                @"Surface\s+gravity\s*=\s*([-+]?\d*\.?\d+)\s*m\s*/\s*s\^2",
                @".*Equ\.?\s*gravity\s*m\s*/\s*s\s*\^\s*2\s*=\s*([-+]?\d*\.?\d+)"
            };

            foreach (var pattern in gravityRegexes)
            {
                var match = System.Text.RegularExpressions.Regex.Match(line, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (match.Success)
                    return double.Parse(match.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
            }

            // if gravity not present, we calculate the gravity form density
            
            var radiusDensityMatch = new System.Text.RegularExpressions.Regex(
                @"Mean\s+radius\s*\(km\)\s*=\s*([-+]?\d*\.?\d+).+Density\s*\(g\s*cm\^-3\)\s*=\s*([-+]?\d*\.?\d+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            ).Match(line);

            if (radiusDensityMatch.Success)
            {
                double radiusKm = double.Parse(radiusDensityMatch.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
                double densityGcm3 = double.Parse(radiusDensityMatch.Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);

                double radiusM = radiusKm * 1000.0;
                double density = densityGcm3 * 1000.0; // g/cm3 -> kg/m3

                double volume = (4.0 / 3.0) * Math.PI * Math.Pow(radiusM, 3);
                double mass = density * volume;

                const double G = 6.67430e-11; // m2 kg-1 s-1
                double g = G * mass / Math.Pow(radiusM, 2);

                return g;
            }

            return null;
        }

        /**
         * @param line Line in data string.
         * @return If successful, returns rotational period of the object. Returns -1 if the rotational period is marked as synchronous with the orbital period
        */
        public static double? _findRotationPeriod(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

  

            // synchronous orbit - return -1
            if (line.IndexOf("Synchronous", StringComparison.OrdinalIgnoreCase) >= 0)
                return -1;

            // h m s format
            var hmsRegex = new System.Text.RegularExpressions.Regex(
                @"(?:Adopted\s+sid\.?\s+rot\.?\s*per\.?|Sidereal\s+rot(?:\.|ation)?\s+period|Sid\.?\s+rot\.?\s+period\s*\(III\)|Mean\s+sidereal\s+day,?\s*hr|Rotational\s+period)\s*=\s*([\d\.]+)h\s*([\d\.]+)m\s*([\d\.]+)s",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var hmsMatch = hmsRegex.Match(line);
            if (hmsMatch.Success)
            {
                double h = double.Parse(hmsMatch.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
                double m = double.Parse(hmsMatch.Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);
                double s = double.Parse(hmsMatch.Groups[3].Value, System.Globalization.CultureInfo.InvariantCulture);
                return (h + m / 60.0 + s / 3600.0) / 24.0;
            }

            // unit after or before the number
            var numberRegex = new System.Text.RegularExpressions.Regex(
                @"(?:Adopted\s+sid\.?\s+rot\.?\s*per\.?|Sidereal\s+rot(?:\.|ation)?\s+period|Sid\.?\s+rot\.?\s+period\s*\(III\)|Mean\s+sidereal\s+day,?\s*(?:hr)?|Rotational\s+period)\s*=?\s*([-+]?\d*\.?\d+)\s*(d|h|hr)?",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var numberMatch = numberRegex.Match(line);
            if (numberMatch.Success)
            {
                double value = double.Parse(numberMatch.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
                string unit = "";

                // behind value
                if (numberMatch.Groups[2].Success)
                    unit = numberMatch.Groups[2].Value.ToLower();
                else
                {
                    // in front of value
                    var namePart = numberMatch.Groups[0].Value;
                    if (namePart.IndexOf("hr", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        namePart.IndexOf("h", StringComparison.OrdinalIgnoreCase) >= 0)
                        unit = "h";
                }

                if (unit.StartsWith("h"))
                    value /= 24.0; 

                return value;
            }

            return null;
        }

        /**
         * @param line Line in data string.
         * @return If successful, returns density of the object
        */
        public static double? _findDenisity(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

 
            var densityRegex = new System.Text.RegularExpressions.Regex(
                @"Density\s*(?:\(|,)?\s*g\s*[/]?\s*cm\s*\^?-?\s*3\s*(?:\))?\s*=\s*([-+]?\d*\.?\d+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            var match = densityRegex.Match(line);
            if (match.Success)
            {
                string value = match.Groups[1].Success ? match.Groups[1].Value : match.Groups[2].Value;
                if (!string.IsNullOrEmpty(value))
                    return double.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            }

            return null;
        }
        /**
         * @param line Line in data string.
         * @return If successful, returns mass of the object
        */
        public static double? _findMass(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            

            // Mass x10^XX 
            var massRegex = new System.Text.RegularExpressions.Regex(
                @"Mass\s*(?:x\s*10\^(\d+)|,\s*10\^(\d+)?)?\s*\(?kg\)?\s*=?\s*~?\s*([-+]?\d*\.?\d+)(?:\+\-\d*\.?\d+)?",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var massMatch = massRegex.Match(line);
            if (massMatch.Success)
            {
                double mantissa = double.Parse(massMatch.Groups[3].Value, System.Globalization.CultureInfo.InvariantCulture);
                int exponent = 0;
                if (!string.IsNullOrEmpty(massMatch.Groups[1].Value))
                    exponent = int.Parse(massMatch.Groups[1].Value);
                else if (!string.IsNullOrEmpty(massMatch.Groups[2].Value))
                    exponent = int.Parse(massMatch.Groups[2].Value);

                return mantissa * Math.Pow(10, exponent);
            }

            // only mass and unit, no exponent
            var simpleMassRegex = new System.Text.RegularExpressions.Regex(
                @"mass\s*:\s*([-+]?\d*\.?\d+)\s*kg",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var simpleMatch = simpleMassRegex.Match(line);
            if (simpleMatch.Success)
            {
                return double.Parse(simpleMatch.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
            }

            // mass derived from density
            var radiusRegex = new System.Text.RegularExpressions.Regex(
                @"Mean radius \(km\)\s*=\s*([-+]?\d*\.?\d+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var densityRegex = new System.Text.RegularExpressions.Regex(
                @"Density\s*\(g\s*cm\^-?3\)\s*=\s*([-+]?\d*\.?\d+)|Density\s*\(g/cm\^3\)\s*=\s*([-+]?\d*\.?\d+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            var rMatch = radiusRegex.Match(line);
            var dMatch = densityRegex.Match(line);
            if (rMatch.Success && dMatch.Success)
            {
                double radiusKm = double.Parse(rMatch.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
                double density = double.Parse(dMatch.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture); // g/cm3
                double volumeM3 = 4.0 / 3.0 * Math.PI * Math.Pow(radiusKm * 1000, 3); // m3
                double massKg = density * 1000 * volumeM3; // density g/cm3 -> kg/m3
                return massKg;
            }

            return null;
        }
        /**
         * @param line Line in data string.
         * @return If successful, returns radius of the object
        */
        public static double? _findRadius(string line)
        {
            if (string.IsNullOrEmpty(line))
                return null;

           
            var pattern = @"(?i)(Vol\.?\s*mean\s*radius|Vol\.?Mean\s*Radius|Mean\s*radius)\s*\(?.*?\)?\s*=\s*([+-]?\d+(\.\d+)?)(\s*[+-]\s*\d+(\.\d+)?)?";
            var match = Regex.Match(line, pattern);

            if (match.Success && double.TryParse(match.Groups[2].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double radius))
            {
                return radius;
            }

            return null;
        }


    }

    /**
     * Parses data from NASA Horizons API into EphemerisObserverData.
    */
    public class HorizonsObserverResponseReader : ObjectReader, IHorizonsResponseReader<EphemerisObserverData>
    {
        /**
         * @param data Data form NASA Horizons API
         * @param objectName Name of the object
         * @param objectCode Code of the object
        */
        public HorizonsObserverResponseReader(string data, string objectName, string objectType, int objectCode)
        {
            this._data = data ?? throw new ArgumentNullException(nameof(data));
            this._objectName = objectName;
            this._objectCode = objectCode;
            this._objectType = objectType;
        }

        public EphemerisObserverData Read()
        {

            ObjectData objectData = this.createObjectInfo();
            List<EphemerisTableRowObserver> ephemerisTableObservers = new List<EphemerisTableRowObserver>();

            var dataReader = new StringReader(this._data!);
            bool isReadingTable = false;
            Regex regexStartOfTable = new Regex(_StartOfTableRegex, RegexOptions.Compiled);
            Regex regexEndOfTable = new Regex(_EndOfTableRegex, RegexOptions.Compiled);



            while (true)
            {
                var line = dataReader.ReadLine();
                if (line == null) break;

                if (!isReadingTable)
                {
                    var match = regexStartOfTable.IsMatch(line);
                    isReadingTable = match;
                }
                else
                {
                    
                    if (regexEndOfTable.IsMatch(line)) break;
                    else
                    {
                        var newRow = EphemerisTableRowObserver.stringToRow(line);
                        ephemerisTableObservers.Add(newRow!);
                    }

                }

            }
            
            return new EphemerisObserverData(ephemerisTableObservers, objectData);

            

        }
    }
    /**
     * Parses data from NASA Horizons API into EphemerisVectorData.
    */
    public class HorizonsVectorResponseReader : ObjectReader, IHorizonsResponseReader<EphemerisVectorData>
    {

        /**
         * @param data Data form NASA Horizons API
         * @param objectName Name of the object
         * @param objectCode Code of the object
        */
        public HorizonsVectorResponseReader(string data, string objectName, string objectType, int objectCode)
        {
            this._data = data;
            this._objectName = objectName;
            this._objectCode = objectCode;
            this._objectType = objectType;
        }

        public EphemerisVectorData Read()
        {
            ObjectData objectData = this.createObjectInfo();
            List<EphemerisTableRowVector> ephemerisTableVector = new List<EphemerisTableRowVector>();

            var dataReader = new StringReader(this._data!);
            bool isReadingTable = false;

            StringBuilder textBuffer = new StringBuilder();
            string? line;
            Regex regexStartOfTable = new Regex(_StartOfTableRegex, RegexOptions.Compiled);
            Regex regexEndOfTable = new Regex(_EndOfTableRegex, RegexOptions.Compiled);
            Regex dateLineRegex = new Regex(@"\s*A\.D\.\s+\d{4}-[A-Za-z]{3}-\d{2}", RegexOptions.Compiled);

            //each row in the table concists of several lines
            while ((line = dataReader.ReadLine()) != null)
            {
                if (!isReadingTable)
                {
                    if (regexStartOfTable.IsMatch(line))
                        isReadingTable = true;
                }
                else
                {
                    if (regexEndOfTable.IsMatch(line))
                    {
                        if (textBuffer.Length > 0)
                        {
                            ephemerisTableVector.Add(EphemerisTableRowVector.stringToRow(textBuffer.ToString()));
                            textBuffer.Clear();
                        }
                        break;
                    }
                    if (dateLineRegex.IsMatch(line))
                    {
                        if (textBuffer.Length > 0)
                        {
                            ephemerisTableVector.Add(EphemerisTableRowVector.stringToRow(textBuffer.ToString()));
                            textBuffer.Clear();
                        }
                    }

                    textBuffer.AppendLine(line);
                }
            }

            
            if (textBuffer.Length > 0)
            {
                ephemerisTableVector.Add(EphemerisTableRowVector.stringToRow(textBuffer.ToString()));
            }
           

            return new EphemerisVectorData(ephemerisTableVector, objectData);
        }


    }


}
