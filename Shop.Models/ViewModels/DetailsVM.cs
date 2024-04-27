namespace ShopLibrary.Models.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Book = new();
            ExistsInCart = false;
        }

        public Book Book { get; set; }
        public bool ExistsInCart { get; set; }
    }
}
