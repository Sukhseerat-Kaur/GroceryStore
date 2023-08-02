using AutoMapper;
using GroceryStore.DataLayer.Entities;
using GroceryStoreCore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreFacade.Utilities.Mapper
{
    public class ProductMapper
    {
        public static ProductWithCategoriesDto ProductDtoToProductDtoWithCategories(ProductDto productDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductDto, ProductWithCategoriesDto>());
            var mapper = config.CreateMapper();

            ProductWithCategoriesDto product = mapper.Map<ProductWithCategoriesDto>(productDto);
            return product;
        }

        public static List<ProductWithCategoriesDto > ProductDtoListToProductDtoWithCategoriesList(List<ProductDto> productDtos)
        {
            List<ProductWithCategoriesDto> products = new List<ProductWithCategoriesDto>();
            foreach (var productDto in productDtos)
            {
                products.Add(ProductMapper.ProductDtoToProductDtoWithCategories(productDto));
            }
            return products;
        }

        public static CartProductDto ProductDtoToCartProductDto(ProductDto productDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductDto, CartProductDto>());
            var mapper = config.CreateMapper();

            CartProductDto product = mapper.Map<CartProductDto>(productDto);
            return product;
        }
    }
}
