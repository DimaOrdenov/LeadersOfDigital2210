namespace Dal.Entities
{
    public class Weather
    {
        public int Id { get; set; }

        public bool IsRainy { get; set; }

        public bool IsSunny { get; set; }

        public bool IsStormy { get; set; }

        public double Temperature { get; set; }
    }
}
