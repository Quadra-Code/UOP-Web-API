using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UOP.Application.Common.DTOs.User;
using UOP.Application.Interfaces;

namespace UOP.Web.API.Controllers.User_Profile
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly IConfiguration configuration;

        public AccountController(IAccountService _accountService, IConfiguration _configuration)
        {
            accountService = _accountService;
            configuration = _configuration;
        }

        [Authorize(Roles = "SuperUser,Manager,Admin")]
        [HttpPost("Create-Staff")]
        public async Task<IActionResult> CreateStaff(CreateStaffDTO createStaffDTO)
        {
            if (ModelState.IsValid)
            {
                var userFullName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "";
                var result = await accountService.CreateStaffAsync(createStaffDTO, userFullName);
                if (result.IsSuccess)
                {
                    return CreatedAtAction(nameof(CreateStaff), new { id = result.Value.Id }, result.Value);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(AccountLoginDTO accountLoginDTO)
        {
            var loginResult = await accountService.LoginAsync(accountLoginDTO);
            if (loginResult.IsSuccess)
            {
                List<Claim> claims =
                [
                    new(ClaimTypes.Name, loginResult.Value?.FullName ?? ""),
                    new(ClaimTypes.Email, loginResult.Value?.Email ?? ""),
                    new(ClaimTypes.PrimarySid, loginResult.Value?.Id.ToString() ?? ""),
                ];

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"] ?? ""));
                var token = new JwtSecurityToken(
                    issuer: configuration["jwt:Issuer"],
                    audience: configuration["jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(15), // Short-lived access token  
                    //expires: DateTime.Now.AddMinutes(15), // Short-lived access token  
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
                string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                // Generate Refresh Token  
                var refreshToken = Guid.NewGuid().ToString();
                // Store refresh token securely (e.g., in a database)
                //await accountService.StoreRefreshTokenAsync(loginResult.Value.Id, refreshToken);

                var loginResponse = loginResult.Value.Adapt<LoginResponseDTO>();
                loginResponse.AccessToken = accessToken;
                loginResponse.RefreshToken = refreshToken;
                loginResponse.Role = "Client";

                return Ok(loginResponse);
            }
            else
            {
                return BadRequest(loginResult.Errors);
            }
        }

        //[HttpPost("RefreshToken")]
        //public async Task<IActionResult> RefreshToken(string refreshToken)
        //{
        //    var userId = await accountService.ValidateRefreshTokenAsync(refreshToken);
        //    if (userId == null)
        //    {
        //        return Unauthorized("Invalid or expired refresh token.");
        //    }

        //    // Generate new access token  
        //    List<Claim> claims = new List<Claim>()
        //   {
        //       new(ClaimTypes.PrimarySid, userId.ToString()),
        //   };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
        //    var token = new JwtSecurityToken(
        //        issuer: configuration["jwt:Issuer"],
        //        audience: configuration["jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(15), // Short-lived access token  
        //        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        //    );
        //    string newAccessToken = new JwtSecurityTokenHandler().WriteToken(token);

        //    return Ok(new { AccessToken = newAccessToken });
        //}

        [HttpPost("Register")]
        public async Task<IActionResult> Register(AccountCreateDTO accRegisterDto)
        {
            if (ModelState.IsValid)
            {
                var result = await accountService.AddAsync(accRegisterDto);
                if (result.IsSuccess)
                {
                    return Created();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(errors);
        }

        //[Authorize]
        //[HttpGet]
        //public async Task<IActionResult> UserDetails()
        //{
        //    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid);
        //    if (userIdClaim is not null && int.TryParse(userIdClaim.Value, out int userId))
        //    {
        //        ResultView<ClientDetailsDTO> result = await accountService.GetClientDetails(userId);
        //        if (result.IsSuccess)
        //            return Ok(result);
        //        else
        //            return BadRequest(result);
        //    }
        //    return Unauthorized("Invalid Token");
        //}
    }
}
