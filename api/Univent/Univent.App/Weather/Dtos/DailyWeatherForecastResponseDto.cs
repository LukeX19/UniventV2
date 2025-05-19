namespace Univent.App.Weather.Dtos
{
    public class DailyWeatherForecastResponseDto
    {
        public DateTime Date { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public double PrecipitationProbability { get; set; }
        public double Uvi { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public double? RainVolume { get; set; }
        public double? SnowVolume { get; set; }
    }
}
