using GroceryStore.DataLayer.UnitOfWork.Interfaces;
using GroceryStoreCore.DTOs;
using GroceryStoreDomain.Services.Interfaces;

namespace GroceryStoreDomain.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddReview(ReviewDto review)
        {
            await _unitOfWork.ReviewRepository.AddReview(review);
        }

        public async Task DeleteReview(int userId, int productId, DateTime time)
        {
            await _unitOfWork.ReviewRepository.DeleteReview(userId, productId, time);
        }

        public async Task<List<ReviewDto>> GetReviewsOfProduct(int productId)
        {
            return await _unitOfWork.ReviewRepository.GetReviewsOfProduct(productId);
        }

        public async Task UpdateReview(ReviewDto oldReview, ReviewDto newReview)
        {
            await _unitOfWork.ReviewRepository.UpdateReview(oldReview, newReview);
        }

        public async Task<ReviewDto> GetReview(int userId, int productId, DateTime time)
        {
            return await _unitOfWork.ReviewRepository.GetReview(userId, productId, time);
        }
    }
}