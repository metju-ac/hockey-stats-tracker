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
        // Would be connected to authentication service in a real application 
        if (loginModel.Username == "admin" && loginModel.Password == "admin")
        {
            return Ok("Logged in");
        }

        return Unauthorized();
    }
}