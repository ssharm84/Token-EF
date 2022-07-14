using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TokenDemo.Authentication;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TokenDemo.Model;
using Microsoft.AspNetCore.Http;
using System;

namespace TokenDemo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticateController:ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationUser> roleManager;
        private readonly IConfiguration configuration;

        public AuthenticateController(UserManager<ApplicationUser> _userManager,RoleManager<ApplicationUser> _roleManager,IConfiguration _configuration)
        {
            this.userManager = _userManager;
            this.roleManager = _roleManager;
            this.configuration = _configuration;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExist = await userManager.FindByNameAsync(model.UserName);
            if(userExist!=null)//If user already exists
                return StatusCode(StatusCodes.Status500InternalServerError, new Response{Status = "Error", Message="User already exists"});
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };
            var result = await userManager.CreateAsync(user,model.Password);
            if(!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response{Status = "Error", Message="User creation failed"});
            }
            return Ok(new Response{Status="Success", Message="User Created Successfully"});
        }
            
       
        
    }
}