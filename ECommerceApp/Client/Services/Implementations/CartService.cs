using Blazored.LocalStorage;
using ECommerceApp.Shared.ViewModels;
using System.Net.Http.Json;

namespace ECommerceApp.Client.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;

        public CartService(ILocalStorageService localStorage, HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
        }
        public event Action OnChange;
        public async Task AddToCart(CartItem cartItem)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();
            var sameItem =
                cart.Find(c => c.ProductId == cartItem.ProductId && c.ProductTypeId == cartItem.ProductTypeId);
            if (sameItem != null)
                sameItem.Quantity += cartItem.Quantity;
            else
                cart.Add(cartItem);
            await _localStorage.SetItemAsync("cart", cart);
            OnChange.Invoke();
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            return await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();
        }

        public async Task<List<CartProductResponse>> GetCartProducts()
        {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            var response = await _http.PostAsJsonAsync("api/cart/products", cartItems);
            return (await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>())?.Data;
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart is null)
                return;
            var cartItem = cart.Find(c => c.ProductId == productId && c.ProductTypeId == productTypeId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                await _localStorage.SetItemAsync("cart", cart);
                OnChange.Invoke();
            }
        }

        public async Task UpdateQuantity(CartProductResponse product)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart is null)
                return;
            var cartItem = cart.Find(c => c.ProductId == product.ProductId && c.ProductTypeId == product.ProductTypeId);
            if (cartItem != null)
            {
                cartItem.Quantity = product.Quantity;
                await _localStorage.SetItemAsync("cart", cart);
            }
        }
    }
}
