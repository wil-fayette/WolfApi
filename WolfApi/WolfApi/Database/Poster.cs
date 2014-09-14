using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WolfTheatres.Database
{
    public class Poster
    {
        public Poster()
        {
            this.PosterId = Guid.NewGuid();
        }

        public Guid PosterId { get; set; }
        public string ImageUrl { get; set; }
        public string FileLocation { get; set; }
        public Guid MovieId { get; set; }
    }
}