namespace Business.Definitions.Models
{
    public class Position
    {
        public Position(double lat, double lng)
        {
            Lat = lat;
            Lng = lng;
        }

        public double Lat { get; }

        public double Lng { get; }
    }
}
