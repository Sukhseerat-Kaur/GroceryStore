using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using GroceryStoreCore.DTOs;
using GroceryStoreBackEnd.ViewModels;
using GroceryStoreBackEnd.Utilities.Mapper;
using GroceryStoreFacade.FacadeInterfaces;
using GroceryStoreCore.Filters;
using FluentValidation;
using FluentValidation.Results;

namespace GroceryStoreBackEnd.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [ServiceFilter(typeof(GroceryStoreExceptionFilterAttribute))]
    public class UserController : ControllerBase
    {
        private readonly IFacade _facade;
        private readonly IValidator<UserViewModel> _userValidator;
        private readonly IValidator<UserLoginViewModel> _userLoginValidator;

        public UserController(IFacade facade, IValidator<UserViewModel> userValidator, IValidator<UserLoginViewModel> userLoginValidator)
        {
            _facade = facade;
            _userValidator = userValidator;
            _userLoginValidator = userLoginValidator;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginViewModel userObj)
        {
            ValidationResult result = await _userLoginValidator.ValidateAsync(userObj);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => new { error.ErrorCode, error.ErrorMessage }).ToList();

                return BadRequest(new
                {
                    Message = errorMessages,
                    Error = "Login failed"
                });
            }
            try
            {
                UserDto userDto = UserMapper.UserLoginViewModelToDto(userObj);


                var userWithToken = await _facade.Authenticate(userDto);
                return Ok(new
                {
                    Message = "Login Success!",
                    User = new
                    {
                        userId = userWithToken.UserId,
                        userEmail = userWithToken.UserEmail,
                        userName = userWithToken.UserName,
                        phoneNumber = userWithToken.PhoneNumber,
                        userRole = userWithToken.UserRole
                    },
                    token = userWithToken.Token
                });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new
                {
                    ex.Message,
                    Error = "Login failed"
                });
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserViewModel userObj)
        {
            ValidationResult result = await _userValidator.ValidateAsync(userObj);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => new { error.ErrorCode, error.ErrorMessage }).ToList();

                return BadRequest(new
                {
                    Message = errorMessages,
                    Error = "Registration failed."
                });
            }
            UserDto userDto = UserMapper.UserViewModelToDto(userObj);
            var userId = await _facade.AddUser(userDto);
            return Ok(new
            {
                Message = "User Added Succesfully",
                User = new
                {
                    userId,
                    userEmail = userDto.UserEmail,
                    userName = userDto.UserName,
                    phoneNumber = userDto.PhoneNumber,
                    userRole = userDto.UserRole
                }
            });
        }

        [HttpPost("add-admin")]
        [Authorize(Roles = "superAdmin")]
        public async Task<IActionResult> AddAdmin([FromBody] UserViewModel userObj)
        {
            ValidationResult result = await _userValidator.ValidateAsync(userObj);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => new { error.ErrorCode, error.ErrorMessage }).ToList();

                return BadRequest(new
                {
                    Message = errorMessages,
                    Error = "Registration failed."
                });
            }
            UserDto userDto = UserMapper.UserViewModelToDto(userObj);
            var userId = await _facade.AddUser(userDto);
            return Ok(new
            {
                Message = "User Added Succesfully",
                User = new
                {
                    userId,
                    userEmail = userDto.UserEmail,
                    userName = userDto.UserName,
                    phoneNumber = userDto.PhoneNumber,
                    userRole = userDto.UserRole
                }
            });
        }

        [HttpGet("get-admins")]
        [Authorize(Roles = "superAdmin")]
        public async Task<ActionResult<List<UserViewModel>>> GetAllAdmins()
        {
            var adminDtoList = await _facade.GetAllAdmins();
            var admins = UserMapper.UserDtoListToViewModelList(adminDtoList);
            return Ok(admins);
        }

        [HttpDelete("remove-admin")]
        [Authorize(Roles = "superAdmin")]
        public async Task<IActionResult> RemoveAdmin([FromQuery] int userId){
            try
            {
                await _facade.RemoveAdminAccess(userId);
                return Ok(new {
                    Message= $"Admin Access for User Id {userId} is removed.",
                });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new
                {
                    ex.Message,
                    Error = "Cannot remove Admin Access"
                });
            }
        }
    }
}
