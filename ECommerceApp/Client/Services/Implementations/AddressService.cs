using ECommerceApp.Shared.ViewModels;
using System.Net.Http.Json;

namespace ECommerceApp.Client.Services.Implementations
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient _http;

        public AddressService(HttpClient http)
        {
            _http = http;
        }
        public async Task<Address> AddOrUpdateAddress(Address address)
        {
            var response = await _http.PostAsJsonAsync<Address>("api/address", address);
            return response.Content.ReadFromJsonAsync<ServiceResponse<Address>>().Result.Data;
        }

        public async Task<Address> GetAddress()
        {
            return (await _http.GetFromJsonAsync<ServiceResponse<Address>>("api/address")).Data;
        }
    }
}
