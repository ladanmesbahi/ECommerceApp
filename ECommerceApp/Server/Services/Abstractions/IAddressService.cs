namespace ECommerceApp.Server.Services.Abstractions
{
    public interface IAddressService
    {
        Task<ServiceResponse<Address>> GetAddress();
        Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address);
    }
}
