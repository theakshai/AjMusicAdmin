﻿using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AjMusicAdmin.Controllers
{
    public class TracksController : Controller
    {

        [Route("admin/tracks")]
        [HttpGet]
        public async Task<IActionResult> TracksIndex(string? query, string? id)
        {


            if (query != null)
            {
                var _qclient = new HttpClient();
                var _qrequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://localhost:7020/ajfy/searchsong?query={query}")
                };

                try
                {
                    using var response = await _qclient.SendAsync(_qrequest);
                    if (response.IsSuccessStatusCode)
                    {
                        var body = (JArray)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                        Console.WriteLine($"The body is {body}");
                        ViewBag.SearchedSong = body;
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Exception = e.Message;
                    return View();
                }
            }

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://localhost:7020/ajfy/getalltracks")
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

        private bool isNullOrEmpty(string? query)
        {
            throw new NotImplementedException();
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
                        return Problem("Song is already there");
                    }
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                        return View();
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
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        dynamic body = (JObject)JObject.Parse(await response.Content.ReadAsStringAsync());
                        ViewBag.SearchData = body;
                        return View();
                    }
                    else
                    {
                        Console.Write("ehy");
                        return Problem("Errork has occured");
                    }
            }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            ViewBag.Data = "API is Currently Down";
            return View();
        }


        public async Task<IActionResult> DeleteSongById(string? id)
        {
            if (id != null)
            {
                var _dclient = new HttpClient();
                var _drequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri($"https://localhost:7020/ajfy/admin/deletesongbyid?id={id}")
                };

                try
                {
                    using var response = await _dclient.SendAsync(_drequest);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(TracksIndex));
                    }
                    else
                    {
                        return RedirectToAction(nameof(TracksIndex));
                    }
                }catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
                return View();
        }

    }
}
