using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZamaraWebApp.Models;

namespace ZamaraWebApp.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        HttpClient client;

        Uri baseAddress = new("https://dummyjson.com/posts");
        public PostsController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                Root root = JsonConvert.DeserializeObject<Root>(data);
                return View(root.Posts);
            }
            return View();
        }
    }
}
