using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineTicket.Models
{
    public class Booking
    {
        public int ID { get; set; }
        public int NumberOfSeats { get; set; }
        public int ShowingID { get; set; }

        public Showing Showing { get; set; }
    }
}
