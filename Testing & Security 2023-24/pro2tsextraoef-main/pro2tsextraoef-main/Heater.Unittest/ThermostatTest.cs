using Moq;

namespace Heater.Unittest
{
    public class ThermostatTest
    {
        private const double Setpoint = 20.0;
        private const double Offset = 5.0;
        private const int MaxFailures = 2;

        private readonly Mock<IHeatingElement> heatingElement;
        private readonly Mock<ITemperatureSensor> temperatureSensor;
        private readonly Thermostat thermostat;

        public ThermostatTest()
        {
            heatingElement = new Mock<IHeatingElement>();
            temperatureSensor = new Mock<ITemperatureSensor>();
            thermostat = new Thermostat(heatingElement.Object, temperatureSensor.Object)
            {
                TemperatureSetpoint = Setpoint,
                TemperatureOffset = Offset,
                MaxTemperatureFailures = MaxFailures
            };
        }

        [Fact]
        public void HeaterIsTurnedOnWhenTemperatureIsBelowSetpointMinusOffset()
        {
            temperatureSensor.Setup(x => x.Temperature()).Returns(Setpoint - Offset - .5);

            thermostat.Work();

            heatingElement.Verify(x => x.Enable(), Times.Once);
        }

        [Fact]
        public void HeaterIsNotTurnedOnWhenTemperatureIsAboveSetpointMinusOffset()
        {
            temperatureSensor.Setup(x => x.Temperature()).Returns(Setpoint - Offset);

            thermostat.Work();

            heatingElement.Verify(x => x.Enable(), Times.Never);
        }

        [Fact]
        public void HeaterIsTurnedOffWhenTemperatureIsAboveSetpointPlusOffset()
        {
            temperatureSensor.Setup(x => x.Temperature()).Returns(Setpoint + Offset + .5);

            thermostat.Work();

            heatingElement.Verify(x => x.Disable(), Times.Once);
        }

        [Fact]
        public void HeaterIsNotTurnedOffWhenTemperatureIsBelowSetpointPlusOffset()
        {
            temperatureSensor.Setup(x => x.Temperature()).Returns(Setpoint + Offset);

            thermostat.Work();

            heatingElement.Verify(x => x.Disable(), Times.Never);
        }

        [Fact]
        public void HeaterIsNotTurnedOffOrOnWhenFailingToGetTemperatureMaxNumberOfTimes()
        {
            temperatureSensor.Setup(x => x.Temperature()).Throws<Exception>();

            for (int i = 0; i < MaxFailures; ++i)
            {
                thermostat.Work();
            }

            heatingElement.Verify(x => x.Disable(), Times.Never);
            heatingElement.Verify(x => x.Enable(), Times.Never);
        }

        [Fact]
        public void HeaterIsTurnedOffWhenFailingToGetTemperatureTooManyTimes()
        {
            GivenMaxTemperatureSensorFailures();

            heatingElement.Verify(x => x.Disable(), Times.Once);
        }

        [Fact]
        public void HeaterCanBeTurnedOnAfterRecoveringFromTemperatureSensorFailure()
        {
            GivenMaxTemperatureSensorFailures();
            temperatureSensor.Setup(x => x.Temperature()).Returns(Setpoint - Offset - .5);

            thermostat.Work();

            heatingElement.Verify(x => x.Enable(), Times.Once);
        }

        [Fact]
        public void HeaterIsNotTurnedOffWhenFailingToGetTemperatureMaxNumberOfTimesAfterRecoveringFromFailure()
        {
            GivenRecoveredFromTemperatureSensorFailures();
            temperatureSensor.Setup(x => x.Temperature()).Throws<Exception>();

            for (int i = 0; i < MaxFailures; ++i)
            {
                thermostat.Work();
            }

            heatingElement.Verify(x => x.Disable(), Times.Once); // once when failed too many times
            heatingElement.Verify(x => x.Enable(), Times.Once);  // once after recovering
        }

        private void GivenMaxTemperatureSensorFailures()
        {
            temperatureSensor.Setup(x => x.Temperature()).Throws<Exception>();

            for (int i = 0; i <= MaxFailures; ++i)
            {
                thermostat.Work();
            }
        }

        private void GivenRecoveredFromTemperatureSensorFailures()
        {
            GivenMaxTemperatureSensorFailures();
            temperatureSensor.Setup(x => x.Temperature()).Returns(Setpoint - Offset - .5);
            thermostat.Work();
        }
    }
}
