using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;
using WolfTheatres.Database;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http.Cors;



namespace WolfTheatres.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MovieController : ApiController
    {
        private const string POSTER_IMAGE_LOCATION = "Movies/Posters/";
        private TheMovieDatabaseController movieDatabase = new TheMovieDatabaseController();

        private WolfTheatresContext db = new WolfTheatresContext();

        public IEnumerable<Movie> GetMovies()
        {
            return db.Movies.AsEnumerable();
        }

        public Movie GetMovie(int id)
        {
            Movie movie = db.Movies.Find(id);

            return movie;
        }

        [System.Web.Http.HttpPost]
        public Movie AddNewMovie(int movieDbId)
        {
            Movie movie = movieDatabase.GetMovieDetails(movieDbId);

            foreach (var poster in movie.Posters)
                GetPosterImage(poster);

            db.Movies.Add(movie);
            db.SaveChanges();

            return movie;
        }

        [System.Web.Http.HttpPost]
        public Movie UpdateMovie(Movie movie)
        {

            if (!ModelState.IsValid)
                return null;

            db.Entry(movie).State = EntityState.Modified;

            foreach (var poster in movie.Posters)
                db.Entry(poster).State = EntityState.Modified;


            foreach (var trailer in movie.Trailers)
                db.Entry(trailer).State = EntityState.Modified;

            db.SaveChanges();

            return movie;
        }


        private void GetPosterImage(Poster poster)
        {
            poster.PosterId = Guid.NewGuid();
            var webClient = new WebClient();

            if (!string.IsNullOrEmpty(poster.ImageUrl))
            {
                webClient.DownloadFile(poster.ImageUrl, HttpContext.Current.Server.MapPath("~/" + POSTER_IMAGE_LOCATION) + poster.MovieId + ".jpg");
                poster.FileLocation = "http://localhost:55400/" + POSTER_IMAGE_LOCATION + poster.MovieId + ".jpg";
            }
        }

        [System.Web.Http.HttpDelete]
        public int DeleteMovieTrailer(Guid trailerId)
        {
            var trailer = db.Trailers.Find(trailerId);

            if (trailer == null)
                return 0;

            db.Trailers.Remove(trailer);
            db.SaveChanges();

            return 1;
        }

        [System.Web.Http.HttpPost]
        public Trailer AddMovieTrailer(Trailer trailer)
        {
            if (!trailer.TrailerUrl.Contains("https://") && !trailer.TrailerUrl.Contains("http://"))
                return null;

            Uri url;
            if (Uri.TryCreate(trailer.TrailerUrl, UriKind.RelativeOrAbsolute, out url))
            {
                trailer.TrailerId = Guid.NewGuid();
                db.Trailers.Add(trailer);
                db.SaveChanges();
            }

            return trailer;
        }

        [System.Web.Http.HttpDelete]
        public void DeleteMovie(Guid movieId)
        {
            Movie movie = db.Movies.Find(movieId);
            if (movie != null)
            {

                DeleteMoviePosters(movie.Posters);
                DeleteMovieTrailers(movie.Trailers);
                DeleteMovieShowtimes(movie.MovieShowtimes);

                db.Movies.Remove(movie);
                db.SaveChanges();
            }
        }

        private void DeleteMovieShowtimes(List<MovieShowtime> showtimes)
        {
            if (showtimes.Count > 0)
            {
                foreach (var showtime in showtimes.ToList())
                {
                    this.DeleteMovieShowtime(showtime.MovieShowtimeId);
                }
            }
        }

        private void DeleteMovieShowtime(Guid showtimeId)
        {
            var databaseShowtime = db.MovieShowtimes.Find(showtimeId);
            if (databaseShowtime != null)
                db.MovieShowtimes.Remove(databaseShowtime);
        }

        public List<MovieShowtime> GetMovieShowtimes()
        {
            return db.MovieShowtimes.ToList();
        }

        public List<MovieShowtime> SaveMovieShowtimes(List<MovieShowtime> showtimes)
        {

            var oldMovieShowtimes = db.MovieShowtimes.ToList();

            foreach (var oldMovieShowtime in oldMovieShowtimes)
            {
                db.MovieShowtimes.Remove(oldMovieShowtime);
            }

            foreach (var showtime in showtimes)
            {
                if (showtime.MovieShowtimeId == null)
                    showtime.MovieShowtimeId = new Guid();

                db.MovieShowtimes.Add(showtime);
            }

            db.SaveChanges();

            return db.MovieShowtimes.ToList();
        }

        private void DeleteMovieTrailers(List<Trailer> trailers)
        {
            if (trailers.Count > 0)
            {
                foreach (var trailer in trailers.ToList())
                {
                    this.DeleteMovieTrailer(trailer.MovieId);
                }
            }
        }

        private void DeleteMoviePosters(List<Poster> posters)
        {
            if (posters.Count > 0)
            {
                foreach (var poster in posters.ToList())
                {
                    this.DeleteMoviePoster(poster.PosterId);
                }
            }
        }

        public int DeleteMoviePoster(Guid posterId)
        {
            try
            {
                var poster = db.Posters.Find(posterId);

                if (poster == null)
                    return 0;

                var filePath = poster.FileLocation.Remove(0, 23);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                db.Posters.Remove(poster);
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}