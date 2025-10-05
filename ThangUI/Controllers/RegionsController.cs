using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ThangUI.Models;
using ThangUI.Models.DTO;

namespace ThangUI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDTO> response = new List<RegionDTO>();
            //Get All Region From Web Api
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponse = await client.GetAsync("https://localhost:7263/api/region");

                httpResponse.EnsureSuccessStatusCode(); //return 200

                response.AddRange(await httpResponse.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());

        
            }
            catch (Exception ex)
            {

                throw;
            }
            return View(response);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7263/api/region"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")

            };

            var httpResponse = await client.SendAsync(httpRequestMessage);
            httpResponse.EnsureSuccessStatusCode();

            var response = await httpResponse.Content.ReadFromJsonAsync<RegionDTO>();
            
            if(response is not null)
            {
                return RedirectToAction("Index", "Regions"); // Chuyển hướng đến index của controller regions
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //ViewBag.Id = id;
            var client = httpClientFactory.CreateClient();

            var httpResponse = await client.GetFromJsonAsync<RegionDTO>($"https://localhost:7263/api/region/{id.ToString()}");

            if(httpResponse is not null)
            {
                return View(httpResponse);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RegionDTO request)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7263/api/region/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };

            var httpResponse = await client.SendAsync(httpRequest);
            httpResponse.EnsureSuccessStatusCode();

            var response = await httpResponse.Content.ReadFromJsonAsync<RegionDTO>();
            if(response is not null)
            {
                return RedirectToAction("Index", "Regions");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDTO request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponse = await client.DeleteAsync($"https://localhost:7263/api/region/{request.Id}");
                httpResponse.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex)
            {
            }
            return View("Edit");

        }

    }
}
