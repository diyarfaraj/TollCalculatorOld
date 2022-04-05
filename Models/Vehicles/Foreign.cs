using TollCalculator.Interface;

namespace TollCalculator.Vehicles
{
    public class Foreign : IVehicle
    {
        public string GetVehicleType() => nameof(Foreign);

        public bool IsTollFree() => true;
    }
}