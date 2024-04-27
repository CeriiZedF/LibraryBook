using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLibrary.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Название книги")]
        [Required(ErrorMessage = "Укажите название книги")]
        public string Name { get; set; } = null!;

        [DisplayName("Короткое описание книги")]
        public string? ShortDesc { get; set; }

        [DisplayName("Описание книги")]
        public string? Description { get; set; }

        [DisplayName("Цена книги")]
        [Required(ErrorMessage = "Укажите цену книги")]
        [Range(1, int.MaxValue)]
        public double Price { get; set; }

        [DisplayName("Обложка книги")]
        public string? Image { get; set; }

        [DisplayName("Категория книги")]
        [Required(ErrorMessage = "Выберите категорию")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null!;
        //  virtual позволяет использ. ленивую загрузку данных, что означает, что
        //  связанные данные будут загружаться только при необходимости
        // (навигационное свойство)

    }
}
