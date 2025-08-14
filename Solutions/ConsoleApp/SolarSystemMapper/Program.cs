namespace SolarSystemMapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var data = GetHorizonsData().GetAwaiter().GetResult();
            foreach (var item in data) Console.WriteLine(item);

            var I = (data.ToList<IEphemerisData<IEphemerisTableRow>>()[0]);
            var J = (EphemerisVectorData)I;
            Console.WriteLine("Here it comes...");
            var value = J.ephemerisTable.ToList<EphemerisVectorData.EphemerisTableRowVector>()[0].X;
            Console.WriteLine(value.Value);
        }
        static async Task<IEnumerable<IEphemerisData<IEphemerisTableRow>>> GetHorizonsData()
        {
            var objects = new List<ObjectEntry>()
            {
                new ObjectEntry("Mercury",199,"Planet"),
                new ObjectEntry("Venus",299, "Planet"),
                new ObjectEntry("Earth",399,"Planet")
            };
            var fetcher = new NASAHorizonsDataFetcher(NASAHorizonsDataFetcher.MapMode.SolarSystem, objects, DateTime.Today, DateTime.Today.AddDays(3));
            var result = await fetcher.Fetch();
            return result;
        }

    }
}
