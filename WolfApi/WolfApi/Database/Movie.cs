using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WolfTheatres.Database
{
    public class Movie
    {
        public Movie()
        {
            this.MovieId = Guid.NewGuid();
        }

        public Guid MovieId { get; set; }
        public int MovieDbId { get; set; }
        public string ImdbId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RunTime { get; set; }
        public string Rating { get; set; }
        public string Year { get; set; }
        public string Cast { get; set; }
        public virtual List<Poster> Posters { get; set; }
        public virtual List<Trailer> Trailers { get; set; }
        public virtual List<MovieShowtime> MovieShowtimes { get; set; }

        public void GetMovieInformationFromJson(Newtonsoft.Json.Linq.JToken movieInformation)
        {
            this.MovieDbId = (int)movieInformation["id"];
            this.ImdbId = (string)movieInformation["imdb_id"];
            this.Name = (string)movieInformation["title"];
            this.Description = (string)movieInformation["overview"];
            this.RunTime = (string)movieInformation["runtime"];
            this.Rating = (string)movieInformation["releases"]["countries"][0]["certification"];
            this.Year = (string)movieInformation["release_date"];


            var imageUrl = "";

            if (!string.IsNullOrEmpty(movieInformation["poster_path"].ToString()))
                imageUrl = "http://image.tmdb.org/t/p/original" + movieInformation["poster_path"];


            this.Posters = new List<Poster>{
                new Poster {
                    PosterId = Guid.NewGuid(),
                    ImageUrl =  imageUrl,
                    MovieId = this.MovieId
                }
           };

            this.Trailers = new List<Trailer>();

            foreach (var trailer in movieInformation["videos"]["results"])
            {
                this.Trailers.Add(new Trailer
                {
                    TrailerId = Guid.NewGuid(),
                    TrailerUrl = "http://www.youtube.com/v/" + trailer["key"],
                    MovieId = this.MovieId
                });
            }
        }
    }
}
