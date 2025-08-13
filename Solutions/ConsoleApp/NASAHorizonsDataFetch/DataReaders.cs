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
        protected int _objectCode { get; set; }
        protected const string _StartOfTableRegex = @".*\$\$SOE.*";
        protected const string _EndOfTableRegex = @".*\$\$EOE.*";

        protected readonly Dictionary<string, string> _RegexDictionary = new Dictionary<string, string>()
        {
            { "radius", @"Vol\. mean radius \(km\)\s*=\s*([-+]?\d*\.?\d+).*"},
            {"density", @".*Density \(g/cm\^3\)\s*=\s*([-+]?\d*\.?\d+).*" },
            {"mass", @".*Mass x10\^23 \(kg\)\s*=\s*([-+]?\d*\.?\d+).*" },
            {"rotationPeriod", @".*Sidereal rot\. period\s*=\s*([-+]?\d*\.?\d+).*" },
            {"gravity",@".*Equ\. gravity\s*m/s\^2\s*=\s*([-+]?\d*\.?\d+).*" },
            {"temperature", @".*Mean temperature \(K\)\s*=\s*([-+]?\d*\.?\d+).*" },
            {"pressure", @".*Atmos\. pressure \(bar\)\s*=\s*([-+]?\d*\.?\d+).*" },
            {"orbitalPeriod", @".*Mean sidereal orb per\s*=\s*([-+]?\d*\.?\d+).*"},
            {"orbitalSpeed",@".*Orbital speed,\s*km/s\s*=\s*([-+]?\d*\.?\d+).*" }
        };
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
                                  valueDict["radius"],
                                  valueDict["density"],
                                  valueDict["mass"],
                                  valueDict["rotationPeriod"],
                                  valueDict["gravity"],
                                  valueDict["temperature"],
                                  valueDict["pressure"],
                                  valueDict["orbitalPeriod"],
                                  valueDict["orbitalSpeed"]);



        }
    }

    public class HorizonsObserverResponseReader : ObjectReader, IHorizonsResponseReader<EphemerisObserverData>
    {

        public HorizonsObserverResponseReader(string data, string objectName, int objectCode)
        {
            this._data = data ?? throw new ArgumentNullException(nameof(data));
            this._objectName = objectName;
            this._objectCode = objectCode;
        }

        public EphemerisObserverData Read()
        {

            ObjectData objectData = this.readObjectinfo();
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
        

        public HorizonsVectorResponseReader(string data, string objectName, int objectCode)
        {
            this._data = data;
            this._objectName = objectName;
            this._objectCode = objectCode;
        }

        public EphemerisVectorData Read() //prozkoumat
        {
            ObjectData objectData = this.readObjectinfo();
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
