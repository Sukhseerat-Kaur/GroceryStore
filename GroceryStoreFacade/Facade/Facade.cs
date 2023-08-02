using GroceryStore.DataLayer.UnitOfWork.Interfaces;
using GroceryStoreCore.DTOs;
using GroceryStoreDomain.Services.Interfaces;
using GroceryStoreFacade.FacadeInterfaces;
using GroceryStoreFacade.Utilities.Mapper;
using Microsoft.AspNetCore.Http;

namespace GroceryStoreFacade.Facade
{
    public class Facade : IFacade
    {
        private readonly IImageService _imageService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ICartService _cartService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductService _productService;
        private readonly IReviewService _reviewService;

        private readonly IUnitOfWork _unitOfWork;

        public Facade(IImageService imageService, IUserService userService, IAuthService authService, ICartService cartService, ICategoryService categoryService, IOrderService orderService, IProductCategoryService productCategoryService, IProductService productService, IReviewService reviewService, IUnitOfWork unitOfWork)
        {
            _imageService = imageService;
            _userService = userService;
            _authService = authService;
            _cartService = cartService;
            _categoryService = categoryService;
            _orderService = orderService;
            _productCategoryService = productCategoryService;
            _productService = productService;
            _reviewService = reviewService;
            _unitOfWork = unitOfWork;
        }


