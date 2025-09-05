using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

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

        
        
        

        private IEnumerable<TData> _readDataParallel<TReader, TData>(List<Tuple<ObjectEntry, string>> rawData)
            where TReader : IHorizonsResponseReader<TData>
            where TData : IEphemerisData<IEphemerisTableRow>
        {
            var results = new TData[rawData.Count];

            Parallel.ForEach(
                Enumerable.Range(0, rawData.Count),
                i =>
                {
                    
                    IHorizonsResponseReader<TData> reader = (Mode == MapMode.NightSky)
                                                            ? (IHorizonsResponseReader<TData>)new HorizonsObserverResponseReader(rawData[i].Item2, rawData[i].Item1.Name, rawData[i].Item1.Type, rawData[i].Item1.Code)
                                                            : (IHorizonsResponseReader<TData>)new HorizonsVectorResponseReader(rawData[i].Item2, rawData[i].Item1.Name, rawData[i].Item1.Type, rawData[i].Item1.Code);

                    results[i] = reader.Read();
                });

            return results;
        }

        /**
         * Fetches data in parallel from NASA Horizons API and parses them.
        */
        public async Task<IEnumerable<IEphemerisData<IEphemerisTableRow>>> Fetch()
        {

            var answers = await _fetchAnswersWithLimit();

            return (Mode == MapMode.NightSky)
                ? _readDataParallel<HorizonsObserverResponseReader, EphemerisObserverData>(answers)
                : _readDataParallel<HorizonsVectorResponseReader, EphemerisVectorData>(answers);

        }



        private int _maxParallelism = 3;

        /**
         * Fetches data from NASA Horizons API in paralell. Adjusts number of threads
         * @return Task<List<Tuple<ObjectEntry,string>>> - tuples of object entry with coresponding response
        */
        private async Task<List<Tuple<ObjectEntry,string>>> _fetchAnswersWithLimit()
        {
           
            using var client = new HttpClient();
            var queue = new ConcurrentQueue<HttpWorker>();
            foreach (var obj in _objectsToFetch) queue.Enqueue(new HttpWorker(client, this._generateURl(obj.Code),obj));

            int maxNumerOfParallelRequests = _maxParallelism;
            var maxNumberLock = new object();

            var results = new List<Tuple<ObjectEntry, string>>();
            object resultsLock = new object();

            var activeTasks = new List<Task>();
            

            while (!queue.IsEmpty || activeTasks.Count > 0)
            {
                

                while (queue.TryDequeue(out var worker))
                {
                    lock (maxNumberLock)
                    {
                        if (activeTasks.Count >= maxNumerOfParallelRequests) 
                        {
                            queue.Enqueue(worker);
                            break;
                        }
                    }
                    var task = Task.Run(async () =>
                    {
                        var response = await worker.Work();

                        if ((int)response.StatusCode >= 500)
                        {
                            lock (maxNumberLock)
                            {
                                if (maxNumerOfParallelRequests != 1) maxNumerOfParallelRequests--;
                            }

                            queue.Enqueue(worker);
                            return;
                        }

                        if ((int)response.StatusCode >= 400)
                            return;

                        lock (maxNumberLock)
                        {
                            maxNumerOfParallelRequests++;
                        }
                        
                        var answer = await response.Content.ReadAsStringAsync();
                        lock (resultsLock) results.Add(new Tuple<ObjectEntry, string>(worker.Entry, answer));
                    });

                    activeTasks.Add(task);
                }

                if (activeTasks.Count == 0)
                    break;

                var finishedTask = await Task.WhenAny(activeTasks);
                activeTasks.Remove(finishedTask);
            }
            return results;


        }




        /**
        * Fetches data from NASA Horizons API. Used for parallel fetching
        */

        private class HttpWorker
        {
            public HttpWorker(HttpClient client, string url, ObjectEntry entry)
            {
                Client = client;
                Entry = entry;
                URL = url;
                
            }

            public HttpClient Client { get; init; }
            public ObjectEntry Entry { get; init; }
            public int NumberOfAttempts { get; private set; } = 0;
            public string URL { get; init; }

            public async Task<HttpResponseMessage> Work()
            {
                NumberOfAttempts++;
                var response = await Client.GetAsync(URL);
                return response;
            }

        }


    }
}
