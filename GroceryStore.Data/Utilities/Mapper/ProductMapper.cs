using AutoMapper;

using GroceryStoreCore.DTOs;
using GroceryStore.DataLayer.Entities;

namespace GroceryStore.DataLayer.Utilities.Mapper
{
    public class ProductMapper
    {
        public static ProductEntity ProductDtoToEntity(ProductDto productDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductDto, ProductEntity>());
            var mapper = config.CreateMapper();

            ProductEntity product = mapper.Map<ProductEntity>(productDto);
            return product;
        }

        public static ProductDto ProductEntityToDto(ProductEntity product)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductEntity, ProductDto>());
            var mapper = config.CreateMapper();

            ProductDto productDto = mapper.Map<ProductDto>(product);
            return productDto;
        }

        public static List<ProductDto> ProductEntityListToDtoList(List<ProductEntity> productEntities)
        {
            List<ProductDto> productDtos = new List<ProductDto>();
            foreach(var productEntity in productEntities)
            {
                productDtos.Add(ProductMapper.ProductEntityToDto(productEntity));
            }
            return productDtos;
        }
    }
}
