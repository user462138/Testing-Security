namespace Heater.Integrationtest
{
    public class TemperatureSensorTest
    {
        private const double MockoonTemperature = 20.0;
        private const string MockoonUrl = "http://localhost:3001/data/2.5/weather";

        [Fact]
        public void TemperatureCanBeFetchedFromOpenWeatherApi()
        {
            var temperatureSensor = new OpenWeatherTemperatureSensor() { Url = MockoonUrl };

            var result = temperatureSensor.Temperature();

            Assert.Equal(MockoonTemperature, result);
        }
    }
}
