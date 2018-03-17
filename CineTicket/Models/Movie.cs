using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineTicket.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int RunningMinutes { get; set; }
        public string Info { get; set; }

        public ICollection<Showing> Showings { get; set; }
    }
}
