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
using Newtonsoft.Json;
using WolfTheatres.Classes;
using System.Web.Http.Cors;


namespace WolfTheatres.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TheMovieDatabaseController : ApiController
    {
        private const string API_KEY = "47d8e43fee40e68d6925f138a2d3e7f0";

        [System.Web.Http.HttpGet]
        public List<MovieSearchResult> Search(string query)
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format("http://api.themoviedb.org/3/search/movie?query={0}&api_key={1}", query, API_KEY));
            request.Method = "GET";
            request.Accept = "application/json";
            request.Headers.Add("Accept-Charset", "UTF-8");
            request.ContentLength = 0;
            string json;

            var movies = new List<MovieSearchResult>();

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (null != response && HttpStatusCode.OK == response.StatusCode)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        json = reader.ReadToEnd();

                        Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JObject.Parse(json);

                        foreach (var result in token.SelectToken("results"))
                        {
                            movies.Add(new MovieSearchResult
                            {
                                MovieDbId = (int)result["id"],
                                Name = (string)result["title"],
                                Year = (string)result["release_date"]
                            });
                        }
                    }
                }
            }

            return movies;
        }

        [System.Web.Http.HttpGet]
        public Movie GetMovieDetails(int movieId)
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format("http://api.themoviedb.org/3/movie/{0}?api_key={1}&append_to_response=releases,videos", movieId, API_KEY));
            request.Method = "GET";
            request.Accept = "application/json";
            request.Headers.Add("Accept-Charset", "UTF-8");
            request.ContentLength = 0;
            string json;

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (null != response && HttpStatusCode.OK == response.StatusCode)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        json = reader.ReadToEnd();

                        Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JObject.Parse(json);

                        var movie = new Movie();
                        movie.GetMovieInformationFromJson(token);

                        return movie;
                    }
                }
            }
            return null;
        }

        [System.Web.Http.HttpGet]
        public List<MovieSearchResult> GetUpcomingMovies()
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format("http://api.themoviedb.org/3/movie/upcoming?api_key={0}", API_KEY));

            request.Method = "GET";
            request.Accept = "application/json";
            request.Headers.Add("Accept-Charset", "UTF-8");
            request.ContentLength = 0;
            string json;

            var movies = new List<MovieSearchResult>();

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (null != response && HttpStatusCode.OK == response.StatusCode)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        json = reader.ReadToEnd();

                        Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JObject.Parse(json);

                        foreach (var result in token.SelectToken("results"))
                        {
                            movies.Add(new MovieSearchResult
                            {
                                MovieDbId = (int)result["id"],
                                Name = (string)result["title"],
                                Year = (string)result["release_date"]
                            });
                        }
                    }
                }
            }

            return movies;

        }
    }
}
