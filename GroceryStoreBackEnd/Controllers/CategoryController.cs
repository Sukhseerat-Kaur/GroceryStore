using GroceryStoreCore.Filters;
using GroceryStoreDomain.Services.Interfaces;
using GroceryStoreFacade.FacadeInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStoreBackEnd.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    [ServiceFilter(typeof(GroceryStoreExceptionFilterAttribute))]

    public class CategoryController : ControllerBase
    {
        private readonly IFacade _facade;
        public CategoryController(IFacade facade)
        {
            _facade = facade;
        }

        [HttpGet("get-all")]
        [AllowAnonymous]
        public async Task<string[]> GetAllCategories()
        {
            return await _facade.GetAllCategories();
        }
    }
}
