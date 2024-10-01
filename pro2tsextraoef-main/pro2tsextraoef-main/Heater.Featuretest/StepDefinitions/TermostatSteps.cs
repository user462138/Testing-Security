using Xunit.Gherkin.Quick;

namespace Heater.Featuretest.StepDefinitions
{
    [FeatureFile("./Features/Thermostat.Feature")]
    public sealed class TermostatSteps : Feature
    {
        private static readonly Dictionary<double, string> MockoonUrl = new()
        {
            { 14, "http://localhost:3001/data/2.5/weather/14" },
            { 15, "http://localhost:3001/data/2.5/weather/15" },
            { 20, "http://localhost:3001/data/2.5/weather/20" },
            { 25, "http://localhost:3001/data/2.5/weather/25" },
            { 26, "http://localhost:3001/data/2.5/weather/26" }
        };
        private const int MaxFailures = 1;

        private readonly PretendHeatingElement heatingElement;
        private readonly OpenWeatherTemperatureSensor temperatureSensor;
        private readonly Thermostat thermostat;

        public TermostatSteps()
        {
            heatingElement = new PretendHeatingElement();
            temperatureSensor = new OpenWeatherTemperatureSensor() { Url = MockoonUrl[20] };
            thermostat = new Thermostat(heatingElement, temperatureSensor) { MaxTemperatureFailures = MaxFailures };
        }

        [Given(@"the temperature setpoint is (.*)°C")]
        public void SetTemperatureSetpoint(int setpoint)
        {
            thermostat.TemperatureSetpoint = setpoint;
        }

        [And(@"the temperature offset is (.*)°C")]
        public void SetTemperatureOffset(int offset)
        {
            thermostat.TemperatureOffset = offset;
        }

        [Given(@"the heater is off")]
        public void HeaterIsOff()
        {
            temperatureSensor.Url = MockoonUrl[Convert.ToInt32(thermostat.TemperatureSetpoint + thermostat.TemperatureOffset + 1.0)];
            thermostat.Work();
            Assert.False(heatingElement.Enabled);
        }

        [Given(@"the heater is on")]
        public void HeaterIsOn()
        {
            temperatureSensor.Url = MockoonUrl[Convert.ToInt32(thermostat.TemperatureSetpoint - thermostat.TemperatureOffset - 1.0)];
            thermostat.Work();
            Assert.True(heatingElement.Enabled);
        }

        [When(@"the temperature drops to (.*)°C")]
        [When(@"the temperature increases to (.*)°C")]
        public void SetTemperature(int temperature)
        {
            temperatureSensor.Url = MockoonUrl[temperature];
            thermostat.Work();
        }

        [When(@"the temperature drops below (.*)°C")]
        public void TemperatureDropsBelow(int temperature)
        {
            temperatureSensor.Url = MockoonUrl[temperature - 1];
            thermostat.Work();
        }

        [When(@"the temperature increases above (.*)°C")]
        public void TemperatureIncreasesAbove(int temperature)
        {
            temperatureSensor.Url = MockoonUrl[temperature + 1];
            thermostat.Work();
        }

        [Then(@"the heater is turned on")]
        public void CheckHeaterTurnedOn()
        {
            Assert.True(heatingElement.Enabled);
        }

        [Then(@"the heater is not turned on")]
        public void CheckHeaterNotTurnedOn()
        {
            Assert.False(heatingElement.Enabled);
        }

        [Then(@"the heater is turned off")]
        public void CheckHeaterTurnedOff()
        {
            Assert.False(heatingElement.Enabled);
        }

        [Then(@"the heater is not turned off")]
        public void CheckHeaterNotTurnedOff()
        {
            Assert.True(heatingElement.Enabled);
        }
    }
}
