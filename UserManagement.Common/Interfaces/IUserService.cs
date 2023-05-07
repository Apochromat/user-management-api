using UserManagement.Common.DTO;

namespace UserManagement.Common.Interfaces; 

/// <summary>
/// Interface for user service
/// </summary>
public interface IUserService {
    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    Task Register(RegisterDto registerDto);
    /// <summary>
    /// Gets a single user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserDto> GetUser(Guid id);
    /// <summary>
    /// Gets a couple users with pagination
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<Pagination<UserDto>> GetAllUsers(int page, int pageSize = 10);
    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteUser(Guid id);
    /// <summary>
    /// Logs in a user
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="userIdentifier"></param>
    /// <returns></returns>
    Task<Guid?> Login(string username, string password);
}