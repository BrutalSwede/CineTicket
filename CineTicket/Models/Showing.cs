using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineTicket.Models
{
    public class Showing
    {
        public int ID { get; set; }
        public int MovieID { get; set; }
        public int SalonID { get; set; }
        public DateTime Date { get; set; }

        public Movie Movie { get; set; }
        public Salon Salon { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
