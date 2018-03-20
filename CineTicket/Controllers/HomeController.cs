using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CineTicket.Models;
using CineTicket.Data;

namespace CineTicket.Controllers
{
    public class HomeController : Controller
    {

        private readonly CinemaContext _context;

        public HomeController(CinemaContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var showings = await _context.Showings.Include(s => s.Movie).Include(s => s.Salon).Include(s => s.Bookings).ToListAsync();

            List<ShowingViewModel> viewModels = new List<ShowingViewModel>();

            foreach (var showing in showings)
            {
                viewModels.Add(new ShowingViewModel()
                {
                    ID = showing.ID,
                    MovieTitle = showing.Movie.Title,
                    SalonName = showing.Salon.Name,
                    Date = showing.Date,
                    RemainingSeats = showing.Salon.MaxSeats - showing.Bookings.Sum(b => b.NumberOfSeats)
                });
            }

            return View(viewModels);
        }

        public async Task<IActionResult> BookTicket(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showing = await _context.Showings.Include(s => s.Bookings).Include(s => s.Movie).Include(s => s.Salon).SingleOrDefaultAsync(s => s.ID == id);

            if (showing == null)
            {
                return NotFound();
            }

            var showingVM = new ShowingViewModel
            {
                ID = showing.ID,
                MovieTitle = showing.Movie.Title,
                SalonName = showing.Salon.Name,
                Date = showing.Date,
                RemainingSeats = showing.Salon.MaxSeats - showing.Bookings.Sum(b => b.NumberOfSeats),
                MovieRunningMinutes = showing.Movie.RunningMinutes
            };

            ViewData["MaxRemainingSeats"] = showingVM.RemainingSeats < 12 ? showingVM.RemainingSeats : 12;

            return View(showingVM);
        }

        [HttpPost]
        public async Task<IActionResult> BookTicket([Bind("NumberOfSeats,ShowingID")] Booking booking)
        {

            // Do checks here to see if the tickets are still available

            if(ModelState.IsValid)
            {
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // CHANGE LATER: Redirect to booking confirmation window instead
            }

            return View(Error());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
