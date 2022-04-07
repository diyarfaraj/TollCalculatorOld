using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TollCalculator.Interface;

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
        public int GetTollFee(DateTime date, IVehicle vehicle)
        {
            if (vehicle.IsTollFree()) return 0;

            if (IsBetween(date, "06:00", "06:29")) return 8;
            if (IsBetween(date, "06:30", "06:59")) return 13;
            if (IsBetween(date, "07:00", "07:59")) return 18;
            if (IsBetween(date, "08:00", "08:29")) return 13;
            if (IsBetween(date, "08:30", "14:59")) return 8;
            if (IsBetween(date, "15:00", "15:29")) return 13;
            if (IsBetween(date, "15:30", "16:59")) return 18;
            if (IsBetween(date, "17:00", "17:59")) return 13;
            if (IsBetween(date, "18:00", "18:29")) return 8;
            if (IsBetween(date, "18:30", "05:59")) return 0;

            return 0;
        }

        private bool IsBetween(DateTime entryTime, string startTime, string endTime)
        {
            TimeSpan start = TimeSpan.Parse(startTime);
            TimeSpan end = TimeSpan.Parse(endTime);
            TimeSpan now = entryTime.TimeOfDay;

            return (now >= start) && (now <= end);
        }

        private async Task<bool> IsTollFreeDate(DateTime date)
        {
            try
            {
                var holidays = await _holidayService.GetHolidays();
                var holidayDates = holidays.Select(day => day).ToList();
                var daysBoforeHoliday = await _holidayService.GetDayBeforeHoliday();

                if ((date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) ||
                (holidayDates.Contains(date) || daysBoforeHoliday.Contains(date))) return true;

                return false;
            }
            catch (Exception e)
            {
                //Add proper exception handling
                Console.WriteLine(e.Message);
                return false;
            }
           
        }
    }
}