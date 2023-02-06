namespace trackingAPI;

public class WeatherForecast
{
    //Temp commit for merge hehe hej brother v2

    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }

    //sup brother
}