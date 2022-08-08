﻿using Ecommerce.Entities.ViewModel;
using Entities.Helper;
using Entities.ViewModel;

namespace ECommerce.Services.IServices;

public interface IUserService
{ 
    Task<ServiceResult> Logout();
    Task<ServiceResult<LoginViewModel>> Login(LoginViewModel loginViewModel);
    Task<ServiceResult> Register(RegisterViewModel registerViewModel);
    Task<ServiceResult<List<UserListViewModel>>> UserList(string search = "",
     int pageNumber = 0, int pageSize = 10, int userSort = 1, bool? isActive = null, bool? isColleague = null, bool? HasBuying = null);
}