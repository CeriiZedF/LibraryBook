using ShopLibrary.DAL.Repository.IRepository;
using ShopLibrary.DAL.Data;
using ShopLibrary.Models;

namespace ShopLibrary.DAL.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly DataContext _db;

        public CategoryRepository(DataContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(Category category)
        {
            var categoryFromDb = await base.FirstOrDefault(x => x.Id == category.Id);
            if (categoryFromDb is not null)
            {
                categoryFromDb.Name = category.Name;
                categoryFromDb.DisplayOrder = category.DisplayOrder;
                _db.Update(categoryFromDb);
            }
        }
    }
}
