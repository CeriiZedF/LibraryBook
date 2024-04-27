using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopLibrary.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int BookId { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("UserId")]
        public virtual ShopUser ShopUser { get; set; } = null!;

        [ForeignKey("BookId")]
        public virtual Book Book{ get; set; } = null!;
    }
}
