using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineTicket.Models
{
    public class BookingViewModel
    {

        public BookingViewModel(){ }

        public int ID { get; set; }
        public int NumberOfSeats { get; set; }
        public string MovieTitle { get; set; }
        public string SalonName { get; set; }
        public DateTime Date { get; set; }

    }
}
