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

        //GET: /api/Auth/Get All
        [HttpGet]
        [Route("GetALL")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.ToListAsync();

            return Ok(users);
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
                            CompanyId = company.Id,
                            IsActive = true


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


        //PUT: api/auth/{id}
        [HttpPut]
        [Route("EditUser/{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserRequestDto updateUserRequestDto)
        {
            var userDomainModel = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userDomainModel == null)
            {
                return NotFound();
            }
            //DTO to main
            userDomainModel.Email = updateUserRequestDto.Email;
            userDomainModel.UserName = updateUserRequestDto.UserName;
            userDomainModel.FullName = updateUserRequestDto.FullName;

            //Save change

            await _context.SaveChangesAsync();

            return Ok(userDomainModel);

        }

        [HttpPut]
        [Route("EditPassword/{id}")]
        public async Task<IActionResult> UpdatePassword([FromRoute] string id, [FromBody] UpdatePasswordDto updatePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Verify the current password
            var passwordCheck = await _userManager.CheckPasswordAsync(user, updatePasswordDto.CurrentPassword);
            if (!passwordCheck)
            {
                return BadRequest("Current password is incorrect.");
            }

            // Change the password
            var result = await _userManager.ChangePasswordAsync(user, updatePasswordDto.CurrentPassword, updatePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password has been changed successfully.");
        }
    }
}
