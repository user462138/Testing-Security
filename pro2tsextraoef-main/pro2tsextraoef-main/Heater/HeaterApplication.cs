namespace Heater
{
    public class HeaterApplication
    {
        public PretendHeatingElement HeatingElement { get; private set; } = new PretendHeatingElement();
        public OpenWeatherTemperatureSensor TemperatureSensor { get; private set; } = new OpenWeatherTemperatureSensor();
        private readonly Thermostat thermostat;

        public HeaterApplication()
        {
            thermostat = new Thermostat(HeatingElement, TemperatureSensor)
            {
                TemperatureSetpoint = 20.0,
                TemperatureOffset = 1.0,
                MaxTemperatureFailures = 3
            };
        }

        public void Loop()
        {
            for (;;)
            {
                thermostat.Work();
                Thread.Sleep(1000);
            }
        }
    }
}
