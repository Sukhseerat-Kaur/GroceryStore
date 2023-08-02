using GroceryStoreCore.DTOs;

namespace GroceryStore.DataLayer.Repository.Interfaces
{
    public interface IReviewRepository
    {
        Task AddReview(ReviewDto review);

        Task<List<ReviewDto>> GetReviewsOfProduct(int productId);

        Task DeleteReview(int userId, int productId, DateTime time);

        Task UpdateReview(ReviewDto oldReview, ReviewDto newReview);

        Task<ReviewDto> GetReview(int userId, int productId, DateTime time);
    }
}
