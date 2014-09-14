using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WolfTheatres.Database
{
    public class Image
    {
        public Guid ImageId { get; set; }
        public string Page { get; set; }
        public string FileLocation { get; set; }
    }
}
