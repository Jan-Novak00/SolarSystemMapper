using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SolarSystemMapper;


namespace SolarMapperUI
{
    internal class PixelBodyInfo
    { 
        public PixelBodyInfo(Point bodyCoordinates, Point centerCoordinates, bool visible, int diameter, Color color, bool showName)
        {
            BodyCoordinates = bodyCoordinates;
            CenterCoordinates = centerCoordinates;
            Visible = visible;
            Diameter = diameter;
            Color = color;
            ShowName = showName;
        }

        public Point BodyCoordinates { get; }
        public Point CenterCoordinates { get; }
        public bool Visible { get; set; }
        public int Diameter { get; set; }
        public Color Color { get; set; }
        public bool ShowName { get; set; }
    }
    internal class FormBody<TData> where TData : IEphemerisData<IEphemerisTableRow>
    {
        public TData BodyData { get; init; }
        public List<PixelBodyInfo> PixelInfos { get; init; }

        public FormBody(TData bodyData, List<PixelBodyInfo> pixelInfo)
        {
            BodyData = bodyData;
            PixelInfos = pixelInfo;
        }

        public string BodyReport(DateTime date)
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append(BodyData.ToString());

            if (this.BodyData is EphemerisVectorData data)
            {

                double x = 0; double y = 0; double z = 0;
                double vx = 0; double vy = 0; double vz = 0;
                foreach (var row in data.ephemerisTable)
                {
                    if (!row.date.HasValue) continue;
                    if (row.date.Value.Date == date.Date)
                    {
                        x = row.X ?? 0;
                        y = row.Y ?? 0;
                        z = row.Z ?? 0;
                        vx = row.VX ?? 0;
                        vy = row.VY ?? 0;
                        vz = row.VZ ?? 0;
                        break;
                    }

                }
                strBuilder.Append($"Current distance from the Sun: {Math.Sqrt(x*x+y*y+z*z)} km    Current speed (relative to the Sun): {Math.Sqrt(vx*vx+vy*vy+vz*vz)} km/s");   
            }
            else if (this.BodyData is EphemerisObserverData odata)
            {
                double rad = 0;
                double dec = 0;
                double azi = 0;
                double elev = 0;
                foreach (var row in odata.ephemerisTable)
                {
                    if (!row.date.HasValue) continue;
                    if (row.date.Value.Date == date.Date)
                    {
                        
                        rad = (!(row.RA is null)) ? row.RA[0] + row.RA[1] / 60 + row.RA[2] / 3600 : double.NaN;
                        dec = (!(row.DEC is null)) ? row.DEC[0] + row.DEC[1] / 60 + row.DEC[2] / 3600 : double.NaN;
                        azi = row.Azi ?? double.NaN;
                        elev = row.Elev ?? double.NaN;

                        break;
                    }

                }
                strBuilder.AppendLine($"Right Ascention: {rad} h    Declination: {dec}°");
                strBuilder.AppendLine($"Azimuth: {azi}°    Elevation: {elev}°");

            }
            

