﻿using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.Interface;
using Entities;
using Entities.Helper;
using Entities.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly SiteSettings _siteSettings;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IEmailRepository emailRepository, SignInManager<User> signInManager, UserManager<User> userManager, SiteSettings siteSettings, IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _emailRepository = emailRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _siteSettings = siteSettings;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginViewModel model, CancellationToken cancellationToken)
        {
            try
            {

                var ipAddress = GetIpAddress();

                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(model.Username))
                        return new ApiResult { Code = ResultCode.BadRequest };

                    var user = await _userRepository.GetByEmailOrUserName(model.Username, cancellationToken);

                    if (user == null)
                        return new ApiResult { Code = ResultCode.NotFound, Messages = new List<string> { "نام کاربری یا کلمه عبور صحیح نمی باشد" } };

                    if (!user.IsActive)
                        return Ok(new ApiResult { Code = ResultCode.DeActive, Messages = new List<string> { "کاربر غیرفعال شده است" } });

                    //var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (result.Succeeded)
                    {

                        var secretKey = Encoding.ASCII.GetBytes(_siteSettings.IdentitySetting.IdentitySecretKey);
                        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

                        //var encryptionKey = Encoding.UTF8.GetBytes(_siteSettings.IdentitySetting.EncryptKey); //must be 16 character
                        //var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionKey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);
                        var expireDate = DateTime.Now.AddMonths(1);

                        var claims = new[]
                        {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim("FullName", user.FirstName + " " + user.LastName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim("IsColleague", model.IsColleague.ToString()),
                        new Claim("IsActive", user.IsActive.ToString()),
                        new Claim("IsActive", user.IsActive.ToString()),
                        new Claim(ClaimTypes.Expired, (expireDate-DateTime.Now).Days.ToString()),
                        new Claim(ClaimTypes.Role, user.UserRole.Name)
                    };

                        
                        var token =new JwtSecurityToken(
                            issuer: _siteSettings.IdentitySetting.Issuer,
                            audience: _siteSettings.IdentitySetting.Audience,
                            expires: expireDate,
                            claims: claims,
                            signingCredentials: signingCredentials
                        );

                        //var descriptor = new SecurityTokenDescriptor
                        //{
                        //    Issuer = _siteSettings.IdentitySetting.Issuer,
                        //    Audience = _siteSettings.IdentitySetting.Audience,
                        //    IssuedAt = DateTime.Now,
                        //    NotBefore = DateTime.Now.AddMinutes(_siteSettings.IdentitySetting.NotBeforeMinutes),
                        //    SigningCredentials = signingCredentials,
                        //    EncryptingCredentials = encryptingCredentials,
                        //    Expires = expireDate,
                        //    Subject = new ClaimsIdentity(claims)
                        //};

                        //var tokenHandler = new JwtSecurityTokenHandler();

                        //var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
                        //var encryptedToken = tokenHandler
                        //    .WriteToken(securityToken);
                        var securityToken = new JwtSecurityTokenHandler().WriteToken(token);

                        await _userRepository.AddLoginHistory(user.Id, securityToken, ipAddress, expireDate);

                        return Ok(new ApiResult { Code = ResultCode.Success, ReturnData = securityToken });
                    }

                    if (result.IsLockedOut)
                    {
                        return Ok(new ApiResult { Code = ResultCode.Error, Messages = new List<string> { "حساب کاربری شما قفل شده است. دیرتر سعی کنید" } });
                    }

                }

                return Ok(new ApiResult { Code = ResultCode.BadRequest });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message); return Ok(new ApiResult
                {
                    Code = ResultCode.DatabaseError
                });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterViewModel register, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    List<string> list = new List<string>();
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            list.Add(error.ErrorMessage);
                        }
                    }

                    return Ok(new ApiResult
                    {
                        Code = ResultCode.BadRequest,
                        Messages = list
                    });
                }
                register.Username = register.Username.Trim();
                if (register.Username.ToLower().Contains("admin"))
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.BadRequest,
                        Messages = new List<string> { "در نام کربری نمی توانید از کلمه admin استفاده کنید" }
                    });
                }

                var repetitiveUsername = await _userRepository.GetByEmailOrUserName(register.Username, cancellationToken);
                if (repetitiveUsername != null)
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.BadRequest,
                        Messages = new List<string> { "نام کاربری تکراری است" }
                    });
                }

                var repetitiveEmail = await _userRepository.GetByEmailOrUserName(register.Email, cancellationToken);
                if (repetitiveEmail != null)
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.BadRequest,
                        Messages = new List<string> { "ایمیل تکراری است" }
                    });
                }

                User user = new User
                {
                    UserName = register.Username,
                    Email = register.Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    IsActive = false,
                    IsColleague = register.IsColleague,
                    LicensePath = register.LicensePath,
                    PicturePath = register.PicturePath,
                    IsFeeder = register.IsFeeder,
                    Mobile = register.Mobile,
                    UserRoleId = 4
                };

                //var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //var emailMessage = Url.Action("ConfirmEmail", "Users",
                //    new { username = user.UserName, token = emailConfirmationToken },
                //    Request.Scheme);
                //await _emailRepository.SendEmailAsync(register.Email, "Email confirmation", emailMessage, cancellationToken);

                var result = await _userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, SystemRoles.Client.ToString());

                    return Ok(new ApiResult
                    {
                        Code = ResultCode.Success,
                        Messages = new List<string> { "Register Succeed" }
                    });
                }
                return Ok(new ApiResult { Code = ResultCode.Error, Messages = result.Errors.Select(p => p.Description) });

            }
            catch (Exception e)
            {
                 _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError,Messages = new List<string> {  "اشکال در سمت سرور" }});
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckEmail(string email)
        {
            try
            {
                var repetitiveEmail = await _userManager.FindByEmailAsync(email);
                if (repetitiveEmail != null)
                {
                    return BadRequest("ایمیل تکراری است");
                }

                return Ok();
            }
            catch (Exception e)
            {
                 _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError,Messages = new List<string> {  "اشکال در سمت سرور" }});
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckUserName(string userName)
        {
            try
            {
                if (userName.ToLower().Contains("admin"))
                {
                    return BadRequest("در نام کربری نمی توانید از کلمه admin استفاده کنید");
                }
                var repetitiveUsername = await _userManager.FindByNameAsync(userName);
                if (repetitiveUsername != null)
                {
                    return BadRequest("نام کاربری تکراری است");
                }

                return Ok();
            }
            catch (Exception e)
            {
                 _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError,Messages = new List<string> {  "اشکال در سمت سرور" }});
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userName, string token, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
                return NotFound();
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return NotFound();
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return Content(result.Succeeded ? "Email Confirmed" : "Email Not Confirmed");
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<User>> Get(int id)
        {
            try
            {
                var result = await _userManager.FindByIdAsync(id.ToString());
                if (result == null)
                {
                    return NotFound();
                }

                return result;
            }
            catch (Exception e)
            {
                 _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError,Messages = new List<string> {  "اشکال در سمت سرور" }});
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationParameters paginationParameters, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(paginationParameters.Search)) paginationParameters.Search = "";
                var entity = await _userRepository.Search(paginationParameters, cancellationToken);
                var paginationDetails = new PaginationDetails
                {
                    TotalCount = entity.TotalCount,
                    PageSize = entity.PageSize,
                    CurrentPage = entity.CurrentPage,
                    TotalPages = entity.TotalPages,
                    HasNext = entity.HasNext,
                    HasPrevious = entity.HasPrevious,
                    Search = paginationParameters.Search
                };

                return Ok(new ApiResult
                {
                    PaginationDetails = paginationDetails,
                    Code = ResultCode.Success,
                    ReturnData = entity
                });
            }
            catch (Exception e)
            {
                 _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError,Messages = new List<string> {  "اشکال در سمت سرور" }});
            }
        }
        [HttpPost]
        [Authorize(Roles = "Client,Admin,SuperAdmin")]
        public async Task<ApiResult> Logout(CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();

            return new ApiResult { Code = ResultCode.Success, Messages = new List<string> { "Logout successfully" } };
        }

        [HttpPut]
        [Authorize(Roles = "Client,Admin,SuperAdmin")]
        public async Task<ActionResult<bool>> Put(MyAccountViewModel accountViewModel, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    List<string> list = new List<string>();
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            list.Add(error.ErrorMessage);
                        }
                    }

                    return Ok(new ApiResult
                    {
                        Code = ResultCode.BadRequest,
                        Messages = list
                    });
                }
                var temp = await _userRepository.Where(x => x.Id == accountViewModel.Id, cancellationToken);
                var user = temp.FirstOrDefault();
                if (user == null)
                {
                    return BadRequest();
                }

                if (user.UserName.ToLower().Contains("admin"))
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.BadRequest,
                        Messages = new List<string> { "در نام کربری نمی توانید از کلمه admin استفاده کنید" }
                    });
                }

                if (user.UserName.ToLower().Equals(accountViewModel.Username))
                {
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.BadRequest,
                        Messages = new List<string> { "نام کاربری تکراری است" }
                    });
                }
                user.IsColleague = accountViewModel.IsColleague;
                user.Mobile = accountViewModel.Mobile;
                user.IsHaveCustomerCode = accountViewModel.IsHaveCustomerCode;
                user.CustomerCodeCustomer = accountViewModel.CustomerCodeCustomer;


                if (accountViewModel.IsColleague && !String.IsNullOrEmpty(accountViewModel.LicensePath) && !String.IsNullOrEmpty(accountViewModel.CompanyName))
                {
                    user.LicensePath = accountViewModel.LicensePath;
                    user.CompanyName = accountViewModel.CompanyName;
                }

                user.FirstName = accountViewModel.FirstName;
                user.LastName = accountViewModel.LastName;
                user.IsFeeder = accountViewModel.IsFeeder;
                user.PicturePath = accountViewModel.PicturePath;
                user.StateId = accountViewModel.StateId;
                user.CityId = accountViewModel.CityId;
                user.Birthday = accountViewModel.Birthday;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded) return Ok(new ApiResult { Code = ResultCode.Success });
                return Ok(new ApiResult { Code = ResultCode.Error, Messages = result.Errors.Select(p => p.Description) });
            }
            catch (Exception e)
            {
                 _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError,Messages = new List<string> {  "اشکال در سمت سرور" }});
            }
        }

        [HttpDelete]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _userRepository.DeleteAsync(id, cancellationToken);
                return NoContent();
            }
            catch (Exception e)
            {
                 _logger.LogCritical(e, e.Message); return Ok(new ApiResult { Code = ResultCode.DatabaseError,Messages = new List<string> {  "اشکال در سمت سرور" }});
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<LoginViewModel>> GetCurrentUser(CancellationToken cancellationToken)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                LoginViewModel loginViewModel = await _userRepository.GetByEmailOrUserName(User.FindFirstValue(ClaimTypes.Name), cancellationToken);
                return Ok(new ApiResult { Code = ResultCode.Success, ReturnData = loginViewModel });
            }
            return Ok(new ApiResult { Code = ResultCode.NotFound });
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email, string url, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Ok(new ApiResult { Code = ResultCode.BadRequest });
            }

            var emailPasswordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var emailMessage = Url.Link(url,
                new { username = user.Email, token = emailPasswordResetToken });

            await _emailRepository.SendEmailAsync(email, "تغییر کلمه عبور", emailMessage, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Client,Admin,SuperAdmin")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordViewModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Token))
                    return new ApiResult { Code = ResultCode.BadRequest };

                var user = await _userRepository.GetByEmailOrUserName(model.Email, cancellationToken);

                if (user == null)
                    return new ApiResult { Code = ResultCode.NotFound, Messages = new List<string> { "کاربری با این مشخصات یافت نشد" } };

                var recoveryCode = model.Token.Replace(" ", string.Empty);
                var result = await _userManager.ResetPasswordAsync(user, model.Token, recoveryCode);

                if (result.Succeeded)
                {
                    return Ok(new ApiResult { Code = ResultCode.BadRequest });
                }
                return Ok(new ApiResult { Code = ResultCode.Error, Messages = new List<string> { "کد بازیابی معتبر نیست" } });
            }

            return Ok(new ApiResult { Code = ResultCode.BadRequest });
        }

        private string GetIpAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];

            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }

    }
}
