using AutoMapper;

using GroceryStoreBackEnd.ViewModels;
using GroceryStoreCore.DTOs;

namespace GroceryStoreBackEnd.Utilities.Mapper
{
    public class ReviewMapper
    {
        public static ReviewDto ReviewViewModelToDto(ReviewViewModel reviewViewModel)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ReviewViewModel, ReviewDto>());
            var mapper = config.CreateMapper();

            ReviewDto reviewDto = mapper.Map<ReviewDto>(reviewViewModel);
            return reviewDto;
        }

        public static ReviewViewModel ReviewDtoToViewModel(ReviewDto reviewDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ReviewDto, ReviewViewModel>());
            var mapper = config.CreateMapper();

            ReviewViewModel reviewViewModel = mapper.Map<ReviewViewModel>(reviewDto);
            return reviewViewModel;
        }
        public static List<ReviewViewModel> ReviewDtoListToViewModelList(List<ReviewDto> reviewDtos)
        {
            List<ReviewViewModel> reviewViewModels = new List<ReviewViewModel>();

            foreach (var reviewDto in reviewDtos)
            {
                reviewViewModels.Add(ReviewMapper.ReviewDtoToViewModel(reviewDto));
            }

            return reviewViewModels;
        }


        public static ReviewUserDto ReviewUserViewModelToReviewUserDto(ReviewUserViewModel reviewViewModel)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ReviewUserViewModel, ReviewUserDto>());
            var mapper = config.CreateMapper();

            ReviewUserDto reviewDto = mapper.Map<ReviewUserDto>(reviewViewModel);
            return reviewDto;
        }

        public static ReviewUserViewModel ReviewDtoToViewModel(ReviewUserDto reviewDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ReviewUserDto, ReviewUserViewModel>());
            var mapper = config.CreateMapper();

            ReviewUserViewModel reviewViewModel = mapper.Map<ReviewUserViewModel>(reviewDto);
            return reviewViewModel;
        }

        public static List<ReviewUserViewModel> ReviewUserDtoListToReviewUserViewList(List<ReviewUserDto> reviewDtos)
        {
            List<ReviewUserViewModel> reviewViewModels = new List<ReviewUserViewModel>();

            foreach (var reviewDto in reviewDtos)
            {
                reviewViewModels.Add(ReviewMapper.ReviewDtoToViewModel(reviewDto));
            }
            return reviewViewModels;
        }
    }
}