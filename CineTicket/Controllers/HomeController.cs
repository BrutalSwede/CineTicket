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


        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewBag.DateSort = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.SeatSort = sortOrder == "seats_asc" ? "seats_desc" : "seats_asc";


            var showings = await _context.Showings.Where(s => s.Date > DateTime.Now).Include(s => s.Movie).Include(s => s.Salon).Include(s => s.Bookings).ToListAsync();

            List<ShowingViewModel> viewModels = new List<ShowingViewModel>();

            foreach (var showing in showings)
            {
                viewModels.Add(new ShowingViewModel()
                {
                    ID = showing.ID,
                    MovieID = showing.MovieID,
                    MovieTitle = showing.Movie.Title,
                    SalonName = showing.Salon.Name,
                    Date = showing.Date,
                    RemainingSeats = showing.Salon.MaxSeats - showing.Bookings.Sum(b => b.NumberOfSeats)
                });
            }

            

            switch (sortOrder)
            {
                case "date_desc":
                    viewModels = viewModels.OrderByDescending(s => s.Date).ToList();
                    break;
                case "seats_asc":
                    viewModels = viewModels.OrderBy(s => s.RemainingSeats).ToList();
                    break;
                case "seats_desc":
                    viewModels = viewModels.OrderByDescending(s => s.RemainingSeats).ToList();
                    break;
                default:
                    viewModels = viewModels.OrderBy(s => s.Date).ToList();
                    break;
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
            if(ModelState.IsValid)
            {

                if (booking.NumberOfSeats < 0)
                    return RedirectToAction(nameof(Error));

                var showing = await _context.Showings.Where(s => s.ID == booking.ShowingID).Include(s => s.Bookings).Include(s => s.Salon).Include(s => s.Movie).SingleOrDefaultAsync();

                int remainingSeats = showing.Salon.MaxSeats - showing.Bookings.Sum(b => b.NumberOfSeats);

                if (booking.NumberOfSeats > remainingSeats)
                    return RedirectToAction(nameof(Error)); // Make custom view for order errors?

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                var bookingVM = new BookingViewModel()
                {
                    ID = booking.ID,
                    MovieTitle = showing.Movie.Title,
                    SalonName = showing.Salon.Name,
                    NumberOfSeats = booking.NumberOfSeats,
                    Date = booking.Showing.Date
                };

                return View("Booking", bookingVM);
            }

            return RedirectToAction(nameof(Error));
        }

        public async Task<IActionResult> Booking(Booking booking)
        {

            if(booking == null)
            {
                return NotFound();
            }

            var bookingExtendedInfo = await _context.Bookings.Where(b => b.ID == booking.ID).Include(b => b.Showing).ThenInclude(s => s.Movie).SingleOrDefaultAsync();

            if(bookingExtendedInfo == null)
            {
                return NotFound();
            }

            return View(bookingExtendedInfo);
        }

        public async Task<IActionResult> Movie(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Movie movie = await _context.Movies.Where(m => m.ID == id).SingleOrDefaultAsync();

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
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
