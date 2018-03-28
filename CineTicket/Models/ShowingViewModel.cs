using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineTicket.Models
{
    public class ShowingViewModel
    {
        public int ID { get; set; }

        public int RemainingSeats { get; set; }
        public int MovieID { get; set; }
        public string MovieTitle { get; set; }
        public int MovieRunningMinutes { get; set; }
        public string SalonName { get; set; }
        public DateTime Date { get; set; }

    }
}
