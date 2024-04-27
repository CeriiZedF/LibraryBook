namespace ShopLibrary.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Category> Catergories { get; set; } = null!;
        public IEnumerable<Book> Books { get; set; } = null!;
    }
}
