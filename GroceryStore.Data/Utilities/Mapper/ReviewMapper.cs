using AutoMapper;

using GroceryStoreCore.DTOs;
using GroceryStore.DataLayer.Entities;

namespace GroceryStore.DataLayer.Utilities.Mapper
{
    public class ReviewMapper
    {
        public static ReviewDto ReviewEntityToDto(ReviewEntity review)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ReviewEntity, ReviewDto>());
            var mapper = config.CreateMapper();

            ReviewDto reviewDto = mapper.Map<ReviewDto>(review);
            return reviewDto;
        }

        public static ReviewEntity ReviewDtoToEntity(ReviewDto reviewDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ReviewDto, ReviewEntity>());
            var mapper = config.CreateMapper();

            ReviewEntity review = mapper.Map<ReviewEntity>(reviewDto);
            return review;
        }

        public static List<ReviewDto> ReviewEntityListToDtoList(List<ReviewEntity> reviewEntities)
        {
            var reviewDtos = new List<ReviewDto>();
            foreach(ReviewEntity reviewEntity in reviewEntities)
            {
                reviewDtos.Add(ReviewMapper.ReviewEntityToDto(reviewEntity));
            }
            return reviewDtos;
        }
    }
}