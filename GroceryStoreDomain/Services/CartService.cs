using GroceryStore.DataLayer.UnitOfWork.Interfaces;
using GroceryStoreCore.DTOs;
using GroceryStore.DataLayer.Utilities.Mapper;
using GroceryStoreDomain.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace GroceryStoreDomain.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        private readonly ILogger<CartService> _logger;
        public CartService(IUnitOfWork unitOfWork, IProductService productService, ILogger<CartService> logger)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _logger = logger;
        }
        public async Task AddToCart(CartDto cartDto)
        {
            var availableQuantity = await _productService.GetProductAvailableQuantity(cartDto.ProductId);
            if(availableQuantity < cartDto.Quantity)
            {
                throw new InvalidOperationException($"Not enough stock available for {cartDto.ProductId}");
            }
            if (await _unitOfWork.CartRepository.ExistInCart(cartDto.UserId, cartDto.ProductId))
            {
                throw new InvalidOperationException("Proudct is already present in cart.");
            }
            await _unitOfWork.CartRepository.AddToCart(cartDto);
        }

        public async Task<List<CartDto>> GetCartByUserId(int userId)
        {
            return await _unitOfWork.CartRepository.GetCartByUserId(userId);

        }

        public async Task RemoveFromCart(int userId, int productId)
        {
           await _unitOfWork.CartRepository.RemoveFromCart(userId, productId);
        }

        public async Task UpdateQuantity(int userId, int productId, int newQuantity)
        {
            try
            {
                var availableQuantity = await _productService.GetProductAvailableQuantity(productId);
                if (newQuantity > availableQuantity)
                {
                    throw new ArgumentException("Item Quantity cannot exceeds its available stocks");
                }
                await _unitOfWork.CartRepository.UpdateQuantity(userId, productId, newQuantity);
            }catch(ArgumentException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
