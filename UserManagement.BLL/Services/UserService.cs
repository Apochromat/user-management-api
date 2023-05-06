using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Common.DTO;
using UserManagement.Common.Enumerations;
using UserManagement.Common.Exceptions;
using UserManagement.Common.Interfaces;
using UserManagement.DAL;
using UserManagement.DAL.Entities;

namespace UserManagement.BLL.Services; 

/// <summary>
/// User management service
/// </summary>
public class UserService : IUserService {
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userManager"></param>
    public UserService(ApplicationDbContext context, UserManager<User> userManager) {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="registerDto"></param>
    public async Task Register(RegisterDto registerDto) {
        var userGroup = await _context.UserGroups.FirstOrDefaultAsync(g => g.Code == GroupCode.User);
        if (userGroup == null) {
            throw new InvalidOperationException("User group not found");
        }
        var activeState = await _context.UserStates.FirstOrDefaultAsync(s => s.Code == StateCode.Active);
        if (activeState == null) {
            throw new InvalidOperationException("Active state not found");
        }
        
        // TODO: check that only one user can be in admin group
        
        var user = new User() {
            Id = Guid.NewGuid(),
            Email = registerDto.Email,
            UserName = registerDto.UserName,
            UserGroup = userGroup,
            UserState = activeState
        };
        
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded) {
            throw new ConflictException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Gets a single user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UserDto> GetUser(Guid id) {
        var user = await _context.Users
            .Include(u=> u.UserState)
            .Include(u=> u.UserGroup)
            .FirstOrDefaultAsync(u=>u.Id == id);
        
        if (user == null) {
            throw new NotFoundException("User not found");
        }
        
        return new UserDto() {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Group = new UserGroupDto() {
                Id = user.UserGroup.Id,
                Code = user.UserGroup.Code,
                Description = user.UserGroup.Description
            },
            State = new UserStateDto() {
                Id = user.UserState.Id,
                Code = user.UserState.Code,
                Description = user.UserState.Description
            },
            CreatedAt = user.CreatedDate
        };
    }

    /// <summary>
    /// Gets a couple users with pagination
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async Task<Pagination<UserDto>> GetAllUsers(int page, int pageSize = 10) {
        if (page < 1) {
            throw new BadRequestException("Invalid page number");
        }
        var usersAmount = _context.Users.Count();
        var pagesAmount = Math.Ceiling((decimal)usersAmount / pageSize);
        if (pagesAmount < page) {
            throw new NotFoundException("Page not found");
        }
        
        var users = await _context.Users
            .Include(u=> u.UserState)
            .Include(u=> u.UserGroup)
            .OrderBy(u=>u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        var paginationUserDto = new Pagination<UserDto>() {
            Items = users.Select(u => new UserDto() {
                Id = u.Id,
                Email = u.Email,
                UserName = u.UserName,
                Group = new UserGroupDto() {
                    Id = u.UserGroup.Id,
                    Code = u.UserGroup.Code,
                    Description = u.UserGroup.Description
                },
                State = new UserStateDto() {
                    Id = u.UserState.Id,
                    Code = u.UserState.Code,
                    Description = u.UserState.Description
                },
                CreatedAt = u.CreatedDate
            }).ToList(),
            CurrentPage = page,
            PageSize = pageSize,
            PageTotalCount = (int)pagesAmount,
        };
        
        return paginationUserDto;
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="id"></param>
    public async Task DeleteUser(Guid id) {
        var user = await _context.Users
            .Include(u=> u.UserState)
            .Include(u=> u.UserGroup)
            .FirstOrDefaultAsync(u=>u.Id == id);
        
        if (user == null) {
            throw new NotFoundException("User not found");
        }

        var blockedState = await _context.UserStates.FirstOrDefaultAsync(s => s.Code == StateCode.Blocked);
        if (blockedState == null) {
            throw new InvalidOperationException("Blocked state not found");
        }
        
        if (user.UserState.Code == StateCode.Blocked) {
            throw new ConflictException("User already blocked");
        }
        user.UserState = blockedState;
        await _context.SaveChangesAsync();
    }
}