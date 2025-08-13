using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystemMapper
{
    public class NASAHorizonsDataFetcher
    {
        public enum MapMode
        {
            NightSky,
            SolarSystem
        }

        public MapMode Mode { get; private set; }
        private List<ObjectEntry> _objectsToFetch;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly double _observerLatitude;
        private readonly double _observerLongitude;

        private const string NASAHorizonsURL = "https://ssd.jpl.nasa.gov/api/horizons.api";

        public NASAHorizonsDataFetcher(MapMode mode, List<ObjectEntry> ObjectsToFetch, DateTime startDate, DateTime endDate, double observerLatitude = 0, double observerLongitude = 0)
        {
            Mode = mode;
            _objectsToFetch = ObjectsToFetch;
            this._startDate = startDate;
            this._endDate = endDate;
            this._observerLatitude = observerLatitude;
            this._observerLongitude = observerLongitude;
        }

        private async Task<string> _askServerForData(int objectCode)
        {

            using var client = new HttpClient();
            string center = (Mode == MapMode.NightSky) ? "399" : "@10";
            var query = new string[]
            {
                "format=text",
                $"COMMAND='{objectCode}'",
                "EPHEM_TYPE=OBSERVER",
                $"CENTER='{center}'",
                $"SITE_COORD='{this._observerLatitude},{this._observerLongitude},0'",
                $"START_TIME='{this._startDate.ToString("yyyy-MM-dd")}'",
                $"STOP_TIME='{this._startDate.ToString("yyyy-MM-dd")}'",
                "STEP_SIZE='1 d'",
                "QUANTITIES='1,3,4'"
            };

            var url = NASAHorizonsURL + string.Join("&", query);
            return await client.GetStringAsync(url);


        }
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
                                                            ? (IHorizonsResponseReader<TData>)new HorizonsObserverResponseReader(answer, name, code)
                                                            : (IHorizonsResponseReader<TData>)new HorizonsVectorResponseReader(answer, name, code);

                result.Add(dataReader.Read());
            }

            return result;
        }

        public async Task<IEnumerable<IEphemerisData<IEphemerisTableRow>>> Fetch()
        {

            string[] answers = new string[_objectsToFetch.Count];

            for (int i = 0; i < answers.Length; i++)
            {
                answers[i] = await _askServerForData(_objectsToFetch[i].Code);
            }


            return (Mode == MapMode.NightSky) ? _readData<HorizonsObserverResponseReader, EphemerisObserverData>(answers) :
                                                _readData<HorizonsVectorResponseReader, EphemerisVectorData>(answers);


        }


    }
}
