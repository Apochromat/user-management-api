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
    private readonly SignInManager<User> _signInManager;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    public UserService(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager) {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
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
        var adminGroup = await _context.UserGroups.FirstOrDefaultAsync(g => g.Code == GroupCode.Admin);
        if (adminGroup == null) {
            throw new InvalidOperationException("User group not found");
        }
        
        if (registerDto.GroupCode == GroupCode.Admin && 
            await _context.Users.AnyAsync(u => u.UserGroup.Code == GroupCode.Admin && 
                                               u.UserState.Code == StateCode.Active)) {
            throw new ConflictException("Admin already exists");
        }
        
        if (await _context.Users.AnyAsync(u => u.UserName == registerDto.UserName)) {
            throw new ConflictException("User with login already exists");
        }
        
        var activeState = await _context.UserStates.FirstOrDefaultAsync(s => s.Code == StateCode.Active);
        if (activeState == null) {
            throw new InvalidOperationException("Active state not found");
        }

        var user = new User() {
            Id = Guid.NewGuid(),
            UserName = registerDto.UserName,
            UserGroup = registerDto.GroupCode == GroupCode.Admin ? adminGroup : userGroup,
            UserState = activeState
        };
        
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded) {
            throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        await _context.SaveChangesAsync();
        
        // If you really need delay
        // Thread.Sleep(5000);
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
        if (pageSize < 1) {
            throw new BadRequestException("Invalid page size");
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

    /// <summary>
    /// Login a user
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<Guid?> Login(string username, string password) {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) {
            return null;
        }
        if (user.UserState.Code == StateCode.Blocked) {
            return null;
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded) {
            return null;
        }
        return user.Id;
    }
}