using GroceryStoreCore.DTOs;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GroceryStoreFacade.FacadeInterfaces
{
    public interface IFacade
    {
        // Product
        Task<int> AddProduct(ProductDto product, IFormFile imageFile, string categories, string contentRootPath);

        Task<List<ProductWithCategoriesDto>> GetAllProducts();
        Task<List<ProductWithCategoriesDto>> GetAvailableProducts();
        Task<ImageBytesAndTypeDto> GetFileBytesAndType(int productId);

        Task DeleteProduct(int productId);

        Task UpdateProduct(ProductDto product,  IFormFile imageFile, string categories, string contentRootPath);

        Task<ProductWithCategoriesDto> GetProductById(int productId);


        // User

        Task<UserWithTokenDto> Authenticate(UserDto userDto);

        Task<int> AddUser(UserDto userDto);

        Task<List<CartProductDto>> GetCartByUserId(int userId);

        Task RemoveAdminAccess(int userId);

        Task<List<UserDto>> GetAllAdmins();

        // Cart

        Task AddToCart(CartDto cartDto);

        Task RemoveFromCart(int userId, int productId);

        Task UpdateCartItemQuantity(int userId, int productId, int quantity);



        // Order
        Task<Guid> PlaceOrder(List<OrderDto> orders);


        Task<List<OrderProductDto>> GetOrdersListByUserId(int userId);

        Task<List<ProductWithCategoriesDto>> GetMostOrderedProductsByMonthAndYear(int year, int month, int quantity);

        Task<string[]> GetAllCategories();

        Task<OrderDto> GetOrderByOrderId(Guid orderId);

        Task<decimal> GetPriceOfProduct(int productId);

        Task<int> GetAvailableQuantity(int productId);

        Task AddReview(ReviewDto review);

        Task DeleteReview(int requestUserId, int userId, int productId, DateTime time);

        Task<List<ReviewUserDto>> GetReviewsOfProduct(int productId);

        Task UpdateReview(int userId, int productId, DateTime time, ReviewDto newReview);
    }
}
