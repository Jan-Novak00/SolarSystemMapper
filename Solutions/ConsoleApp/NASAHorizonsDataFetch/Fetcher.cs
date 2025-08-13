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
        private readonly double _observerLatitude;
        private readonly double _observerLongitude;

        public NASAHorizonsDataFetcher(MapMode mode, List<ObjectEntry> ObjectsToFetch, double observerLatitude = 0, double observerLongitude = 0)
        {
            Mode = mode;
            _objectsToFetch = ObjectsToFetch;
            this._observerLatitude = observerLatitude;
            this._observerLongitude = observerLongitude;
        }

        private string _askServerForData(int objectCode)
        {
            throw new NotImplementedException();
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

        public IEnumerable<IEphemerisData<IEphemerisTableRow>> Fetch()
        {

            string[] answers = new string[_objectsToFetch.Count];
            
            for (int i = 0; i < answers.Length; i++)
            {
                answers[i] = _askServerForData(_objectsToFetch[i].Code);
            }


            return (Mode == MapMode.NightSky) ? _readData<HorizonsObserverResponseReader, EphemerisObserverData>(answers) : 
                                                _readData<HorizonsVectorResponseReader, EphemerisVectorData>(answers);


        }


    }
}
