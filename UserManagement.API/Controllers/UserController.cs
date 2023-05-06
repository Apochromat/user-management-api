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
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    public UserController(ILogger<UserController> logger, IUserService userService) {
        _logger = logger;
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
    public async Task<ActionResult<Pagination<UserDto>>> GetAllUsers([FromQuery] int page = 1) {
        return Ok(await _userService.GetAllUsers(page));
    }
    
    /// <summary>
    /// Returns a single user
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("user/{id}")]
    public async Task<ActionResult<UserDto>> GetUser([FromRoute] Guid id) {
        return Ok(await _userService.GetUser(id));
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