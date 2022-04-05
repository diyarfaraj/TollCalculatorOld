using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator.Interface
{
    public interface IVehicle
    {
        string GetVehicleType();
        bool IsTollFree();
    }
}
