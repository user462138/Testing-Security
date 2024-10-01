/*
 * Note: These integration tests are implemented for demonstration purposes only.
 *       The logic is already tested in the ThermostatTest unittests.
 *       The pretend heating element is already tested in the HeatingElementTest unittests.
 *       The open weather temperature sensor is already tested in the TemperatureSensorTest integrationtests.
 *       So testing them as a whole again does not increase test coverage.
 */

namespace Heater.Integrationtest
{
    public class ThermostatTest
    {
        private const double Offset = 5.0;
        private const int MaxFailures = 2;
        private const double MockoonTemperature = 20.0;
        private const string MockoonUrl = "http://localhost:3001/data/2.5/weather";
        private const string BadUrl = "http://localhost:3001/data/2.5/not-weather";

        private readonly PretendHeatingElement heatingElement;
        private readonly OpenWeatherTemperatureSensor temperatureSensor;
        private readonly Thermostat thermostat;
        
        public ThermostatTest()
        {
            heatingElement = new PretendHeatingElement();
            temperatureSensor = new OpenWeatherTemperatureSensor() { Url = MockoonUrl };
            thermostat = new Thermostat(heatingElement, temperatureSensor)
            {
                TemperatureOffset = Offset,
                MaxTemperatureFailures = MaxFailures
             };
        }

        [Fact]
        public void HeaterIsTurnedOnWhenTemperatureIsBelowSetpointMinusOffset()
        {
            thermostat.TemperatureSetpoint = MockoonTemperature + Offset + .5;

            thermostat.Work();

            Assert.True(heatingElement.Enabled);
        }

        [Fact]
        public void HeaterIsNotTurnedOnWhenTemperatureIsAboveSetpointMinusOffset()
        {
            thermostat.TemperatureSetpoint = MockoonTemperature + Offset;

            thermostat.Work();

            Assert.False(heatingElement.Enabled);
        }

        [Fact]
        public void HeaterIsTurnedOffWhenTemperatureIsAboveSetpointPlusOffset()
        {
            thermostat.TemperatureSetpoint = MockoonTemperature - Offset - .5;

            thermostat.Work();

            Assert.False(heatingElement.Enabled);
        }

        [Fact]
        public void HeaterIsNotTurnedOffWhenTemperatureIsBelowSetpointPlusOffset()
        {
            GivenHeatingElementIsEnabled();
            thermostat.TemperatureSetpoint = MockoonTemperature - Offset;

            thermostat.Work();

            Assert.True(heatingElement.Enabled);
        }

        [Fact]
        public void HeaterIsNotTurnedOffWhenFailingToGetTemperatureMaxNumberOfTimes()
        {
            GivenHeatingElementIsEnabled();
            temperatureSensor.Url = BadUrl;

            for (int i = 0; i < MaxFailures; ++i)
            {
                thermostat.Work();
            }

            Assert.True(heatingElement.Enabled);
        }

        [Fact]
        public void HeaterIsTurnedOffWhenFailingToGetTemperatureTooManyTimes()
        {
            GivenHeatingElementIsEnabled();
            GivenMaxTemperatureSensorFailures();

            Assert.False(heatingElement.Enabled);
        }

        [Fact]
        public void HeaterCanBeTurnedOnAfterRecoveringFromTemperatureSensorFailure()
        {
            GivenHeatingElementIsEnabled();
            GivenMaxTemperatureSensorFailures();
            temperatureSensor.Url = MockoonUrl;

            thermostat.Work();

            Assert.True(heatingElement.Enabled);
        }

        [Fact]
        public void HeaterIsNotTurnedOffWhenFailingToGetTemperatureMaxNumberOfTimesAfterRecoveringFromFailure()
        {
            GivenHeatingElementIsEnabled();
            GivenRecoveredFromTemperatureSensorFailures();
            temperatureSensor.Url = BadUrl;

            for (int i = 0; i < MaxFailures; ++i)
            {
                thermostat.Work();
            }

            Assert.True(heatingElement.Enabled);
        }

        private void GivenHeatingElementIsEnabled()
        {
            thermostat.TemperatureSetpoint = MockoonTemperature + Offset + .5;
            thermostat.Work();
        }

        private void GivenMaxTemperatureSensorFailures()
        {
            temperatureSensor.Url = BadUrl;

            for (int i = 0; i <= MaxFailures; ++i)
            {
                thermostat.Work();
            }
        }

        private void GivenRecoveredFromTemperatureSensorFailures()
        {
            GivenMaxTemperatureSensorFailures();
            temperatureSensor.Url = MockoonUrl;
            thermostat.Work();
        }
    }
}
