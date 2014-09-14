using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WolfTheatres.Database
{
    public class MovieShowtime
    {
        public MovieShowtime()
        {
            this.MovieShowtimeId = Guid.NewGuid();
        }

        public Guid MovieShowtimeId { get; set; }
        public string Showtimes { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string MovieName { get; set; }
        public Guid MovieId { get; set; }
    }
}