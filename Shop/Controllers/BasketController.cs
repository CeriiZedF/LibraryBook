using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

using ShopLibrary.Services;

using ShopLibrary.Models;
using ShopLibrary.Models.ViewModels;
using ShopLibrary.DAL.Repository.IRepository;
using ShopLibrary.DAL.Repository;

namespace ShopLibrary.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly BasketSessionService _basketSessionService;
        private readonly CurrentUserProvider _currentUserProvider;

        public BasketController(
            IBookRepository bookRepository,
            IOrderRepository orderRepository,
            BasketSessionService basketSessionService,
            CurrentUserProvider currentUserProvider)
        {
            _bookRepository = bookRepository;
            _orderRepository = orderRepository;
            _basketSessionService = basketSessionService;
            _currentUserProvider = currentUserProvider;
        }


        [Authorize]
        public async Task<IActionResult> Index()
        {
            // получение товаров из сессии
            ShoppingCartVM? shoppingCart = _basketSessionService.GetShoppingCart();
            if (shoppingCart is null) { return View(); }

            var books = await _bookRepository.GetAll(
                b => shoppingCart.BookId.Contains(b.Id),
                includeProperties: "Category"
            );
            return View("Index", books);
        }

        // получение суммы в корзине
        public int GetTotalSum()
        {
            return _basketSessionService.GetTotalSum();
        }

       
        [HttpPost]
        public IActionResult AddInBasket([FromBody] DetailsVM? detailsVM)
        {
            if (detailsVM is null)
            {
                return Json(new { success = false, message = "Something went wrong, try again later" });
            }

            // обновляем данные о товарах в сессии
            _basketSessionService.SetShoppingCart(detailsVM.Book);

            return Json(new { success = true, message = "Successfully added to cart" });
        }

      
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id is null) { return View(); }

            Book? b = await _bookRepository.FirstOrDefault(b => b.Id == id);
            if (b is null) { return View(); }

            
            _basketSessionService.SetShoppingCart(b, true); 

            return RedirectToAction("Index");
        }

      
        [HttpPost]
        public async Task<IActionResult> SendOrder(string booksIdJson)
        {
            var booksId = JsonSerializer.Deserialize<List<int>>(booksIdJson);
            if (booksId is null) { return RedirectToAction("Index"); }

 
            ShopUser? user = await _currentUserProvider.GetCurrentShopUser();
            if (user is null) { return RedirectToAction("Index"); }

            DateTime dtNow = DateTime.Now;
            foreach (int id in booksId)
            {
                Order order = new()
                {
                    UserId = user.Id,
                    BookId = id,
                    CreatedDate = dtNow
                };
                await _orderRepository.Add(order);
            }
            await _orderRepository.Save();
            _basketSessionService.Clear(); 
            return View("ThankSendOrder");  
        }

        public IActionResult ThankSendOrder()
        {
            return View();
        }
    }
}
