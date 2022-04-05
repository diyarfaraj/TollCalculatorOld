using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TollCalculator.Interface;
using TollCalculator.Models;

namespace TollCalculator
{
    public class TollCalculator
    {
        private readonly IHolidayService _holidayService;
        public TollCalculator(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total toll fee for that day
         */

        public int GetVehicleTollFee(IVehicle vehicle, DateTime[] dates)
        {
            DateTime intervalStart = dates[0];
            int totalFee = 0;
            foreach (DateTime date in dates)
            {
                int nextFee = GetTollFee(date,vehicle);
                int tempFee = GetTollFee(intervalStart, vehicle);

                long diffInMillies = date.Millisecond - intervalStart.Millisecond;
                long minutes = diffInMillies / 1000 / 60;

                if (minutes <= 60)
                {
                    if (totalFee > 0) totalFee -= tempFee;
                    if (nextFee >= tempFee) tempFee = nextFee;
                    totalFee += tempFee;
                }
                else
                {
                    totalFee += nextFee;
                }
            }
            if (totalFee > 60) totalFee = 60;
            return totalFee;
        }

        private bool IsTollFreeVehicle(IVehicle vehicle)
        {
            if (vehicle == null) return false;
            string vehicleType = vehicle.GetVehicleType();
            return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Military.ToString());
        }

        public int GetTollFee(DateTime date, IVehicle vehicle)
        {
            if (IsTollFreeDate(date).Result || IsTollFreeVehicle(vehicle)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        private async Task<bool> IsTollFreeDate(DateTime date)
        {
            var holidays = await _holidayService.GetHolidays();
            var holidayDates = holidays.Select(day => day.Date);
            var daysBoforeHoliday = await _holidayService.GetDayBeforeHoliday();

            if ((date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) ||
            (holidayDates.Contains(date) || daysBoforeHoliday.Contains(date))) return true;

            return false;
        }

        private enum TollFreeVehicles
        {
            Motorbike = 0,
            Tractor = 1,
            Emergency = 2,
            Diplomat = 3,
            Foreign = 4,
            Military = 5
        }
    }
}