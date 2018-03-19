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
            var showings = _context.Showings.Include(s => s.Movie).Include(s => s.Salon).Include(s => s.Bookings);
            return View(await showings.ToListAsync());
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
