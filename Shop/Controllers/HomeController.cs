using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using ShopLibrary.Services;
using ShopLibrary.Filters;

using ShopLibrary.Models;
using ShopLibrary.Models.ViewModels;
using ShopLibrary.DAL.Repository.IRepository;


namespace ShopLibrary.Controllers
{
    [CountRequests]
    public class HomeController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly BasketSessionService _basketSessionService;

        public HomeController(IBookRepository bookRepository, ICategoryRepository categoryRepository, BasketSessionService basketSessionService)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _basketSessionService = basketSessionService;
        }


        // вывод товаров
        public async Task<IActionResult> Index()
        {
            // по умолчанию показываем все товары
            return await ShowView(await _bookRepository.GetAll(
                includeProperties: "Category"
            ));
        }

        [HttpGet]
        public async Task<IActionResult> ChoiceCategory(int? id)
        {
            if (id is not null)  // если выбрана категория
            {
                return await ShowView(await _bookRepository.GetAll(
                    b => b.CategoryId == id,
                    includeProperties: "Category"
                ));
            }
            return RedirectToAction("Index");  // показываем все товары
        }

        // общий метод для отправки view для показа продуктов
        private async Task<IActionResult> ShowView(IEnumerable<Book> book)
        {
            HomeVM homeVM = new()
            {
                Catergories = await _categoryRepository.GetAll(),
                Books = book
            };
            return View("Index", homeVM);
        }

        // детальная информация о товаре
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) { return NotFound(); }

            Book? b = await _bookRepository.FirstOrDefault(
                b => b.Id == id,
                includeProperties: "Category"
            );
            if (b is null) { return NotFound(); }

            // проверяем есть ли товар в корзине
            DetailsVM detailsVM = new() { Book = b };
            ShoppingCartVM? shoppingCartVM = _basketSessionService.GetShoppingCart();
            if (shoppingCartVM is not null)
            {
                detailsVM.ExistsInCart = shoppingCartVM.BookId.Contains((int)id);
            }

            return View(detailsVM);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}