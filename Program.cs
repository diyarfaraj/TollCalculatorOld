using System;
using System.Collections.Generic;
using System.Net.Http;
using TollCalculator.Interface;
using TollCalculator.Services;
using TollCalculator.Vehicles;

namespace TollCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<DateTime> datesList = new List<DateTime>();
            datesList.Add(DateTime.Now);
            DateTime[] dates = datesList.ToArray();
            HttpClient httpClient = new HttpClient();
            HolidayService hs = new HolidayService(httpClient);
            var mcal = new TollCalculator(hs);
            var car = new Car();
            mcal.GetVehicleTollFee(car, dates);
        }
    }
}
