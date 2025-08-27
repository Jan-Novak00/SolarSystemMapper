using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using static SolarSystemMapper.EphemerisObserverData;
using static SolarSystemMapper.EphemerisVectorData;
using System.Text;


namespace SolarSystemMapper
{


    public interface IHorizonsResponseReader<out TData> where TData : IEphemerisData<IEphemerisTableRow>
    {
        TData Read();
        
    }

    public abstract class ObjectReader
    {
        protected string? _data { get; set; }
        protected string _objectName { get; set; } = "";
        protected string _objectType { get; set; } = "";
        protected int _objectCode { get; set; }
        protected const string _StartOfTableRegex = @".*\$\$SOE.*";
        protected const string _EndOfTableRegex = @".*\$\$EOE.*";





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


        public static double? _findOrbitalPeriod(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            var culture = System.Globalization.CultureInfo.InvariantCulture;

            
            var pattern = @"(?i)(?:Sidereal\s+orbit\s+period|Sidereal\s+orb\.?\s*per\.?|Mean\s+sidereal\s+orb\s*per|Orbital\s+period)[^0-9\-+]*([-+]?\d*\.?\d+)\s*(y|d)?";


            var matches = System.Text.RegularExpressions.Regex.Matches(line, pattern);

            double? periodInYears = null;

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                if (match.Success)
                {
                    double value = double.Parse(match.Groups[1].Value, culture);
                    string unit = match.Groups[2].Value?.ToLower() ?? "";

                    if (unit == "y" || string.IsNullOrEmpty(unit))
                    {
                        // preferujeme roky
                        periodInYears = value;
                        break; // rovnou končíme, našli jsme vhodnou hodnotu
                    }

                    if (unit == "d")
                    {
                        // převedeme dny na roky, ale jen pokud ještě nemáme hodnotu v rocích
                        if (!periodInYears.HasValue)
                            periodInYears = value / 365.25;
                    }
                }
            }

            return periodInYears;
        }


        public static double? _findPressure(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            var culture = System.Globalization.CultureInfo.InvariantCulture;

            // Regex pro různé varianty zápisu tlaku
            var pattern = @"(?i)(?:Atmos\.?\s*pressure|Atm\.?\s*pressure|Atmos\.?\s*press)\s*(?:\(?bar\)?)?\s*=?\s*([-+]?\d*\.?\d+)";

            var match = System.Text.RegularExpressions.Regex.Match(line, pattern);
            if (match.Success)
            {
                return double.Parse(match.Groups[1].Value, culture);
            }

            return null;
        }

        public static double? _findTemperature(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            var culture = System.Globalization.CultureInfo.InvariantCulture;

            // Regex zachytí různé varianty názvu a jednotky K
            var pattern = @"(?i)(?:Mean\s+Temperature|Mean\s+surface\s+temp|Effective\s+temp|Atmos\.?\s*temp).*?=\s*([-+]?\d*\.?\d+)";

            var match = System.Text.RegularExpressions.Regex.Match(line, pattern);
            if (match.Success)
            {
                return double.Parse(match.Groups[1].Value, culture);
            }

            return null;
        }

        public static double? _findGravity(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            var culture = System.Globalization.CultureInfo.InvariantCulture;

            // 1) Přímá hodnota gravitačního zrychlení v m/s^2
            var gravityRegexes = new[]
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
                    return double.Parse(match.Groups[1].Value, culture);
            }

            // 2) Krajní případ: radius + density na stejném řádku
            var radiusDensityMatch = new System.Text.RegularExpressions.Regex(
                @"Mean\s+radius\s*\(km\)\s*=\s*([-+]?\d*\.?\d+).+Density\s*\(g\s*cm\^-3\)\s*=\s*([-+]?\d*\.?\d+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            ).Match(line);

            if (radiusDensityMatch.Success)
            {
                double radiusKm = double.Parse(radiusDensityMatch.Groups[1].Value, culture);
                double densityGcm3 = double.Parse(radiusDensityMatch.Groups[2].Value, culture);

                double radiusM = radiusKm * 1000.0;
                double density = densityGcm3 * 1000.0; // g/cm³ → kg/m³

                double volume = (4.0 / 3.0) * Math.PI * Math.Pow(radiusM, 3);
                double mass = density * volume;

                const double G = 6.67430e-11; // m³ kg⁻¹ s⁻²
                double g = G * mass / Math.Pow(radiusM, 2);

                return g;
            }

            return null;
        }


