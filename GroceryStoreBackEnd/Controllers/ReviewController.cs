using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GroceryStoreBackEnd.ViewModels;
using GroceryStoreBackEnd.Utilities.Mapper;
using GroceryStoreCore.DTOs;
using GroceryStoreFacade.FacadeInterfaces;
using GroceryStoreCore.Filters;
using System.Data;
using FluentValidation;
using FluentValidation.Results;
using GroceryStore.DataLayer.UnitOfWork.Interfaces;

namespace GroceryStoreBackEnd.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [ServiceFilter(typeof(GroceryStoreExceptionFilterAttribute))]
    public class ReviewController : ControllerBase
    {
        private readonly IFacade _facade;
        private readonly IUnitOfWork _unitOfWork;
        public ReviewController(IFacade facade, IUnitOfWork unitOfWork)
        {
            _facade = facade;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("get-reviews")]
        public async Task<ActionResult<IEnumerable<ReviewUserViewModel>>> GetReviewsOfProduct([FromQuery] int productId)
        {
            var reviewDtoList = await _facade.GetReviewsOfProduct(productId);
            var reviews = ReviewMapper.ReviewUserDtoListToReviewUserViewList(reviewDtoList);
            return reviews;
        }

        [HttpPost("add-review")]
        [Authorize(Roles = "user, admin, superAdmin")]
        public async Task<IActionResult> AddReview([FromBody] ReviewViewModel review)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                ReviewDto reviewDto = ReviewMapper.ReviewViewModelToDto(review);
                await _facade.AddReview(reviewDto);

                await _unitOfWork.SaveAsync();
                await _unitOfWork.Commit();
                return Ok(reviewDto);
            }
            catch (UnauthorizedAccessException ex) {
                await _unitOfWork.Rollback();
                return Unauthorized(new
                {
                    ex.Message,
                    Error="UnAuthorized Access"
                });
            }catch(KeyNotFoundException ex)
            {
                await _unitOfWork.Rollback();
                return BadRequest(new
                {
                    ex.Message,
                    Error = "Cannot Add the review",
                });
            }
        }

        [HttpPut("update-review")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> UpdateReview([FromQuery] int userId, [FromQuery] int productId, [FromQuery] DateTime time, [FromBody] ReviewViewModel newReview)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                ReviewDto newReviewDto = ReviewMapper.ReviewViewModelToDto(newReview);

                await _facade.UpdateReview(userId, productId, time, newReviewDto);

                await _unitOfWork.SaveAsync();
                await _unitOfWork.Commit();

                return Ok(newReview);

            }
            catch (UnauthorizedAccessException ex)
            {
                await _unitOfWork.Rollback();
                return Unauthorized(new
                {
                    ex.Message,
                    Error = "UnAuthorized Access"
                });
            }
            catch (KeyNotFoundException ex)
            {
                await _unitOfWork.Rollback();
                return Unauthorized(new
                {
                    ex.Message,
                    Error = "Cannot update the review",
                });
            }
        }

        [HttpDelete("delete-review")]
        [Authorize(Roles = "admin, superAdmin, user")]
        public async Task<IActionResult> DeleteReview([FromQuery] int fromUserId, [FromQuery] int userId, [FromQuery] int productId, [FromQuery] DateTime time)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                await _facade.DeleteReview(fromUserId,userId, productId, time);

                await _unitOfWork.SaveAsync();
                await _unitOfWork.Commit();

                return Ok(new
                {
                    Message = "Review Deleted Successfully"
                }) ;

            }
            catch (UnauthorizedAccessException ex)
            {
                await _unitOfWork.Rollback();
                return Unauthorized(new
                {
                    ex.Message,
                    Error = "UnAuthorized Access"
                });
            }
            catch (KeyNotFoundException ex)
            {
                await _unitOfWork.Rollback();
                return Unauthorized(new
                {
                    ex.Message,
                    Error = "Coudnot delete review",
                });
            }
        }
    }

}