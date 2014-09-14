using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.IO;
using WolfTheatres.Database;
using System.Web.Http.Cors;

namespace WolfTheatres.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PosterController : ApiController
    {
        private const string POSTER_IMG_LOCATION_PROJECT = "~/Movies/Posters/";
        private const string POSTER_IMG_LOCATION_WEB = "http://localhost:55400/Movies/Posters/";

        private WolfTheatresContext db = new WolfTheatresContext();

        public HttpResponseMessage Post()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var posters = new List<Poster>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath(POSTER_IMG_LOCATION_PROJECT + postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    var fileLocation = POSTER_IMG_LOCATION_WEB + postedFile.FileName;
                    var movieId = Guid.Parse(httpRequest.Form["movieId"]);

                    var poster = new Poster
                    {
                        PosterId = Guid.NewGuid(),
                        FileLocation = fileLocation,
                        MovieId = movieId
                    };

                    db.Posters.Add(poster);
                    posters.Add(poster);
                }
                db.SaveChanges();
                result = Request.CreateResponse(HttpStatusCode.Created, posters);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }
    }
}
