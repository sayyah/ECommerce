using ECommerce.Domain.Entities.HolooEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UsersController(IUnitOfWork unitOfWork,
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        SiteSettings siteSettings,
        ILogger<UsersController> logger,
        IEmailRepository emailRepository)
    : ControllerBase
{
    private readonly ICityRepository _cityRepository = unitOfWork.GetRepository<CityRepository, City>();
    private readonly IHolooCustomerRepository _holooCustomerRepository = unitOfWork.GetHolooRepository<HolooCustomerRepository, HolooCustomer>();
    private readonly IHolooSarfaslRepository _holooSarfaslRepository = unitOfWork.GetHolooRepository<HolooSarfaslRepository, HolooSarfasl>();
    private readonly IStateRepository _stateRepository = unitOfWork.GetRepository<StateRepository, State>();
    private readonly IUserRepository _userRepository = unitOfWork.GetRepository<UserRepository, User>();


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
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.NotFound,
                        Messages = new List<string> { "نام کاربری یا کلمه عبور صحیح نمی باشد" }
                    });

                if (!user.IsActive)
                    return Ok(new ApiResult
                    { Code = ResultCode.DeActive, Messages = new List<string> { "کاربر غیرفعال شده است" } });

                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                var oneTimePass = model.Password == user.ConfirmCode + "" &&
                                  (user.ConfirmCodeExpirationDate! - DateTime.Now).Value.TotalSeconds > 0;
                if (result.Succeeded || oneTimePass)
                {
                    if (siteSettings.IdentitySetting != null)
                    {
                        var secretKey = Encoding.ASCII.GetBytes(siteSettings.IdentitySetting.IdentitySecretKey!);
                        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                            SecurityAlgorithms.HmacSha256Signature);

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
                            new Claim(ClaimTypes.Expired, (expireDate - DateTime.Now).Days.ToString()),
                            new Claim(ClaimTypes.Role, user.UserRole?.Name!)
                        };


                        var token = new JwtSecurityToken(
                            siteSettings.IdentitySetting.Issuer,
                            siteSettings.IdentitySetting.Audience,
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

                        _userRepository.AddLoginHistory(user.Id, securityToken, ipAddress, expireDate);
                        await unitOfWork.SaveAsync(cancellationToken);

                        return Ok(new ApiResult { Code = ResultCode.Success, ReturnData = securityToken });
                    }
                }

                if (result.IsLockedOut)
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.Error,
                        Messages = new List<string> { "حساب کاربری شما قفل شده است. دیرتر سعی کنید" }
                    });
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound,
                    Messages = new List<string> { "نام کاربری یا پسورد اشتباه است" }
                });
            }

            return Ok(new ApiResult { Code = ResultCode.BadRequest });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
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
                var list = new List<string>();
                foreach (var modelState in ModelState.Values)
                    foreach (var error in modelState.Errors)
                        list.Add(error.ErrorMessage);

                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest,
                    Messages = list
                });
            }

            register.Username = register.Username.Trim();
            if (register.Username.ToLower().Contains("admin"))
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest,
                    Messages = new List<string> { "در نام کاربری نمی توانید از کلمه admin استفاده کنید" }
                });

            var repetitiveUsername = await _userRepository.GetByEmailOrUserName(register.Username, cancellationToken);
            if (repetitiveUsername != null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest,
                    Messages = new List<string> { "نام کاربری تکراری است" }
                });

            if (register.IsHaveEmail)
            {
                var repetitiveEmail = await _userRepository.GetByEmailOrUserName(register.Email, cancellationToken);
                if (repetitiveEmail != null)
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.BadRequest,
                        Messages = new List<string> { "ایمیل تکراری است" }
                    });
            }
            else
            {
                var emailSplit = register.Email.Split('@');
                emailSplit[0] += _userRepository.TableNoTracking.OrderByDescending(x => x.Id).First().Id;
                register.Email = $"{emailSplit[0]}@{emailSplit[1]}";
            }

            var customerCode = await _holooCustomerRepository.GetNewCustomerCode();
            var customerName = register.IsColleague
                ? $"{register.CompanyName} {register.CompanyTypeName}-آنلاین"
                : $"{register.FirstName}-{register.LastName}-شخصی-آنلاین";
            var moeinCode = await _holooSarfaslRepository.Add(customerName, cancellationToken);
            var cityName = (await _cityRepository.GetByIdAsync(cancellationToken, register.CityId))?.Name;
            var stateName = (await _stateRepository.GetByIdAsync(cancellationToken, register.StateId))?.Name;

            var cityCode = register.IsColleague ? register.CompanyType : 45;
            var holooCustomer = new HolooCustomer
            {
                C_Code = customerCode.customerCode,
                C_Code_C = customerCode.customerCodeC,
                C_Name = customerName,
                C_Mobile = register.Mobile,
                Col_Code_Bed = "103",
                Moien_Code_Bed = moeinCode,
                National_Code = register.NationalCode,
                Etebar = 0,
                KarPorsant = 0,
                C_type = 1,
                Mekhrajkar = false,
                JavaherSaz = false,
                Arayeshgar = false,
                Arayesh_Porsant = 0,
                ArzID = 1,
                City_Code = cityCode,
                money_price = 1,
                Cust_type = 1,
                TasviehType = 3,
                SanadOutOfEtebar = false,
                TimeEndCustArayesh = DateTime.Now,
                TimeStartCustArayesh = DateTime.Now,
                Sum_Takhfif = 0,
                Porsant = 0,
                Porsant2 = 0,
                First_BalanceSanad = 0,
                Cust_City = cityName,
                Cust_Ostan = stateName,
                C_Address = $"{stateName}, {cityName}",
                Kharid = true,
                Forosh = true
            };
            var newCustomerCode = await _holooCustomerRepository.AddWithoutSave(holooCustomer, cancellationToken);

            var user = new User
            {
                UserName = register.Username,
                Email = register.Email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                FirstName = register.FirstName,
                LastName = register.LastName,
                IsActive = true,
                IsColleague = register.IsColleague,
                LicensePath = register.LicensePath,
                PicturePath = register.PicturePath,
                IsFeeder = register.IsFeeder,
                Mobile = register.Mobile,
                UserRoleId = 4,
                CustomerCode = newCustomerCode,
                NationalCode = register.NationalCode,
                CityId = register.CityId,
                StateId = register.StateId
            };

            if (register.IsColleague) user.CompanyName = register.CompanyName;

            //var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            //var emailMessage = Url.Action("ConfirmEmail", "Users",
            //    new { username = user.UserName, token = emailConfirmationToken },
            //    Request.Scheme);
            //await _emailRepository.SendEmailAsync(register.Email, "Email confirmation", emailMessage, cancellationToken);

            var result = await userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, SystemRoles.Client.ToString());
                await unitOfWork.SaveAsync(cancellationToken, true);

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
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    public async Task<IActionResult> CheckEmail(string email)
    {
        try
        {
            var repetitiveEmail = await userManager.FindByEmailAsync(email);
            if (repetitiveEmail != null) return BadRequest("ایمیل تکراری است");

            return Ok();
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    public async Task<IActionResult> CheckUserName(string userName)
    {
        try
        {
            if (userName.ToLower().Contains("admin"))
                return BadRequest("در نام کاربری نمی توانید از کلمه admin استفاده کنید");
            var repetitiveUsername = await userManager.FindByNameAsync(userName);
            if (repetitiveUsername != null) return BadRequest("نام کاربری تکراری است");

            return Ok();
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userName, string token, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
            return NotFound();
        var user = await userManager.FindByNameAsync(userName);
        if (user == null) return NotFound();
        var result = await userManager.ConfirmEmailAsync(user, token);

        return Content(result.Succeeded ? "Email Confirmed" : "Email Not Confirmed");
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<User>> Get(int id)
    {
        try
        {
            var result = await userManager.FindByIdAsync(id.ToString());
            if (result == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
            {
                Code = ResultCode.DatabaseError,
                Messages = new List<string> { "اشکال در سمت سرور" }
            });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Get([FromQuery] userFilterParameters userFilterParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            if(userFilterParameters.PaginationParameters == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            if (string.IsNullOrEmpty(userFilterParameters.PaginationParameters.Search))
                userFilterParameters.PaginationParameters.Search = "";
            var entity = await _userRepository.Search(userFilterParameters, cancellationToken);
            var paginationDetails = new PaginationDetails
            {
                TotalCount = entity.TotalCount,
                PageSize = entity.PageSize,
                CurrentPage = entity.CurrentPage,
                TotalPages = entity.TotalPages,
                HasNext = entity.HasNext,
                HasPrevious = entity.HasPrevious,
                Search = userFilterParameters.PaginationParameters.Search
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
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ApiResult> Logout(CancellationToken cancellationToken)
    {
        await signInManager.SignOutAsync();

        return new ApiResult { Code = ResultCode.Success, Messages = new List<string> { "Logout successfully" } };
    }

    [HttpPut]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<bool>> PutOld(MyAccountViewModel accountViewModel,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var list = new List<string>();
                foreach (var modelState in ModelState.Values)
                    foreach (var error in modelState.Errors)
                        list.Add(error.ErrorMessage);

                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest,
                    Messages = list
                });
            }

            var temp = await _userRepository.Where(x => x.Id == accountViewModel.Id, cancellationToken);
            if(temp == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest
                });

            var user = temp.FirstOrDefault();
            if (user == null) return BadRequest();

            if (user.UserName != null && user.UserName.ToLower().Contains("admin"))
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest,
                    Messages = new List<string> { "در نام کاربری نمی توانید از کلمه admin استفاده کنید" }
                });

            if (user.UserName != null && user.UserName.ToLower().Equals(accountViewModel.Username))
                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest,
                    Messages = new List<string> { "نام کاربری تکراری است" }
                });
            user.IsColleague = accountViewModel.IsColleague;
            user.Mobile = accountViewModel.Mobile;
            user.IsHaveCustomerCode = accountViewModel.IsHaveCustomerCode;
            user.CustomerCodeCustomer = accountViewModel.CustomerCodeCustomer;


            if (accountViewModel.IsColleague && !string.IsNullOrEmpty(accountViewModel.LicensePath) &&
                !string.IsNullOrEmpty(accountViewModel.CompanyName))
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
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok(new ApiResult { Code = ResultCode.Success });
            return Ok(new ApiResult { Code = ResultCode.Error, Messages = result.Errors.Select(p => p.Description) });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpDelete]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _userRepository.DeleteById(id, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);
            return NoContent();
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<LoginViewModel>> GetCurrentUser(CancellationToken cancellationToken)
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            LoginViewModel loginViewModel =
                (await _userRepository.GetByEmailOrUserName(User.FindFirstValue(ClaimTypes.Name)!, cancellationToken))!;
            return Ok(new ApiResult { Code = ResultCode.Success, ReturnData = loginViewModel });
        }

        return Ok(new ApiResult { Code = ResultCode.NotFound });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(model.EmailOrPhoneNumber);
        if (user == null) return Ok(new ApiResult { Code = ResultCode.BadRequest });

        var emailPasswordResetToken = await userManager.GeneratePasswordResetTokenAsync(user);

        //var emailMessage = Url.Link(url,
        //    new { username = user.Email, token = emailPasswordResetToken });
        var emailMessage = "<!DOCTYPE html><html><body><a href='"
                           + "localhost:7176" + "/ResetForgotPassword/token=" + emailPasswordResetToken + "&user=" +
                           user.UserName
                           + "'>dsf</a></body></html>";
        await userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword",
            emailPasswordResetToken);
        await emailRepository.SendEmailAsync(model.EmailOrPhoneNumber, "تغییر کلمه عبور", emailMessage,
            cancellationToken);

        return Ok(new ApiResult
        {
            Code = ResultCode.Success,
            Messages = new List<string> { "ایمیل با موفقیت ارسال شد" }
        });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetForgotPassword([FromBody] ResetForgotPasswordViewModel model,
        CancellationToken cancellationToken)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetByEmailOrUserName(model.Email, cancellationToken);
                if (user == null)
                    return new ApiResult
                    { Code = ResultCode.NotFound, Messages = new List<string> { "کاربری با این مشخصات یافت نشد" } };

                //var passToken = UserManager<User>.ResetPasswordTokenPurpose;
                //string resetToken = model.PasswordResetToken.Replace(" ", "+");
                //var VerifyToken = _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", resetToken);
                //if (VerifyToken.Result)
                //{
                var result = await userManager.ResetPasswordAsync(user, model.PasswordResetToken, model.Password);

                if (result.Succeeded)
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.Success,
                        Messages = new List<string> { "پسورد با موفقیت تغییر کرد" }
                    });

                return Ok(new ApiResult
                { Code = ResultCode.Error, Messages = new List<string> { "تغییر پسورد با شکست مواجه شد" } });

                //
                //}
            }

            return Ok(new ApiResult { Code = ResultCode.BadRequest });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordViewModel model,
        CancellationToken cancellationToken)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Username))
                    return new ApiResult { Code = ResultCode.BadRequest };

                var userFindByUsername = await _userRepository.GetByEmailOrUserName(model.Username, cancellationToken);
                var userId = userFindByUsername?.Id;
                var user = await userManager.FindByIdAsync(userId.ToString()!);

                if (user == null)
                    return new ApiResult
                    { Code = ResultCode.NotFound, Messages = new List<string> { "کاربری با این مشخصات یافت نشد" } };
                var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);

                if (result.Succeeded)
                    return Ok(new ApiResult
                    {
                        Code = ResultCode.Success,
                        Messages = new List<string> { "پسورد با موفقیت تغییر کرد" }
                    });

                return Ok(new ApiResult
                { Code = ResultCode.Error, Messages = new List<string> { "تغییر پسورد با شکست مواجه شد" } });
            }

            return Ok(new ApiResult { Code = ResultCode.BadRequest });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    private string GetIpAddress()
    {
        // get source ip address for the current request
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"]!;

        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()??"";
    }

    [HttpPut]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult> Put(User user, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var list = new List<string>();
                foreach (var modelState in ModelState.Values)
                    foreach (var error in modelState.Errors)
                        list.Add(error.ErrorMessage);

                return Ok(new ApiResult
                {
                    Code = ResultCode.BadRequest,
                    Messages = list
                });
            }

            _userRepository.Update(user);
            await unitOfWork.SaveAsync(cancellationToken);
            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                Messages = new List<string> { "ویرایش با موفقیت انجام شد" }
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult
            { Code = ResultCode.DatabaseError, Messages = new List<string> { "اشکال در سمت سرور" } });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Client,Admin,SuperAdmin")]
    public async Task<ActionResult<User>> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userRepository.GetByIdAsync(cancellationToken, id);
            if (result == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    public async Task<ActionResult<bool>> SetConfirmCodeByUsername(string username, int confirmCode,
        CancellationToken cancellationToken)
    {
        var codeConfirmExpireDate = DateTime.Now.AddSeconds(130);
        try
        {
          _userRepository.SetConfirmCodeByUsername(username, confirmCode, codeConfirmExpireDate);
          await unitOfWork.SaveAsync(cancellationToken);

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = true
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }

    [HttpGet]
    public async Task<ActionResult<int?>> GetSecondsLeftConfirmCodeExpire(string username,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userRepository.GetSecondsLeftConfirmCodeExpire(username, cancellationToken);
            if (result == null)
                return Ok(new ApiResult
                {
                    Code = ResultCode.NotFound
                });

            return Ok(new ApiResult
            {
                Code = ResultCode.Success,
                ReturnData = result
            });
        }
        catch (Exception e)
        {
            logger.LogCritical(e, e.Message);
            return Ok(new ApiResult { Code = ResultCode.DatabaseError });
        }
    }
}