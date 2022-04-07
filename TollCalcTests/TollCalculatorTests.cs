using System;
using Xunit;
using Moq;
using TollCalculator.Interface;
using TollCalculator.Vehicles;

namespace TollCalculator.Tests
{
    using static TollCalculatorTestHarness;
    public class TollCalculator_GetVehicleTollFee
    {
        [Fact]
        public void ReturnFee_CarIsPassing()
        {
            var sut = CreateSut();
            var result = sut.GetVehicleTollFee(DefaultCar(), DefaultTimes);
            Assert.True(result == 13);
        }

        [Fact]
        public void ReturnNoFee_DiplomatIsPassing()
        {
            var sut = CreateSut();
            var result = sut.GetVehicleTollFee(DefaultDiplomat(), DefaultTimes);
            Assert.True(result == 0);
        }

        [Fact]
        public void Return60min_EntryExceeds60min()
        {
            var sut = CreateSut();
            var result = sut.GetVehicleTollFee(DefaultCar(), DefaultTimesOver60Min);
            Assert.True(result == 60);
        }

    }

    public static class TollCalculatorTestHarness
    {
        public static TollCalculator CreateSut(IHolidayService holidayService = null) => new TollCalculator(holidayService ?? Mock.Of<IHolidayService>());

        public static Car DefaultCar() => new Car();
        public static Diplomat DefaultDiplomat() => new Diplomat();

        public static DateTime[] DefaultTimes = new[]
        {
                DateTime.Parse("2022-03-03 17:54:00"),
                DateTime.Parse("2022-06-04 17:34:00"),
                DateTime.Parse("2022-04-04 17:44:00"),
                DateTime.Parse("2022-10-01 02:34:00"),
                DateTime.Parse("2022-12-21 08:23:00"),
                DateTime.Parse("2022-11-21 08:01:00"),
            };

        public static DateTime[] DefaultTimesOver60Min = new[]
      {
                DateTime.Parse("2022-03-03 07:34:00"),
                DateTime.Parse("2022-03-03 07:44:00"),
                DateTime.Parse("2022-03-03 07:54:00"),
                DateTime.Parse("2022-03-03 08:01:00"),
                DateTime.Parse("2022-03-03 08:23:00"),
                DateTime.Parse("2022-03-03 08:31:00"),
            };

    }
}
