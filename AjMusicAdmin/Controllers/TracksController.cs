using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AjMusicAdmin.Controllers
{
    public class TracksController : Controller
    {

        [Route("admin/tracks")]
        [HttpGet]
        public async Task<IActionResult> TracksIndex(string? query)
        {



                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://localhost:7020/ajfy/admin/getalltracks")
                };

                try
                {
                    using var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        dynamic body = (JArray)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                        ViewBag.TracksData = body;
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }

                ViewBag.data = "API is Down";
                return View();
        }

        [Route("admin/tracks/singleTrack")]
        [HttpGet]
        public async Task<IActionResult> SingleTrack( string? id)
        {
            Console.WriteLine("The trackid is " + id);

            if(id == null)
            {
                ViewBag.NoData = "No Track for this id";
                return View();
            }

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://localhost:7020/ajfy/admin/gettrackbyid?track_id={id}")
            };

            try {
            using var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                dynamic body = (JArray)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                    ViewBag.TracksData = body;
                return View();
            }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                    
            }

            ViewBag.data = "API is Down";
            return View();
        }

        [HttpGet]
        [Route("/admin/addsong")]

        public async Task<IActionResult> AddSong(string? song, string songData)
        {

            if(songData != null)
            {
                var client = new HttpClient();
                var data = (JObject)JObject.Parse(songData);
                try
                {
                    using var response = await client.PostAsJsonAsync("https://localhost:7020/ajfy/user/addnewsong", data);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(TracksIndex));
                    }
                    else
                    {
                        return BadRequest(response.StatusCode);
                    }
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (song != null)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7020/ajfy/admin/searchtracks?title={song}")

                };
            try
            {
                using var response = await client.SendAsync(request);
                if(response.IsSuccessStatusCode)
                    {
                        dynamic body = (JObject)JObject.Parse(await response.Content.ReadAsStringAsync());
                        ViewBag.SearchData = body;
                    }
            }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            ViewBag.Data = "API is Currently Down";
            return View();
        }

    }
}
