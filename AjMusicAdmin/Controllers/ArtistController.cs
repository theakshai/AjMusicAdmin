using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AjMusicAdmin.Controllers
{
    public class ArtistController : Controller
    {
        [HttpGet]
        [Route("ajfy/admin/artist")]
        public async Task<IActionResult> ArtistIndex()
        {

            Console.WriteLine("calling function");
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method =  HttpMethod.Get,
                RequestUri = new Uri("https://localhost:7020/ajfy/admin/getallartist")
            };

            try
            {
                using var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
            Console.WriteLine("calling function");
                    var body = (JArray)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                    ViewBag.ArtistData = body;
                    Console.WriteLine(body);
                    return View();
                }
                else
                {
                    return View();
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());    
            }
            return View();
        }

        [HttpGet]
        [Route("ajfy/singleArtist")]

        public async Task<IActionResult> GetArtistById(string? id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://localhost:7020/ajfy/admin/getartistbyid?id={id}")
            };

            try
            {
                using var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var body = (JArray)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                    ViewBag.SingleArtist = body;
                    return View();
                }
                else
                {
                    return View();
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return View();
        }
    }
}
