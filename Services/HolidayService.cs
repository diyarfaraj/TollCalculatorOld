using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

        public async Task<List<DateTime>> GetDayBeforeHoliday()
        {
            List<DateTime> preHolidays = new List<DateTime>();
            var holidays = await GetHolidays();
            foreach (var day in holidays)
            {
                preHolidays.Add(day.AddDays(-1));
            }
            return preHolidays;

        }

        public async Task<List<DateTime>> GetHolidays()
        {
            var apiKey = ConfigurationManager.AppSetting["HolidayApiSettings:ApiKey"];
            var countryCode = ConfigurationManager.AppSetting["HolidayApiSettings:CountryCode"];
            var currentYear = DateTime.Now.ToString("yyyy");
            string URL = string.Format("https://calendarific.com/api/v2/holidays?&api_key={0}&country={1}&year={2}", apiKey, countryCode, currentYear);

            List<Holiday> holidays;
            _httpClient.BaseAddress = new Uri(URL);
            HttpResponseMessage response = await _httpClient.GetAsync("");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("api call failed");
            }
            var jsonString = await response.Content.ReadAsStringAsync();

            dynamic responseData = JsonConvert.DeserializeObject<Holiday>(jsonString);
            return responseData;

        }
    }
}
