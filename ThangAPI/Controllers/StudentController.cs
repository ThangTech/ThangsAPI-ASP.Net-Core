using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ThangAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudent()
        {
            string[] studentNames = new string[]
            {
                "Dan",
                "Thang",
                "Son",
                "Phuc",
                "Thuan"
            };
            return Ok(studentNames);

        }

    }
}
