using AutoMapper;

using GroceryStoreBackEnd.ViewModels;
using GroceryStoreCore.DTOs;

namespace GroceryStoreBackEnd.Utilities.Mapper
{
    public class ProductMapper
    {

        public static ProductDto ProductViewModelToDto(ProductViewModel productViewModel)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductViewModel, ProductDto>());
            var mapper = config.CreateMapper();

            ProductDto productDto = mapper.Map<ProductDto>(productViewModel);
            return productDto;
        }

        public static ProductViewModel ProductDtoToViewModel(ProductDto productDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductDto, ProductViewModel>());
            var mapper = config.CreateMapper();

            ProductViewModel productViewModel = mapper.Map<ProductViewModel>(productDto);
            return productViewModel;
        }
        public static List<ProductViewModel> ProductDtoListToViewModelList(List<ProductDto> productDtos)
        {
            List<ProductViewModel> productViewModels = new List<ProductViewModel>();

            foreach (var productDto in productDtos)
            {
                productViewModels.Add(ProductMapper.ProductDtoToViewModel(productDto));
            }

            return productViewModels;
        }

        public static ProductViewModelWithImage ProductDtoToViewModelWithImage(ProductDto productDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductDto, ProductViewModelWithImage>());
            var mapper = config.CreateMapper();

            ProductViewModelWithImage productViewModel = mapper.Map<ProductViewModelWithImage>(productDto);
            return productViewModel;
        }

        public static List<ProductViewModelWithImage> ProductDtoListToViewModelWithImageList(List<ProductDto> productDtos)
        {
            List<ProductViewModelWithImage> productViewModels = new List<ProductViewModelWithImage>();

            foreach (var productDto in productDtos)
            {
                productViewModels.Add(ProductMapper.ProductDtoToViewModelWithImage(productDto));
            }

            return productViewModels;
        }

        public static ProductDto ProductCreationViewModelToDto(ProductCreationViewModel productCreationViewModel)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductCreationViewModel, ProductDto>());
            var mapper = config.CreateMapper();

            ProductDto productDto = mapper.Map<ProductDto>(productCreationViewModel);
            return productDto;
        }

        public static CartProductViewModel ProductDtoToCartProductViewModel(ProductDto productDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductDto, CartProductViewModel>());
            var mapper = config.CreateMapper();

            CartProductViewModel cartProductViewModel = mapper.Map<CartProductViewModel>(productDto);
            return cartProductViewModel;
        }

        public static ProductViewModelWithImage ProductDtoToProductViewModelWithImage(ProductDto productDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductDto, ProductViewModelWithImage>());
            var mapper = config.CreateMapper();

            ProductViewModelWithImage productView = mapper.Map<ProductViewModelWithImage>(productDto);
            return productView;
        }

        public static ProductViewModelWithImage ProductWithCategoryDtoToViewModelWithImage(ProductWithCategoriesDto productDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductWithCategoriesDto, ProductViewModelWithImage>());
            var mapper = config.CreateMapper();

            ProductViewModelWithImage productViewModel = mapper.Map<ProductViewModelWithImage>(productDto);
            return productViewModel;
        }

        public static List<ProductViewModelWithImage> ProductWithCategoryDtoListToViewModelWithImageList(List<ProductWithCategoriesDto> productDtos)
        {
            List<ProductViewModelWithImage> productViewModels = new List<ProductViewModelWithImage>();

            foreach (var productDto in productDtos)
            {
                productViewModels.Add(ProductMapper.ProductWithCategoryDtoToViewModelWithImage(productDto));
            }

            return productViewModels;
        }

        public static CartProductViewModel CartProductDtoToCartProductViewModel(CartProductDto cartProductDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CartProductDto, CartProductViewModel>());
            var mapper = config.CreateMapper();

            CartProductViewModel cartProductViewModel = mapper.Map<CartProductViewModel>(cartProductDto);
            return cartProductViewModel;
        }

        public static List<CartProductViewModel> CartProductDtoListToViewModelList(List<CartProductDto> cartProductDtos)
        {
            List<CartProductViewModel> cartProductViewModels = new List<CartProductViewModel>();

            foreach (var cartProductDto in cartProductDtos)
            {
                cartProductViewModels.Add(ProductMapper.CartProductDtoToCartProductViewModel(cartProductDto));
            }

            return cartProductViewModels;
        }
    }
}
