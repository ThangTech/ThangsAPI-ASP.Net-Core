using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index()
        {
            List<RegionDTO> response = new List<RegionDTO>();
            //Get All Region From Web Api
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponese = await client.GetAsync("https://localhost:7263/api/region");

                httpResponese.EnsureSuccessStatusCode(); //return 200

                response.AddRange(await httpResponese.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());

        
            }
            catch (Exception ex)
            {

                throw;
            }
            return View(response);
        }
    }
}
