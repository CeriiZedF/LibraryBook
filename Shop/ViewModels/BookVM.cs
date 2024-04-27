using ShopLibrary.Helpers;
using ShopLibrary.Models;

namespace ShopLibrary.ViewModels
{
    public class BookVM
    {
        public IEnumerable<Book> Books { get; set; } = null!;
        public Pager Pager { get; set; } = null!;
    }
}
