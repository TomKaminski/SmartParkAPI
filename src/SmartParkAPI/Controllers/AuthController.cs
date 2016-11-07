using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SmartParkAPI.Contracts.Common;
using SmartParkAPI.Contracts.DTO.User;
using SmartParkAPI.Contracts.DTO.UserPreferences;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Models.Auth;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IUserService _userService;
        private readonly IUserDeviceService _userDeviceService;

        public AuthController(IOptions<JwtIssuerOptions> jwtOptions, ILoggerFactory loggerFactory, IUserService userService, IUserDeviceService userDeviceService)
        {
            _userService = userService;
            _userDeviceService = userDeviceService;
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);

            _logger = loggerFactory.CreateLogger<AuthController>();

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpPost]
        [Route("LoginWeb")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWeb([FromBody] ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return ReturnBadRequestWithModelErrors();
            }

            var userLoginResult = await _userService.LoginAsync(applicationUser.UserName, applicationUser.Password);
            if (!userLoginResult.IsValid)
            {
                _logger.LogInformation($"Invalid username ({applicationUser.UserName}) or password ({applicationUser.Password})");
                return BadRequest(SmartJsonResult.Failure("Invalid credentials."));
            }
            var identity = GetClaimsIdentity(userLoginResult);

            var encodedJwt = await CreateJwtToken(applicationUser.UserName, identity);

            
            // Serialize and return the response
            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            var json = JsonConvert.SerializeObject(SmartJsonResult<object>.Success(response), _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Route("LoginApp")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginApp([FromForm] MobileApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return ReturnBadRequestWithModelErrors();
            }

            var userLoginResult = await _userService.LoginAsync(applicationUser.UserName, applicationUser.Password);
            if (!userLoginResult.IsValid)
            {
                _logger.LogInformation($"Invalid username ({applicationUser.UserName}) or password ({applicationUser.Password})");
                return BadRequest("Invalid credentials");
            }
            var identity = GetClaimsIdentity(userLoginResult);

            var appTokenResult = await _userDeviceService.CreateUpdateMobileTokenAsync(userLoginResult.Result, applicationUser.DeviceName);

            if (!appTokenResult.IsValid)
            {
                return BadRequest("Cannot register current device in system.");
            }

            var encodedJwt = await CreateJwtToken(applicationUser.UserName, identity);

            // Serialize and return the response
            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                refresh_token = appTokenResult.Result.Token
            };

            var json = JsonConvert.SerializeObject(SmartJsonResult<object>.Success(response), _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpPost]
        [Route("RefreshWebToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshAppToken([FromForm] RefreshAppTokenModel model)
        {
            if (!ModelState.IsValid)
            {
                return ReturnBadRequestWithModelErrors();
            }
            var validateMobileTokenResult = await _userDeviceService.ValidateMobileTokenAsync(model.Email, model.Token);
            if (validateMobileTokenResult.IsValid)
            {
                var identity = GetClaimsIdentity(validateMobileTokenResult);
                var encodedJwt = await CreateJwtToken(model.Email, identity);
                // Serialize and return the response
                var response = new
                {
                    access_token = encodedJwt,
                    expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
                };

                var json = JsonConvert.SerializeObject(response, _serializerSettings);
                return new OkObjectResult(json);
            }
            return BadRequest();
        }


        [HttpPost]
        [Route("RefreshWebToken")]
        public IActionResult RefreshWebToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].First().Replace("Bearer ", "");
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var decodedJwt = jwtSecurityTokenHandler.ReadJwtToken(token);
            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: decodedJwt.Claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = jwtSecurityTokenHandler.WriteToken(jwt);

            // Serialize and return the response
            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }



        private async Task<string> CreateJwtToken(string email, ClaimsIdentity identity)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(),
                    ClaimValueTypes.Integer64)
            };
            var allClaims = claims.Union(identity.Claims);

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: allClaims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private ClaimsIdentity GetClaimsIdentity(ServiceResult<UserBaseDto, UserPreferencesDto> validServiceResult)
        {
            var claims = new List<Claim>
                {
                    new Claim("Username", validServiceResult.Result.Email),
                    new Claim("Name", validServiceResult.Result.Name),
                    new Claim("isAdmin",validServiceResult.Result.IsAdmin.ToString()),
                    new Claim("LastName",validServiceResult.Result.LastName),
                    new Claim("SidebarShrinked",validServiceResult.SecondResult.ShrinkedSidebar.ToString()),
                    new Claim("userId",validServiceResult.Result.Id.ToString()),
                    new Claim("photoId", validServiceResult.SecondResult.ProfilePhotoId.ToString())
                };
            var identity = new ClaimsIdentity(claims, "Token");
            return identity;
        }
    }
}