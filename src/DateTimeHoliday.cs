using System;

namespace Afonsoft.Date
{
    public class DateTimeHoliday
    {
        public string Description { get; set; }

        public string Country { get; set; } = "BR";

        public DateTime Holiday{ get; set; }

        public DateTimeHoliday(DateTime holiday, string description)
        {
            Description = description;
            Holiday = holiday;
        }
    }
}
