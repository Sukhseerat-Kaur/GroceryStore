using AutoMapper;

using GroceryStoreCore.DTOs;
using GroceryStore.DataLayer.Entities;

namespace GroceryStore.DataLayer.Utilities.Mapper
{
    public class ProductCategoryMapper
    {
        public static ProductCategoryEntity ProductCategoryDtoToEntity(ProductCategoryDto productCategoryDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductCategoryDto, ProductCategoryEntity>());
            var mapper = config.CreateMapper();

            ProductCategoryEntity productCategory = mapper.Map<ProductCategoryEntity>(productCategoryDto);

            return productCategory;
        }

        public static ProductCategoryDto ProductCategoryEntityToDto(ProductCategoryEntity productCategory)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductCategoryEntity, ProductCategoryDto>());
            var mapper = config.CreateMapper();

            ProductCategoryDto productCategoryDto = mapper.Map<ProductCategoryDto>(productCategory);
            return productCategoryDto;
        }
    }
}
