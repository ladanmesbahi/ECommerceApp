namespace ECommerceApp.Client.Services.Abstractions
{
    public interface IAddressService
    {
        Task<Address> GetAddress();
        Task<Address> AddOrUpdateAddress(Address address);
    }
}
