using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ShopLibrary.Filters;
using ShopLibrary.ViewModels;
using ShopLibrary.Helpers;

using ShopLibrary.Models;
using ShopLibrary.DAL.Repository.IRepository;

namespace ShopLibrary.Controllers
{
    [CountRequests]
    [Authorize(Policy = WC.AdminManagerPolicy)]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(IBookRepository bookrepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookrepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }


        // READ
        public async Task<IActionResult> Index(int numPage)
        {
            if (numPage <= 0) { numPage = 0; }
            const int pageSize = 2;

            var books = await _bookRepository.GetAll(includeProperties: "Category");
            BookVM bookVM = new()
            {
                Books = books,
                Pager = new Pager(books.Count(), numPage, pageSize)
            };

            return View(bookVM);
        }


        // CREATE
        public IActionResult Create()
        {
            ViewData["CategoryItems"] = _bookRepository.GetAllDropDownList("Category");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (book.Category is not null && !ModelState.IsValid)
            {
                return RedirectToAction(nameof(Create));
            }

            // отправлена ли картинка
            var files = HttpContext.Request.Form.Files;
            if (files.Count != 0)
            {
                string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    await files[0].CopyToAsync(fileStream);
                }
                book.Image = fileName + extension;
            }

            await _bookRepository.Add(book);
            await _bookRepository.Save();
            return RedirectToAction(nameof(Index));
        }


        // UPDATE
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) { return NotFound(); }

            var book = await _bookRepository.FirstOrDefault(p => p.Id == id);
            if (book is null) { return NotFound(); }

            ViewData["CategoryItems"] = _bookRepository.GetAllDropDownList("Category");
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book? book)
        {
            if (book is null) { return NotFound(); }
            try
            {
                // находим книгу
                Book? b = await _bookRepository.FirstOrDefault(b => b.Id == book.Id);
                if (b is null) { return NotFound(); }

                var files = HttpContext.Request.Form.Files;  // отправленные файлы
                book.Image = b.Image;  // картинка которая была до обновления
                if (files.Count > 0)
                {
                    string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
                    // если картинка была, то удаляем
                    if (b.Image is not null)
                    {
                        string oldFile = Path.Combine(upload, b.Image);
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                    }
                    // загружаем новую
                    string filename = Guid.NewGuid().ToString();
                    string ext = Path.GetExtension(files[0].FileName);
                    using (FileStream fs = new(Path.Combine(upload, filename + ext), FileMode.Create))
                    {
                        await files[0].CopyToAsync(fs);
                    }
                    book.Image = filename + ext;
                }

                _bookRepository.Update(book);

                await _bookRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction(nameof(Edit));
            }
        }


        // DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) { return NotFound(); }

            var book = await _bookRepository.FirstOrDefault(
                m => m.Id == id,
                includeProperties: "Category"
            );
            if (book is null) { return NotFound(); }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(Book? book)
        {
            if (book is null) { return NotFound(); }
            _bookRepository.Remove(book);
            await _bookRepository.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
