using GroceryStoreCore.DTOs;

namespace GroceryStoreDomain.Services.Interfaces
{
    public interface IReviewService
    {
        Task AddReview(ReviewDto review);

        Task DeleteReview(int userId, int productId, DateTime time);

        Task<List<ReviewDto>> GetReviewsOfProduct(int productId);

        Task UpdateReview(ReviewDto oldReview, ReviewDto newReview);

        Task<ReviewDto> GetReview(int userId, int productId, DateTime time);
    }
}