namespace Weather.Model
{
    public class ForecastForHour
    {
        public string Description { get; set; }
        public string time { get; set; }
        public int temp { get; set; }
        public string icon { get; set; }
        public int humidity { get; set; }
        public int wind { get; set; }
        public int deg { get; set; }
    }
}
