namespace SolarSystemMapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var data = GetHorizonsData().GetAwaiter().GetResult();
            foreach (var item in data) 
            { 
                Console.WriteLine(item);
                foreach (var row in item.ephemerisTable)
                {
                    Console.WriteLine(row);
                }
                Console.WriteLine("------------------------------------------");
            }

           
        }
        static async Task<IEnumerable<IEphemerisData<IEphemerisTableRow>>> GetHorizonsData()
        {
            var objects = DataTables.Planets.ToList();
            var fetcher = new NASAHorizonsDataFetcher(NASAHorizonsDataFetcher.MapMode.SolarSystem, objects, DateTime.Today, DateTime.Today.AddDays(3));
            var result = await fetcher.Fetch();
            return result;
        }

    }
}
