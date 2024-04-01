using Microsoft.AspNetCore.Mvc;
using MovieRecommendationApi.Dtos;
using MovieRecommendationApi.Models;
using Newtonsoft.Json;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

namespace MovieConsume.Controllers
{
    public class ConsumeController : Controller
    {
        private string localUrl = "https://localhost:7080";
        public IActionResult Index()

        {
            List<ReadMovieDto> Movielist = new List<ReadMovieDto>();
            try
            {
                using (HttpClient client= new HttpClient())
                {
                    client.BaseAddress = new Uri(localUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage responseMessage = client.GetAsync("/api/v1/Movies").Result;
                    client.Dispose();
                    if (responseMessage.IsSuccessStatusCode) 
                    {
                        string stringData = responseMessage.Content.ReadAsStringAsync().Result;
                        Movielist = JsonConvert.DeserializeObject<List<ReadMovieDto>>(stringData);
                    }
                    else
                    {
                        TempData["error"] = $"{responseMessage.ReasonPhrase}";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["exception"] = ex.Message;
            }
            return View(Movielist);
        }
        public IActionResult Display(string genre)
        {
            List<ReadMovieDto> Movielist = new List<ReadMovieDto>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(localUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                
                    HttpResponseMessage responseMessage = client.GetAsync($"/api/v1/Movies/genre/{genre}").Result;

                    client.Dispose();
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        string stringData = responseMessage.Content.ReadAsStringAsync().Result;
                        Movielist = JsonConvert.DeserializeObject<List<ReadMovieDto>>(stringData);
                    }
                    else
                    {
                        TempData["error"] = $"{responseMessage.ReasonPhrase}";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["exception"] = ex.Message;
            }
            return View(Movielist);
        }
       


        [HttpPost]
        public IActionResult Watch(string genre)
        {
            if (string.IsNullOrEmpty(genre))
            {
                return BadRequest("Genre cannot be empty");
            }

            
            if (HttpContext.Session.GetInt32("ActionCounter") == null)
            {
                HttpContext.Session.SetInt32("ActionCounter", 0);
            }
            if (HttpContext.Session.GetInt32("RomanceCounter") == null)
            {
                HttpContext.Session.SetInt32("RomanceCounter", 0);
            }
            if (HttpContext.Session.GetInt32("ComedyCounter") == null)
            {
                HttpContext.Session.SetInt32("ComedyCounter", 0);
            }
            if (HttpContext.Session.GetInt32("NoneCounter") == null)
            {
                HttpContext.Session.SetInt32("NoneCounter", 0);
            }

            
            if (genre == "Action")
            {
                HttpContext.Session.SetInt32("ActionCounter", HttpContext.Session.GetInt32("ActionCounter").Value + 1);
            }
            else if (genre == "Romance")
            {
                HttpContext.Session.SetInt32("RomanceCounter", HttpContext.Session.GetInt32("RomanceCounter").Value + 1);
            }
            else if (genre == "Comedy")
            {
                HttpContext.Session.SetInt32("ComedyCounter", HttpContext.Session.GetInt32("ComedyCounter").Value + 1);
            }
            else if (genre == "none")
            {
                HttpContext.Session.SetInt32("NoneCounter", HttpContext.Session.GetInt32("NoneCounter").Value + 1);
            }

            return RedirectToAction("Index");
        }
        private string GetMostWatchedGenre()
        {
            string mostWatchedGenre = "";
            int highestCounter = 0;

            
            int actionCounter = HttpContext.Session.GetInt32("ActionCounter") ?? 0;
            int romanceCounter = HttpContext.Session.GetInt32("RomanceCounter") ?? 0;
            int comedyCounter = HttpContext.Session.GetInt32("ComedyCounter") ?? 0;
            int noneCounter = HttpContext.Session.GetInt32("NoneCounter") ?? 0;

           
            if (actionCounter > highestCounter)
            {
                highestCounter = actionCounter;
                mostWatchedGenre = "Action";
            }
            if (romanceCounter > highestCounter)
            {
                highestCounter = romanceCounter;
                mostWatchedGenre = "Romance";
            }
            if (comedyCounter > highestCounter)
            {
                highestCounter = comedyCounter;
                mostWatchedGenre = "Comedy";
            }
            if (noneCounter > highestCounter)
            {
                highestCounter = noneCounter;
                mostWatchedGenre = "None";
            }

            return mostWatchedGenre;
        }

        private List<ReadMovieDto> FetchRecommendedMovies(string genre)
        {
            List<ReadMovieDto> recommendedMovies = new List<ReadMovieDto>();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(localUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage responseMessage = client.GetAsync($"/api/v1/Movies/genre/{genre}").Result;

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        string stringData = responseMessage.Content.ReadAsStringAsync().Result;
                        recommendedMovies = JsonConvert.DeserializeObject<List<ReadMovieDto>>(stringData);
                    }
                    else
                    {
                        TempData["error"] = $"{responseMessage.ReasonPhrase}";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["exception"] = ex.Message;
            }

            return recommendedMovies;
        }
        public IActionResult RecommendedMovies()
        {
            
            string mostWatchedGenre = GetMostWatchedGenre();

            
            List<ReadMovieDto> recommendedMovies = FetchRecommendedMovies(mostWatchedGenre);

            
            return View(recommendedMovies);
        }

        public IActionResult Addmovie()
        {   
            return View(); 
        }  

        [HttpPost]

        public IActionResult AddMovie(CreateMovieDto createMovieDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(localUrl);
                        var data = JsonConvert.SerializeObject(createMovieDto);
                        var contentData = new StringContent(data, Encoding.UTF8, "applciation/json");
                        HttpResponseMessage response = client.PostAsync("api/v1/Movies",contentData).Result;
                        TempData["success"] = response.Content.ReadAsStringAsync().Result;
                    }
                }

                else
                {
                    ModelState.AddModelError(String.Empty, "ModelState is not valid");
                    return View(createMovieDto);
                }
            }
            catch (Exception ex)
            {
                
            }

            return RedirectToAction("Index");   
        }
    }
}
