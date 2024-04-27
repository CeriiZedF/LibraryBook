using System.Text.Json;
using ShopLibrary.Models;
using ShopLibrary.Models.ViewModels;

namespace ShopLibrary.Services
{
    // Работа с сессиями для корзины товаров (общая сумма и коллекция id книг)
    public class BasketSessionService
    {
        private static string ShoppingCartKey = "ShopCart";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketSessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetTotalSum()
        {
            ShoppingCartVM? shopCart = GetShoppingCart();
            return shopCart is null ? 0 : shopCart.TotalSum;
        }

        public void Clear()
        {
            // clear basket
            _httpContextAccessor?.HttpContext?.Session.Set(
                ShoppingCartKey,
                JsonSerializer.SerializeToUtf8Bytes(new ShoppingCartVM())
            );
        }

        public ShoppingCartVM? GetShoppingCart()
        {
            string? json = _httpContextAccessor?.HttpContext?.Session.GetString(ShoppingCartKey);
            if (json is null) { return null; }
            return JsonSerializer.Deserialize<ShoppingCartVM>(json);
        }

        public void SetShoppingCart(Book book, bool isDelete = false)
        {
            var httpContext = _httpContextAccessor?.HttpContext;
            if (httpContext is null) { return; }

            if (!httpContext.Session.TryGetValue(ShoppingCartKey, out byte[]? cartData))
            {
                cartData = JsonSerializer.SerializeToUtf8Bytes(new ShoppingCartVM());
            }

            ShoppingCartVM shoppingCart = JsonSerializer.Deserialize<ShoppingCartVM>(cartData) ?? new();
            if (isDelete)
            {
                shoppingCart.BookId.RemoveAll(bId => bId == book.Id);
                shoppingCart.TotalSum -= (int)book.Price;
            }
            else
            {
                shoppingCart.BookId.Add(book.Id);
                shoppingCart.TotalSum += (int)book.Price;
            }
            httpContext.Session.Set(ShoppingCartKey, JsonSerializer.SerializeToUtf8Bytes(shoppingCart));
        }
    }
}
