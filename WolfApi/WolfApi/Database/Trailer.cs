using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WolfTheatres.Database
{
    public class Trailer
    {
        public Trailer()
        {
            this.TrailerId = Guid.NewGuid();
        }

        public Guid TrailerId { get; set; }
        public string TrailerUrl { get; set; }
        public Guid MovieId { get; set; }
    }
}