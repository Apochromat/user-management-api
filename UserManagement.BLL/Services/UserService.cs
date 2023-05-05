using UserManagement.Common.DTO;
using UserManagement.Common.Interfaces;

namespace UserManagement.BLL.Services; 

/// <summary>
/// User management service
/// </summary>
public class UserService : IUserService {
    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="registerDto"></param>
    public async Task Register(RegisterDto registerDto) {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets a single user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UserDto> GetUser(Guid id) {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets a couple users with pagination
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async Task<Pagination<UserDto>> GetAllUsers(int page, int pageSize = 10) {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="NotImplementedException"></exception>
    public async Task DeleteUser(Guid id) {
        throw new NotImplementedException();
    }
}