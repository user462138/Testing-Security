using Xunit.Gherkin.Quick;

namespace Heater.Featuretest.StepDefinitions
{
    [FeatureFile("./Features/FailSafe.Feature")]
    public sealed class FailSafeSteps : Feature
    {
        private static readonly Dictionary<double, string> MockoonUrl = new()
        {
            { -1, "http://localhost:3001/data/2.5/notweather" },
            { 14, "http://localhost:3001/data/2.5/weather/14" },
            { 15, "http://localhost:3001/data/2.5/weather/15" },
            { 20, "http://localhost:3001/data/2.5/weather/20" },
            { 25, "http://localhost:3001/data/2.5/weather/25" },
            { 26, "http://localhost:3001/data/2.5/weather/26" }
        };
        private const int MaxFailures = 2;

        private readonly PretendHeatingElement heatingElement;
        private readonly OpenWeatherTemperatureSensor temperatureSensor;
        private readonly Thermostat thermostat;

        public FailSafeSteps()
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

        [And(@"the maximum number of temperature sensor failures is (.*)")]
        public void SetMaxFailures(int maxFailures)
        {
            thermostat.MaxTemperatureFailures = maxFailures;
        }

        [Given(@"the heater is on")]
        public void HeaterIsOn()
        {
            temperatureSensor.Url = MockoonUrl[Convert.ToInt32(thermostat.TemperatureSetpoint - thermostat.TemperatureOffset - 1.0)];
            thermostat.Work();
            Assert.True(heatingElement.Enabled);
        }

        [Given(@"the thermostat is in safe mode because of temperature sensor failure")]
        [When(@"the temperature sensor fails more than the maximum number of times")]
        public void TemperatureSensorFailsTooManyTimes()
        {
            temperatureSensor.Url = MockoonUrl[-1];
            for (int i = 0; i <= thermostat.MaxTemperatureFailures; ++i)
            {
                thermostat.Work();
            }
        }

        [When(@"the temperature drops below (.*)°C")]
        public void TemperatureDropsBelow(int temperature)
        {
            temperatureSensor.Url = MockoonUrl[temperature - 1];
            thermostat.Work();
        }

        [Then(@"the heater is turned on")]
        public void CheckHeaterTurnedOn()
        {
            Assert.True(heatingElement.Enabled);
        }

        [Then(@"the heater is turned off")]
        public void CheckHeaterTurnedOff()
        {
            Assert.False(heatingElement.Enabled);
        }
    }
}
