using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WolfTheatres.Database;
using System.Web.Http.Cors;

namespace WolfTheatres.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImageController : ApiController
    {
        private const string IMG_LOCATION_PROJECT = "~/Images/";
        private const string IMG_LOCATION_WEB = "http://localhost:55400/Images/";

        private WolfTheatresContext db = new WolfTheatresContext();

        public HttpResponseMessage Post()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var posters = new List<Image>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var imageId = Guid.NewGuid();
                    var imageExtension = System.IO.Path.GetExtension(postedFile.FileName);
                    var imageName = imageId + imageExtension;
                    var filePath = HttpContext.Current.Server.MapPath(IMG_LOCATION_PROJECT + imageName);

                    postedFile.SaveAs(filePath);

                    var fileLocation = IMG_LOCATION_WEB + imageName;
                    var page = httpRequest.Form["page"];

                    var poster = new Image
                    {
                        ImageId = imageId,
                        FileLocation = fileLocation,
                        Page = page
                    };

                    db.Images.Add(poster);
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

        public List<Image> Get(string page)
        {
            return db.Images.Where(x => x.Page == page.Trim()).ToList();
        }

        public int Delete(Guid imageId)
        {
            try
            {
                var imageToDelete = db.Images.Find(imageId);

                if (imageToDelete == null)
                    return 0;

                var filePath = imageToDelete.FileLocation.Remove(0, 23);

                if (File.Exists(HttpContext.Current.Server.MapPath(filePath)))
                    File.Delete((HttpContext.Current.Server.MapPath(filePath)));

                db.Images.Remove(imageToDelete);
                db.SaveChanges();

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
