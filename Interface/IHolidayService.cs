using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TollCalculator.Models;

namespace TollCalculator.Interface
{
    public interface IHolidayService
    {
        Task<List<Holiday>> GetHolidays();
        Task<List<DateTime>> GetDayBeforeHoliday();
    }
}
