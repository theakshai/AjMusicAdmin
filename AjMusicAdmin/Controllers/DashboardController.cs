using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AjMusicAdmin.Controllers
{
    public class DashboardController : Controller
    {


        [Route("admin/dashboard")]
        public async Task<IActionResult> DashBoardIndex()
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://localhost:7020/ajfy/admin")
            };

            try {
            using var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                dynamic body = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                    Console.WriteLine(body);
                ViewBag.usersCount = body["userCount"];
                ViewBag.tracksCount = body["tracksCount"];
                ViewBag.artistsCount = body["artistsCount"];
                return View();
            }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                    
            }

            ViewBag.data = "API is Down";
            return View();

        }
    }
}
