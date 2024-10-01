namespace Heater
{
    public class PretendHeatingElement : IHeatingElement
    {
        public bool Enabled { get; set; } = false;

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }
    }
}
