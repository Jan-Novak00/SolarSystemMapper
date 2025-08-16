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
        public PixelBodyInfo(Point bodyCoordinates, Point centerCoordinates, bool visible, int diameter, Color color)
        {
            BodyCoordinates = bodyCoordinates;
            CenterCoordinates = centerCoordinates;
            Visible = visible;
            Diameter = diameter;
            Color = color;
        }

        public Point BodyCoordinates { get; }
        public Point CenterCoordinates { get; }
        public bool Visible { get; set; }
        public int Diameter { get; set; }
        public Color Color { get; set; }
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
                    return 4;
            }
        }

        internal static PixelBodyInfo ToPixelBodyInfo(this EphemerisTableRowObserver row, Point center, float scale_Km)
        {
            throw new NotImplementedException();
        }

        internal static PixelBodyInfo ToPixelBodyInfo(this EphemerisTableRowVector row, Point center, float scale_Km)
        {
            Point pixelCoordiantes = _KmToPixels(row.X, row.Y, scale_Km);
            Point finalCoordinates = new Point(pixelCoordiantes.X + center.X, pixelCoordiantes.Y+center.Y);
            return new PixelBodyInfo(finalCoordinates, center, false, 0, Color.White);
        }

        internal static FormBody<EphemerisObserverData> ToFormBody(this EphemerisObserverData observerData,Point center,float scale_Km,int mapRadius)
        {
            List<PixelBodyInfo> pixelBodyInfos = new List<PixelBodyInfo>();

            foreach (var row in observerData.ephemerisTable)
            {
                var pixelBodyInfo = row.ToPixelBodyInfo(center, scale_Km);

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

        internal static FormBody<EphemerisVectorData> ToFormBody(this EphemerisVectorData vectorData, Point center, float scale_Km, int mapHeight, int mapWidth)
        {
            List<PixelBodyInfo> pixelBodyInfos = new List<PixelBodyInfo>();
            foreach (var row in vectorData.ephemerisTable)
            {
                var pixelBodyInfo = row.ToPixelBodyInfo(center, scale_Km);
                pixelBodyInfo.Visible = !((pixelBodyInfo.BodyCoordinates.X > mapWidth) || (pixelBodyInfo.BodyCoordinates.X < 0) || (pixelBodyInfo.BodyCoordinates.Y > mapHeight) || (pixelBodyInfo.BodyCoordinates.Y < 0));
                pixelBodyInfo.Diameter = _getDiameter(vectorData.objectData.Type);
                pixelBodyInfo.Color = _getColor(vectorData.objectData.Name);
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
    }


}
