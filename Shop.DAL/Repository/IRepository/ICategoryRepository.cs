using ShopLibrary.Models;

namespace ShopLibrary.DAL.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task Update(Category category);
    }
}
