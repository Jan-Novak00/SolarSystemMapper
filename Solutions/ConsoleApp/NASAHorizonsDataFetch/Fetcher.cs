using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystemMapper
{
    /**
     * Fetches data in paralell form NASA Horizons API
    */
    public class NASAHorizonsDataFetcher
    {
        /**
         * Tells NASAHorizonsDataFetcher what purpose do the data serve. With this information NASAHorizonsDataFetcher can adjust querries
        */
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
        /**
         * Translates planet names into planet satelite mode.
        */
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
        /**
         * @param mode Mode of the fetch.
         * @param ObjectsToFetch Objects to fetch
         * @param startDate First date for which to fetch the data.
         * @param endDate Last date for which to fetch the data.
         * @param observerLatitude Latitude of the observer, relevant only if mode = MapMode.NightSky
         * @param observerLongitude Longitude of the observer, relecant only if mode = MapMode.NightSky
        */
        public NASAHorizonsDataFetcher(MapMode mode, List<ObjectEntry> ObjectsToFetch, DateTime startDate, DateTime endDate, double observerLatitude = 0, double observerLongitude = 0)
        {
            Mode = mode;
            _objectsToFetch = ObjectsToFetch;
            this._startDate = startDate;
            this._endDate = endDate;
            this._observerLatitude = observerLatitude;
            this._observerLongitude = observerLongitude;
        }
        /**
         * Generates querry for NASA Horizons API.
         * @param objectCode NASA Horizon API code for the object to fetch
         * @return URL with querry
        */
        private string _generateURl(int objectCode)
        {

            
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

        /**
         * Fetches data in parallel form NASA Horizons API and parses them.
        */
        public async Task<IEnumerable<IEphemerisData<IEphemerisTableRow>>> Fetch()
        {

            var answers = await _fetchAnswersWithLimit(2);

            return (Mode == MapMode.NightSky)
                ? _readDataParallel<HorizonsObserverResponseReader, EphemerisObserverData>(answers)
                : _readDataParallel<HorizonsVectorResponseReader, EphemerisVectorData>(answers);

        }
        /**
         * Fetches data from NASA Horizons API i paralell.
         * @param maxParallelism Maximal number of parralel threads.
        */
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
                    Debug.WriteLine(answer);
                    return answer;
                }
                finally
                {
                    semaphore.Release();
                }

            });

            return await Task.WhenAll(tasks);
        }


        [Obsolete]
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