        public static double? _findRotationPeriod(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            var culture = System.Globalization.CultureInfo.InvariantCulture;

            // 1) Synchronous
            if (line.IndexOf("Synchronous", StringComparison.OrdinalIgnoreCase) >= 0)
                return -1;

            // 2) Formát h m s
            var hmsRegex = new System.Text.RegularExpressions.Regex(
                @"(?:Adopted\s+sid\.?\s+rot\.?\s*per\.?|Sidereal\s+rot(?:\.|ation)?\s+period|Sid\.?\s+rot\.?\s+period\s*\(III\)|Mean\s+sidereal\s+day,?\s*hr|Rotational\s+period)\s*=\s*([\d\.]+)h\s*([\d\.]+)m\s*([\d\.]+)s",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var hmsMatch = hmsRegex.Match(line);
            if (hmsMatch.Success)
            {
                double h = double.Parse(hmsMatch.Groups[1].Value, culture);
                double m = double.Parse(hmsMatch.Groups[2].Value, culture);
                double s = double.Parse(hmsMatch.Groups[3].Value, culture);
                return (h + m / 60.0 + s / 3600.0) / 24.0;
            }

            // 3) Číslo s jednotkou nebo jednotka v názvu
            var numberRegex = new System.Text.RegularExpressions.Regex(
                @"(?:Adopted\s+sid\.?\s+rot\.?\s*per\.?|Sidereal\s+rot(?:\.|ation)?\s+period|Sid\.?\s+rot\.?\s+period\s*\(III\)|Mean\s+sidereal\s+day,?\s*(?:hr)?|Rotational\s+period)\s*=?\s*([-+]?\d*\.?\d+)\s*(d|h|hr)?",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var numberMatch = numberRegex.Match(line);
            if (numberMatch.Success)
            {
                double value = double.Parse(numberMatch.Groups[1].Value, culture);
                string unit = "";

                // 3a) jednotka explicitně za číslem
                if (numberMatch.Groups[2].Success)
                    unit = numberMatch.Groups[2].Value.ToLower();
                else
                {
                    // 3b) jednotka v názvu, ale pouze pokud za číslem není žádná jednotka
                    var namePart = numberMatch.Groups[0].Value;
                    if (namePart.IndexOf("hr", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        namePart.IndexOf("h", StringComparison.OrdinalIgnoreCase) >= 0)
                        unit = "h";
                }

                if (unit.StartsWith("h"))
                    value /= 24.0; // převod hodin na dny

                return value;
            }

            return null;
        }

        public static double? _findDenisity(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            var culture = System.Globalization.CultureInfo.InvariantCulture;

            // Regex zachytí různé varianty zápisu:
            // Density (g/cm^3), Density(g / cm ^ 3), Mean density, g/cm^3, Density (g cm^-3)
            var densityRegex = new System.Text.RegularExpressions.Regex(
                @"Density\s*(?:\(|,)?\s*g\s*[/]?\s*cm\s*\^?-?\s*3\s*(?:\))?\s*=\s*([-+]?\d*\.?\d+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            var match = densityRegex.Match(line);
            if (match.Success)
            {
                string value = match.Groups[1].Success ? match.Groups[1].Value : match.Groups[2].Value;
                if (!string.IsNullOrEmpty(value))
                    return double.Parse(value, culture);
            }

            return null;
        }
        public static double? _findMass(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            var culture = System.Globalization.CultureInfo.InvariantCulture;

            // 1) Přímá hmotnost: Mass x10^XX (kg)
            var massRegex = new System.Text.RegularExpressions.Regex(
                @"Mass\s*(?:x\s*10\^(\d+)|,\s*10\^(\d+)?)?\s*\(?kg\)?\s*=?\s*~?\s*([-+]?\d*\.?\d+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var massMatch = massRegex.Match(text);
            if (massMatch.Success)
            {
                double mantissa = double.Parse(massMatch.Groups[3].Value, culture);
                int exponent = 0;
                if (!string.IsNullOrEmpty(massMatch.Groups[1].Value))
                    exponent = int.Parse(massMatch.Groups[1].Value);
                else if (!string.IsNullOrEmpty(massMatch.Groups[2].Value))
                    exponent = int.Parse(massMatch.Groups[2].Value);

                return mantissa * Math.Pow(10, exponent);
            }

            // 2) Přímá hmotnost v základních jednotkách (např. "mass : 705 kg")
            var simpleMassRegex = new System.Text.RegularExpressions.Regex(
                @"mass\s*:\s*([-+]?\d*\.?\d+)\s*kg",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var simpleMatch = simpleMassRegex.Match(text);
            if (simpleMatch.Success)
            {
                return double.Parse(simpleMatch.Groups[1].Value, culture);
            }

            // 3) Odvozená hmotnost: pokud máme radius a density na stejném řádku
            var radiusRegex = new System.Text.RegularExpressions.Regex(
                @"Mean radius \(km\)\s*=\s*([-+]?\d*\.?\d+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var densityRegex = new System.Text.RegularExpressions.Regex(
                @"Density\s*\(g\s*cm\^-?3\)\s*=\s*([-+]?\d*\.?\d+)|Density\s*\(g/cm\^3\)\s*=\s*([-+]?\d*\.?\d+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            var rMatch = radiusRegex.Match(text);
            var dMatch = densityRegex.Match(text);
            if (rMatch.Success && dMatch.Success)
            {
                double radiusKm = double.Parse(rMatch.Groups[1].Value, culture);
                double density = double.Parse(dMatch.Groups[1].Value, culture); // g/cm3
                double volumeM3 = 4.0 / 3.0 * Math.PI * Math.Pow(radiusKm * 1000, 3); // m3
                double massKg = density * 1000 * volumeM3; // density g/cm3 -> kg/m3
                return massKg;
            }

            return null;
        }

        public static double? _findRadius(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            // Regex: hledá varianty radius, pak '=' a číslo, ignoruje mezery, plus/minus
            var pattern = @"(?i)(Vol\.?\s*mean\s*radius|Vol\.?Mean\s*Radius|Mean\s*radius)\s*\(?.*?\)?\s*=\s*([+-]?\d+(\.\d+)?)(\s*[+-]\s*\d+(\.\d+)?)?";
            var match = Regex.Match(text, pattern);

            if (match.Success && double.TryParse(match.Groups[2].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double radius))
            {
                return radius;
            }

            return null; // nenalezeno
        }






        protected readonly Dictionary<string, string> _RegexDictionary  = new Dictionary<string, string>()
        {
            { "radius",          @"Vol\. mean radius \(km\)\s*=\s*([-+]?\d*\.?\d+(?:[Ee][-+]?\d+)?)" },
            { "density",         @"Density \(g/cm\^3\)\s*=\s*([-+]?\d*\.?\d+(?:[Ee][-+]?\d+)?)" },
            { "mass",            @"Mass x10\^23 \(kg\)\s*=\s*([-+]?\d*\.?\d+(?:[Ee][-+]?\d+)?)" },
            { "rotationPeriod",  @"Sidereal rot\. period\s*=\s*([-+]?\d*\.?\d+(?:[Ee][-+]?\d+)?)" },
            { "gravity",         @"Equ\. gravity\s*m/s\^2\s*=\s*([-+]?\d*\.?\d+(?:[Ee][-+]?\d+)?)" },
            { "temperature",     @"Mean temperature \(K\)\s*=\s*([-+]?\d*\.?\d+(?:[Ee][-+]?\d+)?)" },
            { "pressure",        @"Atmos\. pressure \(bar\)\s*=\s*([-+]?\d*\.?\d+(?:[Ee][-+]?\d+)?)" },
            { "orbitalPeriod",   @"Mean sidereal orb per\s*=\s*([-+]?\d*\.?\d+(?:[Ee][-+]?\d+)?)" }
        };

        [Obsolete]
        protected ObjectData readObjectinfo()
        {

            var dataReader = new StringReader(this._data!);
            var valueDict = new Dictionary<string, double>();
            foreach (var key in this._RegexDictionary.Keys)
            {
                valueDict.Add(key, double.NaN);
            }


            while (true)
            {
                var line = dataReader.ReadLine();
                if (line == null) break;

                int numberOfMatches = 0;

                foreach (var keyRegex in this._RegexDictionary)
                {
                    var key = keyRegex.Key;
                    var regex = keyRegex.Value;

                    var match = Regex.Match(line, regex);
                    if (match.Success)
                    {
                        var value = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                        valueDict[key] = value;
                        numberOfMatches++;

                        if (numberOfMatches == 2) break;


                    }
                    else continue;

                }

            }
            return new ObjectData(this._objectName,
                                  this._objectCode,
                                  this._objectType,
                                  valueDict["radius"],
                                  valueDict["density"],
                                  valueDict["mass"],
                                  valueDict["rotationPeriod"],
                                  valueDict["gravity"],
                                  valueDict["temperature"],
                                  valueDict["pressure"],
                                  valueDict["orbitalPeriod"]
                                 );



        }
    }

    public class HorizonsObserverResponseReader : ObjectReader, IHorizonsResponseReader<EphemerisObserverData>
    {

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



            while (true)
            {
                var line = dataReader.ReadLine();
                if (line == null) break;

                if (!isReadingTable)
                {
                    var match = Regex.Match(line, _StartOfTableRegex);
                    isReadingTable = match.Success;
                }
                else
                {
                    var match = Regex.Match(line, _EndOfTableRegex);
                    if (match.Success) break;
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

    public class HorizonsVectorResponseReader : ObjectReader, IHorizonsResponseReader<EphemerisVectorData>
    {
        

        public HorizonsVectorResponseReader(string data, string objectName, string objectType, int objectCode)
        {
            this._data = data;
            this._objectName = objectName;
            this._objectCode = objectCode;
            this._objectType = objectType;
        }

        public EphemerisVectorData Read() //prozkoumat
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

                    // Pokud řádek začíná novým datovým blokem (např. "A.D. 2000-Jul-08"), znamená to začátek nového záznamu
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

            // Poslední blok po ukončení cyklu
            if (textBuffer.Length > 0)
            {
                ephemerisTableVector.Add(EphemerisTableRowVector.stringToRow(textBuffer.ToString()));
            }

            return new EphemerisVectorData(ephemerisTableVector, objectData);
        }


    }


}
