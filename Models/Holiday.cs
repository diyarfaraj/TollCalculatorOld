using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator.Models
{
    public class Meta
    {
        public int Code { get; set; }
    }

    public class Country
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Datetime
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int? Hour { get; set; }
        public int? Minute { get; set; }
        public int? Second { get; set; }
    }

    public class Timezone
    {
        public string Offset { get; set; }
        public string Zoneabb { get; set; }
        public int Zoneoffset { get; set; }
        public int Zonedst { get; set; }
        public int Zonetotaloffset { get; set; }
    }

    public class Date
    {
        public Object Iso { get; set; }
        public Datetime Datetime { get; set; }
        public Timezone Timezone { get; set; }
    }

    public class Holiday
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Country Country { get; set; }
        public Date Date { get; set; }
        public List<string> Type { get; set; }
        public string Locations { get; set; }
        public string States { get; set; }
    }

    public class Response
    {
        public List<Holiday> Holidays { get; set; }
    }

    public class Root
    {
        public Meta Meta { get; set; }
        public Response Response { get; set; }
    }
}
