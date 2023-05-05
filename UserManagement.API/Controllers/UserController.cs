using Microsoft.AspNetCore.Mvc;

namespace UserManagement.API.Controllers;

/// <summary>
/// Controller for managing users
/// </summary>
[ApiController]
[Route("api")]
public class UserController : ControllerBase {
    private readonly ILogger<UserController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    public UserController(ILogger<UserController> logger) {
        _logger = logger;
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Register() {
        return Ok();
    }
    
    /// <summary>
    /// Returns all users with pagination
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("users")]
    public async Task<ActionResult> GetAllUsers() {
        return Ok();
    }
    
    /// <summary>
    /// Returns a single user
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("user/{id}")]
    public async Task<ActionResult> GetUser() {
        return Ok();
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <returns></returns>
    [HttpDelete]
    [Route("user/{id}")]
    public async Task<ActionResult> DeleteUser() {
        return Ok();
    }
}