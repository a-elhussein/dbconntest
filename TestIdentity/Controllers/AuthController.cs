using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestIdentity.Models.DTO;
using TestIdentity.Models;
using TestIdentity.Data;
using Microsoft.EntityFrameworkCore;

namespace TestIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TestIdentityDbContext _context;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            TestIdentityDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // POST: /api/Auth/register
        [HttpPost]
        [Route("(Register)")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, FullName = model.FullName };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Link user to companies
                    foreach (var companyName in model.Company)
                    {
                        var company = await _context.Companies
                            .FirstOrDefaultAsync(c => c.CompanyName == companyName);

                        if (company == null)
                        {
                            return BadRequest("Company Does Not Exist");
                        }

                        // Create UserCompany relationship
                        var userCompany = new UserCompany
                        {
                            UserId = user.Id,
                            CompanyId = company.Id

                        };
                        _context.UserCompanies.Add(userCompany);
                    }

                    // Link user to roles
                    foreach (var role in model.Roles)
                    {
                        if (await _roleManager.RoleExistsAsync(role))
                        {
                            await _userManager.AddToRoleAsync(user, role);
                        }
                    }

                    await _context.SaveChangesAsync();
                    return Ok("User registered successfully.");
                }

                return BadRequest(result.Errors);
            }

            return BadRequest(ModelState);
        }

        // POST: api/auth/SignIn
        [HttpPost]
        [Route("SignIn")]

        public async Task<IActionResult> Signin([FromBody] SigninRequestDto signinRequestDto) 
        {
            var user = await _userManager.FindByNameAsync(signinRequestDto.UserName);
            if (user != null) 
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, signinRequestDto.Password);

                if (checkPasswordResult) 
                {
                    return Ok("UserName and Password is correct");
                }
            }

            return BadRequest();
        }
    }
}
