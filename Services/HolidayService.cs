using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TollCalculator.Interface;
using TollCalculator.Models;

namespace TollCalculator.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly HttpClient _httpClient;
        public HolidayService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async  Task<List<DateTime>> GetDayBeforeHoliday()
        {
            List<DateTime> preHolidays = new List<DateTime>();
            var holidays = await GetHolidays();
            foreach (var day in holidays)
            {
                preHolidays.Add(day.Date.AddDays(-1));
            }
            return preHolidays;

        }

        public async Task<List<Holiday>> GetHolidays()
        {
            var apiKey = ConfigurationManager.AppSetting["ApiKey"];
            var countryCode = ConfigurationManager.AppSetting["CountryCode"];
            var currentYear = DateTime.Now.ToString("yy");
            string URL = string.Format("https://calendarific.com/api/v2/holidays?&api_key={0}&country={1}&year={2}", apiKey, countryCode, currentYear);

            try
            {
                List<Holiday> holidays;
                _httpClient.BaseAddress = new Uri(URL);
                HttpResponseMessage response = await _httpClient.GetAsync("");
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("api call failed");
                }
                holidays = response.Content.ReadAsAsync<List<Holiday>>().Result;

                return holidays;
            }
            catch (Exception)
            {

                throw new HttpRequestException("api failed");
            }
        }
    }
}