        // *************************PRODUCTS **********************************
        public async Task<int> AddProduct(ProductDto product, IFormFile imageFile, string categories, string contentRootPath)
        {
            try
            {
                var filePath = _imageService.GetFilePath(contentRootPath, imageFile);

                _imageService.uploadFile(filePath, imageFile);

                product.ImagePath = filePath;

                await _unitOfWork.CreateTransaction();

                var productId = await _productService.AddProduct(product);

                await _productCategoryService.AddProductCategories(productId, categories);

                await _unitOfWork.SaveAsync();
                await _unitOfWork.Commit();
                return productId;
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<List<ProductWithCategoriesDto>> GetAllProducts()
        {
            var productDtos = await _productService.GetAllProducts();

            var productsWithCategoriesList = ProductMapper.ProductDtoListToProductDtoWithCategoriesList(productDtos);
            foreach (var product in productsWithCategoriesList)
            {
                var productId = product.ProductId;
                string[] categories = await _productCategoryService.GetAllCategoriesByProductId(productId);

                var categoriesString = string.Join(",", categories);
                product.ProductCategories = categoriesString;
            }
            return productsWithCategoriesList;
        }

        public async Task<List<ProductWithCategoriesDto>> GetAvailableProducts()
        {
            var productDtos = await _productService.GetAllProducts();

            productDtos = productDtos.Where(product => product.IsDeleted == false).ToList();

            var productsWithCategoriesList = ProductMapper.ProductDtoListToProductDtoWithCategoriesList(productDtos);
            foreach (var product in productsWithCategoriesList)
            {
                var productId = product.ProductId;
                string[] categories = await _productCategoryService.GetAllCategoriesByProductId(productId);

                var categoriesString = string.Join(",", categories);
                product.ProductCategories = categoriesString;
            }
            return productsWithCategoriesList;
        }

        public async Task<ImageBytesAndTypeDto> GetFileBytesAndType(int productId)
        {
            try
            {
                var product = await _productService.GetProductById(productId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Cannot find product with Id {productId}");
                }
                var filePath = product.ImagePath;

                var bytes = _imageService.GetFileBytes(filePath);
                var contentType = _imageService.GetImageContentType(filePath);
                return new ImageBytesAndTypeDto { Bytes = bytes, ContentType = contentType };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteProduct(int productId)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                await _productService.DeleteProduct(productId);
                await _productCategoryService.DeletedCategoriesWithProductId(productId);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task UpdateProduct(ProductDto product, IFormFile imageFile, string categories, string contentRootPath)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                var filePath = _imageService.GetFilePath(contentRootPath, imageFile);
                _imageService.uploadFile(filePath, imageFile);

                product.ImagePath = filePath;

                await _productService.UpdateProduct(product);

                await _productCategoryService.DeletedCategoriesWithProductId(product.ProductId);
                await _productCategoryService.AddProductCategories(product.ProductId, categories);

                await _unitOfWork.SaveAsync();
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<ProductWithCategoriesDto> GetProductById(int productId)
        {
            try
            {
                var productDto = await _productService.GetProductById(productId);
                if (productDto == null)
                {
                    throw new KeyNotFoundException($"Product cannot be found with this {productId} id");
                }
                var productWithCategories = ProductMapper.ProductDtoToProductDtoWithCategories(productDto);

                var categories = await _productCategoryService.GetAllCategoriesByProductId(productId);

                var categoriesString = string.Join(',', categories);

                productWithCategories.ProductCategories = categoriesString;
                return productWithCategories;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> GetPriceOfProduct(int productId)
        {
            var product = await this.GetProductById(productId);
            return product.ProductPrice;
        }

        public async Task<int> GetAvailableQuantity(int productId)
        {
            var product = await this.GetProductById(productId);
            return product.ProductQuantity;
        }


        //*************** USER ********************
        public async Task<UserWithTokenDto> Authenticate(UserDto userDto)
        {
            var password = userDto.Password;

            UserWithoutPasswordDto userWithoutPassword = await _userService.GetUser(userDto.UserEmail, password);

            userDto = UserMapper.UserWithoutPasswordToUserDto(userWithoutPassword, password);
            string token = _authService.Authenticate(userDto);


            UserWithTokenDto userWithTokenDto = UserMapper.UserWithoutPasswordToUserWithTokenDto(userWithoutPassword, token);

            return userWithTokenDto;

        }

        public async Task<int> AddUser(UserDto userDto)
        {
            return await _userService.AddUser(userDto);
        }

        public async Task RemoveAdminAccess(int userId)
        {
            await _userService.RemoveAdminAccess(userId);
        }

        public async Task<List<UserDto>> GetAllAdmins()
        {
            return await _userService.GetAllAdmins();
        }

        //********************* CART *************
        public async Task<List<CartProductDto>> GetCartByUserId(int userId)
        {
            List<CartDto> cartDtos = await _cartService.GetCartByUserId(userId);
            List<CartProductDto> cartProducts = new List<CartProductDto>();

            foreach (var cartItem in cartDtos)
            {
                ProductDto productDto = await _productService.GetProductById(cartItem.ProductId);
                CartProductDto cartProductDto = ProductMapper.ProductDtoToCartProductDto(productDto);
                cartProductDto.ProductQuantityInCart = cartItem.Quantity;
                cartProductDto.IsDeleted = productDto.IsDeleted;
                cartProducts.Add(cartProductDto);
            }
            return cartProducts;
        }

        public async Task AddToCart(CartDto cartDto)
        {
            await _cartService.AddToCart(cartDto);
        }

        public async Task RemoveFromCart(int userId, int productId)
        {
            await _cartService.RemoveFromCart(userId, productId);
        }

        public async Task UpdateCartItemQuantity(int userId, int productId, int quantity)
        {
            await _cartService.UpdateQuantity(userId, productId, quantity);
        }


        //********************* ORDERS *************
        public async Task<Guid> PlaceOrder(List<OrderDto> orders)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                var currentTime = DateTime.Now;
                var newOrderId = Guid.NewGuid();
                foreach (var orderDto in orders)
                {
                    orderDto.OrderDateTime = currentTime;
                    orderDto.OrderId = newOrderId;
                    var availableQuantity = await _productService.GetProductAvailableQuantity(orderDto.ProductId);
                    if (orderDto.Quantity > availableQuantity)
                    {
                        throw new InvalidOperationException("Order quantity exceeds availability");
                    }
                    await _orderService.PlaceOrder(orderDto);

                    await _productService.UpdateQuantity(orderDto.ProductId, availableQuantity - orderDto.Quantity);

                    await _cartService.RemoveFromCart(orderDto.UserId, orderDto.ProductId);
                }

                await _unitOfWork.SaveAsync();
                await _unitOfWork.Commit();
                return newOrderId;
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<OrderDto> GetOrderByOrderId(Guid orderId)
        {
            return await _orderService.GetOrderByOrderId(orderId);
        }

        public async Task<List<OrderProductDto>> GetOrdersListByUserId(int userId)
        {
            var orders = await _orderService.GetOrdersByUserId(userId);

            List<OrderProductDto> orderByUser = new List<OrderProductDto>();
            foreach (var order in orders)
            {
                var orderProductDetails = OrderMapper.OrderDtoToOrderProductDto(order);
                var product = await _productService.GetProductById(orderProductDetails.ProductId);

                orderProductDetails.ProductName = product.ProductName;
                orderProductDetails.ProductDescription = product.ProductDescription;
                orderProductDetails.IsDeleted = product.IsDeleted;
                orderProductDetails.ImagePath = product.ImagePath;

                orderByUser.Add(orderProductDetails);
            }
            return orderByUser;
        }

        public async Task<List<ProductWithCategoriesDto>> GetMostOrderedProductsByMonthAndYear(int year, int month, int quantity)
        {
            var validProducts = await _orderService.GetMostOrderedProductsByMonthAndYear(year, month, quantity);
            List<ProductWithCategoriesDto> productWithCategoriesDtos = new List<ProductWithCategoriesDto>();

            foreach (var product in validProducts)
            {
                var productDto = await _productService.GetProductById(product.ProductId);

                var productWithCategory = ProductMapper.ProductDtoToProductDtoWithCategories(productDto);

                productWithCategory.ProductQuantity = quantity;

                productWithCategoriesDtos.Add(productWithCategory);
            }
            return productWithCategoriesDtos;
        }


        public async Task<string[]> GetAllCategories()
        {
            return await _categoryService.GetAllCategories();
        }


        //**********************REVIEWS***********************

        public async Task AddReview(ReviewDto review)
        {
            await _reviewService.AddReview(review);
        }

        public async Task DeleteReview(int requestUserId, int userId, int productId, DateTime time)
        {
            var role = await _userService.GetUserRole(requestUserId);
            if (role == "admin" || role == "superAdmin" || requestUserId == userId)
            {
                await _reviewService.DeleteReview(userId, productId, time);
            }
            else throw new UnauthorizedAccessException("You don't have right access to perform this action.");
        }

        public async Task<List<ReviewUserDto>> GetReviewsOfProduct(int productId)
        {
            var reviewDtoList = await _reviewService.GetReviewsOfProduct(productId);
            List<ReviewUserDto> reviews = new List<ReviewUserDto>();
            foreach (var reviewDto in reviewDtoList)
            {
                ReviewUserDto review = new ReviewUserDto
                {
                    UserId = reviewDto.UserId,
                    ProductId = productId,
                    Time = reviewDto.Time,
                    ReviewString = reviewDto.ReviewString
                };
                var user = await _userService.GetUserById(reviewDto.UserId);
                review.UserName = user.UserName;
                review.UserEmail = user.UserEmail;

                reviews.Add(review);
            }
            return reviews;
        }

        public async Task UpdateReview(int userId, int productId, DateTime time, ReviewDto newReview)
        {
            var oldReview = await _reviewService.GetReview(userId, productId, time);
            await _reviewService.UpdateReview(oldReview, newReview);
        }

        public async Task<ReviewDto> GetReview(int userId, int productId, DateTime time)
        {
            return await _reviewService.GetReview(userId, productId, time);
        }
    }
}
