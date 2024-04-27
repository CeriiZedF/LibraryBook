using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

using ShopLibrary.DAL.Repository.IRepository;
using ShopLibrary.Models;

namespace ShopLibrary.Areas.API
{
    [ApiController]
    [Route("api/[controller]/")]
    [Authorize]
    public class BookController : ControllerBase
    {
        
    }
}
