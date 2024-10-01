using System.Diagnostics.Contracts;

namespace Heater
{
    public class Thermostat
    {
        private readonly IHeatingElement heatingElement;
        private readonly ITemperatureSensor temperatureSensor;
        private int failures;

        public double TemperatureSetpoint { get; set; } = 20.0;
        public double TemperatureOffset { get; set; } = .5;
        public int MaxTemperatureFailures { get; set; } = 3;

        public Thermostat(IHeatingElement heatingElement, ITemperatureSensor temperatureSensor)
        {
            Contract.Requires(heatingElement != null);
            Contract.Requires(temperatureSensor != null);

            this.heatingElement = heatingElement;
            this.temperatureSensor = temperatureSensor;
            failures = 0;
        }

        public void Work()
        {
            double temperature;
            try
            {
                temperature = temperatureSensor.Temperature();
            }
            catch
            {
                if (MaxTemperatureFailures < ++failures)
                {
                    heatingElement.Disable();
                }
                return;
            }

            failures = 0;
            if ((temperature + TemperatureOffset) < TemperatureSetpoint)
            {
                heatingElement.Enable();
            }
            else if ((TemperatureSetpoint + TemperatureOffset) < temperature)
            {
                heatingElement.Disable();
            }
        }
    }
}
