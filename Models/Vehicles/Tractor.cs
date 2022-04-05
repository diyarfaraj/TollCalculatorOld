using TollCalculator.Interface;

namespace TollCalculator.Vehicles
{
    public class Tractor : IVehicle
    {
        public string GetVehicleType() => nameof(Tractor);

        public bool IsTollFree() => true;
    }
}