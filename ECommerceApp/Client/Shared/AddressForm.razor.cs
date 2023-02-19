namespace ECommerceApp.Client.Shared
{
    public partial class AddressForm
    {
        Address _address;
        bool _editAddress = false;
        protected override async Task OnInitializedAsync()
        {
            _address = await AddressService.GetAddress();
        }
        private async Task SubmitAddress()
        {
            _editAddress = false;
            _address = await AddressService.AddOrUpdateAddress(_address);
        }
        private void InitAddress()
        {
            _address = new Address();
            _editAddress = true;
        }
        private void EditAddress()
        {
            _editAddress = true;
        }
    }
}
