﻿using ECommerceApp.Shared.Dtos;
using ECommerceApp.Shared.ViewModels;

namespace ECommerceApp.Client.Services.Abstractions
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegister request);
        Task<ServiceResponse<string>> Login(UserLogin request);
        Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request);
        Task<bool> IsUserAuthenticated();
    }
}
