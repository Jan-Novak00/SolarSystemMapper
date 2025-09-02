using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystemMapper
{
    public class NASAHorizonsDataFetcher
    {
        public enum MapMode : int
        {
            NightSky,
            SolarSystem,
            EarthSatelites = 399,
            MarsSatelites = 499,
            JupiterSatelites = 599,
            SaturnSatelites = 699,
            UranusSatelites = 799,
            NeptuneSatelites = 899,
            PlutoSatelites = 999
        }

        public static MapMode ObjectToMapMode(string objectName)
        {
            switch (objectName)
            {
                case "Earth":
                    return MapMode.EarthSatelites;
                case "Mars":
                    return MapMode.MarsSatelites;
                case "Jupiter":
                    return MapMode.JupiterSatelites;
                case "Saturn":
                    return MapMode.SaturnSatelites;
                case "Uranus":
                    return MapMode.UranusSatelites;
                case "Neptune":
                    return MapMode.NeptuneSatelites;
                case "Pluto":
                    return MapMode.PlutoSatelites;
                default:
                    return (MapMode)(-1);
            }
        }

        public MapMode Mode { get; private set; }
        private List<ObjectEntry> _objectsToFetch;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly double _observerLatitude;
        private readonly double _observerLongitude;

        private const string NASAHorizonsURL = "https://ssd.jpl.nasa.gov/api/horizons.api";

        public NASAHorizonsDataFetcher(MapMode mode, List<ObjectEntry> ObjectsToFetch, DateTime startDate, DateTime endDate, double observerLatitude = 0, double observerLongitude = -90)
        {
            Mode = mode;
            _objectsToFetch = ObjectsToFetch;
            this._startDate = startDate;
            this._endDate = endDate;
            this._observerLatitude = observerLatitude;
            this._observerLongitude = observerLongitude;
        }

        private string _generateURl(int objectCode)
        {

            using var client = new HttpClient();
            string center = this.Mode switch
            {
                MapMode.NightSky => "coord@399",
                MapMode.SolarSystem => "@10",
                _ => "@" + ((int)this.Mode).ToString(),
            };

            string ephemType = (Mode == MapMode.NightSky) ? "OBSERVER" : "VECTOR";
            var query = new string[]
            {
                "format=text",
                $"COMMAND='{objectCode}'",
                $"EPHEM_TYPE={ephemType}",
                $"CENTER='{center}'",
                $"SITE_COORD='{this._observerLatitude},{this._observerLongitude},0'",
                $"START_TIME='{this._startDate.ToString("yyyy-MM-dd")}'",
                $"STOP_TIME='{this._endDate.ToString("yyyy-MM-dd")}'",
                "STEP_SIZE='1 d'",
                "QUANTITIES='1,3,4'"
            };

            return NASAHorizonsURL + "?" + string.Join("&", query);
            


        }
        [Obsolete]
        private IEnumerable<TData> _readData<TReader, TData>(string[] rawData)
            where TReader : IHorizonsResponseReader<TData>
            where TData : IEphemerisData<IEphemerisTableRow>
        {
            var result = new List<TData>();

            for (int i = 0; i < rawData.Length; i++)
            {
                var answer = rawData[i];
                var name = _objectsToFetch[i].Name;
                var code = _objectsToFetch[i].Code;

                IHorizonsResponseReader<TData> dataReader = (Mode == MapMode.NightSky)
                                                            ? (IHorizonsResponseReader<TData>)new HorizonsObserverResponseReader(answer, name, _objectsToFetch[i].Type, code)
                                                            : (IHorizonsResponseReader<TData>)new HorizonsVectorResponseReader(answer, name, _objectsToFetch[i].Type, code);

                result.Add(dataReader.Read());
            }

            return result;
        }

        public async Task<IEnumerable<IEphemerisData<IEphemerisTableRow>>> Fetch()
        {

            var answers = await _fetchAnswersWithLimit(2);

            return (Mode == MapMode.NightSky)
                ? _readDataParallel<HorizonsObserverResponseReader, EphemerisObserverData>(answers)
                : _readDataParallel<HorizonsVectorResponseReader, EphemerisVectorData>(answers);

        }
        private async Task<string[]> _fetchAnswersWithLimit(int maxParallelism)
        {
            using var semaphore = new SemaphoreSlim(maxParallelism);
            using var client = new HttpClient();

            IEnumerable<Task<string>> tasks = _objectsToFetch.Select(async obj =>
            {
                await semaphore.WaitAsync();
                try
                {
                    string answer = await client.GetStringAsync(_generateURl(obj.Code));
                    //Debug.WriteLine(answer);
                    //Debug.WriteLine("------------------");
                    return answer;
                }
                finally
                {
                    semaphore.Release();
                }

            });

            return await Task.WhenAll(tasks);
        }



        private IEnumerable<TData> _readDataParallel<TReader, TData>(string[] rawData)
            where TReader : IHorizonsResponseReader<TData>
            where TData : IEphemerisData<IEphemerisTableRow>
        {
            var results = new TData[rawData.Length];

            Parallel.ForEach(
                Enumerable.Range(0, rawData.Length),
                i =>
                {
                    var obj = _objectsToFetch[i];
                    IHorizonsResponseReader<TData> reader = (Mode == MapMode.NightSky)
                                                            ? (IHorizonsResponseReader<TData>) new HorizonsObserverResponseReader(rawData[i], obj.Name, obj.Type, obj.Code)
                                                            : (IHorizonsResponseReader<TData>) new HorizonsVectorResponseReader(rawData[i], obj.Name, obj.Type, obj.Code);

                    results[i] = reader.Read();
                });

            return results;
        }


    }
}