            return strBuilder.ToString();
        }

    }

    internal static class Translation
    {


        private static Color _getColor(string name)
        {
            switch (name)
            {
                case "Sun":
                    return Color.Yellow;
                case "Mercury":
                    return Color.SlateGray;
                case "Venus":
                    return Color.Orange;
                case "Earth":
                    return Color.Aquamarine;
                case "Mars":
                    return Color.Red;
                case "Jupiter":
                    return Color.Tan;
                case "Saturn":
                    return Color.NavajoWhite;
                case "Uranus":
                    return Color.Aqua;
                case "Neptune":
                    return Color.Blue;
                default:
                    return Color.White;
            }

        }

        private static int _getDiameter(string type)
        {
            switch (type)
            {
                case "Planet":
                    return 10;
                case "Star":
                    return 30;
                case "Dwarf Planet":
                    return 8;
                default:
                    return 8;
            }
        }

        internal static PixelBodyInfo ToPixelBodyInfo(this EphemerisTableRowObserver row, Point center, int mapRadius, string bodyType, string bodyName)
        {
            
            Point pixelCoordinates = _degreesToPixels(row.Azi ?? 0, row.Elev ?? double.NegativeInfinity, mapRadius);
            int diameter = _getDiameter(bodyType);
            Point finalCoordinates = new Point(center.X + pixelCoordinates.X - diameter/2, center.Y + pixelCoordinates.Y - diameter/2);
            return new PixelBodyInfo(finalCoordinates, center, row.Elev >= 0, diameter, _getColor(bodyName), bodyType == "Planet" || bodyType == "Star");

        }

        internal static PixelBodyInfo ToPixelBodyInfo(this EphemerisTableRowVector row, Point center, float scale_Km, string bodyType, string bodyName)
        {
            int diameter = _getDiameter(bodyType);
            Point pixelCoordiantes = _KmToPixels(row.X, row.Y, scale_Km);
            Point finalCoordinates = new Point(pixelCoordiantes.X + center.X - diameter/2, pixelCoordiantes.Y+center.Y - diameter / 2);
            return new PixelBodyInfo(finalCoordinates, center, false, diameter, _getColor(bodyName), bodyType == "Planet" || bodyType == "Star");
        }

        internal static FormBody<EphemerisObserverData> ToFormBody(this EphemerisObserverData observerData,Point center,int mapRadius)
        {
            List<PixelBodyInfo> pixelBodyInfos = new List<PixelBodyInfo>();

            foreach (var row in observerData.ephemerisTable)
            {
                var pixelBodyInfo = row.ToPixelBodyInfo(center, mapRadius, observerData.objectData.Type, observerData.objectData.Name);

                int dx = pixelBodyInfo.BodyCoordinates.X - center.X;
                int dy = pixelBodyInfo.BodyCoordinates.Y - center.Y;

                double dist = Math.Sqrt(dx * dx + dy * dy);

                pixelBodyInfo.Visible = dist <= mapRadius;

                pixelBodyInfo.Diameter = _getDiameter(observerData.objectData.Type);
                pixelBodyInfo.Color = _getColor(observerData.objectData.Name);
                pixelBodyInfos.Add(pixelBodyInfo);
            }

            return new FormBody<EphemerisObserverData>(observerData, pixelBodyInfos);
        }

        internal static FormBody<EphemerisVectorData> ToFormBody(this EphemerisVectorData vectorData, Point center, float scale_Km, int mapHeight, int mapWidth, bool respectScale = false)
        {
            List<PixelBodyInfo> pixelBodyInfos = new List<PixelBodyInfo>();
            foreach (var row in vectorData.ephemerisTable)
            {
                var pixelBodyInfo = row.ToPixelBodyInfo(center, scale_Km, vectorData.objectData.Type, vectorData.objectData.Name);
                if (respectScale) pixelBodyInfo.Diameter = (int)(Math.Ceiling((vectorData.objectData.Radius_km == double.NaN) ? 0 : vectorData.objectData.Radius_km) / scale_Km);

                pixelBodyInfo.Visible = !((pixelBodyInfo.BodyCoordinates.X > mapWidth) || (pixelBodyInfo.BodyCoordinates.X < 0) || (pixelBodyInfo.BodyCoordinates.Y > mapHeight) || (pixelBodyInfo.BodyCoordinates.Y < 0));
                pixelBodyInfos.Add(pixelBodyInfo);
            }

            return new FormBody<EphemerisVectorData>(vectorData, pixelBodyInfos);
        }



        private static Point _KmToPixels(double? xKm, double? yKm, float scale_Km)
        {
            int xPx = (!(xKm is null)) ? (int)(xKm / scale_Km) : int.MinValue;
            int yPx = (!(yKm is null)) ? (int)(yKm / scale_Km) : int.MinValue;
            return new Point(xPx, yPx);
        }

        public static Point _degreesToPixels(double azimuthDeg, double elevationDeg, double mapRadius)
        {
            // převod na radiány
            double az = azimuthDeg * Math.PI / 180.0;
            double el = elevationDeg * Math.PI / 180.0;

            // vzdálenost od středu (zenitu) – čím menší elevace, tím dál od středu
            double r = (Math.PI / 2.0 - el) / (Math.PI / 2.0) * mapRadius;

            // souřadnice
            double x = r * Math.Sin(az);
            double y = - r * Math.Cos(az);

            return new Point((int)x, (int)y);
        }
    }


}
