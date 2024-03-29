﻿using ECommerceApp.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace ECommerceApp.Client.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigationManager;

        public OrderService(HttpClient http, AuthenticationStateProvider authStateProvider, NavigationManager navigationManager)
        {
            _http = http;
            _authStateProvider = authStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task<OrderDetailsResponse> GetOrderDetails(int orderId)
        {
            return (await _http.GetFromJsonAsync<ServiceResponse<OrderDetailsResponse>>($"api/order/{orderId}")).Data;
        }

        public async Task<List<OrderOverviewResponse>> GetOrders()
        {
            return (await _http.GetFromJsonAsync<ServiceResponse<List<OrderOverviewResponse>>>("api/order")).Data;
        }

        public async Task PlaceOrder()
        {
            if (await IsUserAuthenticated())
                await _http.PostAsync("api/order", null);
            else
                _navigationManager.NavigateTo("login");
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
