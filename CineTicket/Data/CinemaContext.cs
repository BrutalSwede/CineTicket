using Microsoft.EntityFrameworkCore;
using CineTicket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineTicket.Data
{
    public class CinemaContext : DbContext
    {
        public CinemaContext(DbContextOptions<CinemaContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Showing> Showings { get; set; }
        public DbSet<Salon> Salons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().ToTable("Movies");
            modelBuilder.Entity<Booking>().ToTable("Bookings");
            modelBuilder.Entity<Showing>().ToTable("Showings");
            modelBuilder.Entity<Salon>().ToTable("Salons");

            modelBuilder.Entity<Booking>().HasOne(b => b.Showing).WithMany(s => s.Bookings).HasForeignKey(b => b.ShowingID);
            modelBuilder.Entity<Salon>().HasMany(s => s.Showings).WithOne(sh => sh.Salon).HasForeignKey(sh => sh.SalonID);
            modelBuilder.Entity<Movie>().HasMany(m => m.Showings).WithOne(s => s.Movie).HasForeignKey(s => s.MovieID);
        }
    }
}
