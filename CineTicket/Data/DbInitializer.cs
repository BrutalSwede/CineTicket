using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineTicket.Models;

namespace CineTicket.Data
{
    public class DbInitializer
    {
        public static void Initialize(CinemaContext context)
        {
            context.Database.EnsureCreated();

            if (context.Movies.Any())
            {
                return; //Db is already seeded.
            }

            var movies = new Movie[]
            {
                new Movie {Title="Lord of the Rings: Return of the King", Info="Extended cut!", RunningMinutes=263},
                new Movie {Title="Hannibal", Info="The classic horror film", RunningMinutes=131},
                new Movie {Title="Finding Nemo", Info="For all ages", RunningMinutes=100}
            };
            foreach (Movie m in movies)
            {
                context.Movies.Add(m);
            }

            context.SaveChanges();

            var salons = new Salon[]
            {
                new Salon {Name="Red Salon", MaxSeats=75},
                new Salon {Name="Blue Salon", MaxSeats=50},
                new Salon {Name="Yellow Submarine", MaxSeats=45}
            };
            foreach(Salon s in salons)
            {
                context.Salons.Add(s);
            }

            context.SaveChanges();

            var showings = new Showing[]
            {
                new Showing {MovieID=1, SalonID=1, Date=DateTime.Today},
                new Showing {MovieID=2, SalonID=2, Date=DateTime.Today},
                new Showing {MovieID=3, SalonID=3, Date=DateTime.Today}
            };
            foreach(Showing s in showings)
            {
                context.Showings.Add(s);
            }
            context.SaveChanges();

            var bookings = new Booking[]
            {
                new Booking { NumberOfSeats=2, ShowingID=2},
                new Booking {NumberOfSeats=3, ShowingID=1},
                new Booking {NumberOfSeats=1, ShowingID=1},
                new Booking {NumberOfSeats=5, ShowingID=3}
            };
            foreach(Booking b in bookings)
            {
                context.Bookings.Add(b);
            }
            context.SaveChanges();

        }
    }
}
