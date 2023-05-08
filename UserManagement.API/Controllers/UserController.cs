using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Common.DTO;
using UserManagement.Common.Interfaces;

namespace UserManagement.API.Controllers;

/// <summary>
/// Controller for managing users
/// </summary>
[ApiController]
[Route("api")]
public class UserController : ControllerBase {
    private readonly IUserService _userService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="userService"></param>
    public UserController(IUserService userService) {
        _userService = userService;
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto registerDto) {
        await _userService.Register(registerDto);
        return Ok();
    }
    
    /// <summary>
    /// Returns all users with pagination
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("users")]
    public async Task<ActionResult<Pagination<UserDto>>> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10) {
        return Ok(await _userService.GetAllUsers(page, pageSize));
    }
    
    /// <summary>
    /// Returns a single user
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("user/{id}")]
    public async Task<ActionResult<UserDto>> GetUserById([FromRoute] Guid id) {
        return Ok(await _userService.GetUser(id));
    }
    
    /// <summary>
    /// [Authorize] Returns an authenticated user
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(AuthenticationSchemes = "Basic")]
    [Route("user/me")]
    public async Task<ActionResult<UserDto>> GetUser() {
        if (User.Identity?.Name == null) return Unauthorized();
        return Ok(await _userService.GetUser(Guid.Parse(User.Identity.Name)));
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <returns></returns>
    [HttpDelete]
    [Route("user/{id}")]
    public async Task<ActionResult> DeleteUser([FromRoute] Guid id) {
        await _userService.DeleteUser(id);
        return Ok();
    }
}