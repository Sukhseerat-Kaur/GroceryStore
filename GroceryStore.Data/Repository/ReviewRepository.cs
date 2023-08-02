using Microsoft.EntityFrameworkCore;

using GroceryStore.DataLayer.Context;
using GroceryStore.DataLayer.Entities;
using GroceryStore.DataLayer.Repository.Interfaces;
using GroceryStoreCore.DTOs;
using GroceryStore.DataLayer.Utilities.Mapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace GroceryStore.DataLayer.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly GroceryStoreDbContext _dbContext;
        private readonly ILogger<ReviewRepository> _logger;

        public ReviewRepository(GroceryStoreDbContext dbContext, ILogger<ReviewRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task AddReview(ReviewDto review)
        {
            var reviewEntity = ReviewMapper.ReviewDtoToEntity(review);
            await _dbContext.Reviews.AddAsync(reviewEntity);
            await _dbContext.SaveChangesAsync();
            _logger.LogDebug($"User with ID {review.UserId} added a review for Product with Id {review.ProductId} at time {DateTime.Now}");
        }

        public async Task DeleteReview(int userId, int productId, DateTime time)
        {
            try
            {
                var review = await _dbContext.Reviews.FirstOrDefaultAsync(review => review.UserId == userId && review.ProductId == productId && review.Time == time);
                if (review == null)
                {
                    throw new KeyNotFoundException($"Could not find Review from User with id {userId} on product with id {productId}");
                }
                _dbContext.Reviews.Remove(review);
                await _dbContext.SaveChangesAsync();

                _logger.LogWarning($"Review on product with product Id {productId} by user with user Id {userId} is removed with review {review.ReviewString}");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }
        }

        public async Task<List<ReviewDto>> GetReviewsOfProduct(int productId)
        {
            var reviews = await _dbContext.Reviews.Where(review => review.ProductId == productId).ToListAsync();
            var reviewDtoList = ReviewMapper.ReviewEntityListToDtoList(reviews);
            _logger.LogInformation($"Reviews for product with Id {productId} is fetched at time {DateTime.Now}");
            return reviewDtoList;
        }

        public async Task UpdateReview(ReviewDto oldReview, ReviewDto newReview)
        {
            await this.DeleteReview(oldReview.UserId, oldReview.ProductId, oldReview.Time);
            await this.AddReview(newReview);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ReviewDto> GetReview(int userId, int productId, DateTime time)
        {
            try
            {
                var review = await _dbContext.Reviews.FirstOrDefaultAsync(review => review.UserId == userId && review.ProductId == productId && review.Time == time);
                if (review == null)
                {
                    throw new KeyNotFoundException($"Could not find Review from User with id {userId} on product with id {productId}");
                }
                var reviewDto = ReviewMapper.ReviewEntityToDto(review);
                return reviewDto;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }
        }
    }
}