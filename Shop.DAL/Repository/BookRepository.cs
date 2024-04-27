using Microsoft.AspNetCore.Mvc.Rendering;

using ShopLibrary.DAL.Repository.IRepository;
using ShopLibrary.DAL.Data;
using ShopLibrary.Models;

namespace ShopLibrary.DAL.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly DataContext _db;

        public BookRepository(DataContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Book book)
        {
            _db.Books.Update(book);
        }

        public SelectList? GetAllDropDownList(string obj)
        {
            if (obj.Equals("Category"))
            {
                return new SelectList(_db.Categories, "Id", "Name");
            }
            return null;
        }
    }
}
