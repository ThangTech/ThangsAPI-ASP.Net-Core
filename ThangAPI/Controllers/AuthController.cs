using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThangAPI.Models.DTO;
using ThangAPI.Repositoty;

namespace ThangAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        //POST /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.UserName
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerDTO.Password);
            if(identityResult.Succeeded)
            {
                //Add roles to this User
                if(registerDTO.Roles !=  null && registerDTO.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerDTO.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! You can login!!!");
                    }
                }
            }
            return BadRequest("Something went wrong!!");
        }
        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await userManager.FindByEmailAsync(loginDTO.UserName);
            if (user != null)
            {
                var checkPassword = await userManager.CheckPasswordAsync(user, loginDTO.Password);
                if (checkPassword)
                {
                    //Get Roles for this user
                    var roles = await userManager.GetRolesAsync(user);
                    if(roles != null)
                    {
                        //Create Token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var reponse = new LoginResponseDTO
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(reponse);
                    }
                }
            }
            return BadRequest("Something went wrong");
        }


    }
}
