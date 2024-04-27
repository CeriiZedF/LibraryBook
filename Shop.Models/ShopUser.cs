using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace ShopLibrary.Models
{
    public class ShopUser : IdentityUser
    {
        [DisplayName("ФИО")]
        public string FullName { get; set; } = null!;

        [DisplayName("Адрес доставки")]
        public string AddressDelivery { get; set; } = null!;
    }
}
