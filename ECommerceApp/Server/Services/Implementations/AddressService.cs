using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Server.Services.Implementations
{
    public class AddressService : IAddressService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;

        public AddressService(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address)
        {
            var response = new ServiceResponse<Address>();
            var dbAddress = (await GetAddress()).Data;
            if (dbAddress == null)
            {
                address.UserId = _authService.GetUserId();
                _context.Addresses.Add(address);
            }
            else
            {
                dbAddress.FirstName = address.FirstName;
                dbAddress.LastName = address.LastName;
                dbAddress.State = address.State;
                dbAddress.Country = address.Country;
                dbAddress.City = address.City;
                dbAddress.Street = address.Street;
                dbAddress.Zip = address.Zip;
            }
            await _context.SaveChangesAsync();
            response.Data = address;
            return response;
        }

        public async Task<ServiceResponse<Address>> GetAddress()
        {
            return new ServiceResponse<Address>
            {
                Data = await _context.Addresses.FirstOrDefaultAsync(a => a.UserId == _authService.GetUserId())
            };
        }
    }
}
