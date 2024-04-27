namespace ShopLibrary.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public List<int> BookId { get; set; } = null!;
        public int TotalSum { get; set; }

        public ShoppingCartVM()
        {
            BookId = new();
            TotalSum = 0;
        }
    }
}
