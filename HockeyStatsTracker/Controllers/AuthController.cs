using DB.Models;
using Microsoft.AspNetCore.Mvc;

namespace HockeyStatsTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (loginModel.Username == "admin" && loginModel.Password == "admin")
        {
            return Ok("Logged in");
        }

        return Unauthorized();
    }
}