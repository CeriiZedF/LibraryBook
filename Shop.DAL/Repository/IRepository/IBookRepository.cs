using Microsoft.AspNetCore.Mvc.Rendering;
using ShopLibrary.Models;

namespace ShopLibrary.DAL.Repository.IRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        void Update(Book book);
        SelectList? GetAllDropDownList(string obj);
    }
}
