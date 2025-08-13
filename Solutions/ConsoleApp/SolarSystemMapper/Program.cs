namespace SolarSystemMapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string data = GetHorizonsData().GetAwaiter().GetResult();
            Console.WriteLine(data);
            HorizonsVectorResponseReader reader = new HorizonsVectorResponseReader(data, "Mars", 500);
            Console.WriteLine(reader.Read().ToString());
        }
        static async Task<string> GetHorizonsData()
        {
            using var client = new HttpClient();

            var baseUrl = "https://ssd.jpl.nasa.gov/api/horizons.api";
            var query = new[]
            {
            "format=text",
            "COMMAND='499'", // Mars
            "EPHEM_TYPE=VECTOR",
            "CENTER='500@399'", // pozorovatel: Země
            "SITE_COORD='0,0,0'", // volitelně GPS
            "START_TIME='2000-07-08'",
            "STOP_TIME='2000-07-12'",
            "STEP_SIZE='1 d'",
            "QUANTITIES='1,3,4'"
            };

            var url = baseUrl + "?" + string.Join("&", query);
            var response = await client.GetStringAsync(url);
            return response;
        }

    }
}
