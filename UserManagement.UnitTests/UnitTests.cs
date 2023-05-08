using Microsoft.EntityFrameworkCore;
using UserManagement.Common.DTO;
using UserManagement.Common.Enumerations;
using UserManagement.Common.Exceptions;
using UserManagement.Common.Interfaces;
using UserManagement.DAL;
using UserManagement.DAL.Entities;

namespace UserManagement.UnitTests;

public abstract class TestsBase : IDisposable {
    protected TestsBase(IUserService userService, ApplicationDbContext context) {
        UserService = userService;
        Context = context;
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }
    
    protected async Task InitDatabase() {
        foreach (var groupCode in Enum.GetValues(typeof(GroupCode)).Cast<GroupCode>()) {
            if (await Context.UserGroups.CountAsync(g => g.Code == groupCode) == 0) {
                await Context.UserGroups.AddAsync(new UserGroup() {
                    Id = Guid.NewGuid(),
                    Code = groupCode,
                    Description = groupCode.ToString()
                });
            }
        }
        foreach (var stateCode in Enum.GetValues(typeof(StateCode)).Cast<StateCode>()) {
            if (await Context.UserStates.CountAsync(g => g.Code == stateCode) == 0) {
                await Context.UserStates.AddAsync(new UserState() {
                    Id = Guid.NewGuid(),
                    Code = stateCode,
                    Description = stateCode.ToString()
                });
            }
        }
        await Context.SaveChangesAsync();
    }

    public void Dispose() {
        Context.Database.EnsureDeleted();
    }

    protected ApplicationDbContext Context { get; private set; }
    protected IUserService UserService { get; private set; }
}

public class UnitTests : TestsBase {
    [Fact]
    public async Task Test_DefaultUserGroupsExist_Pass() {
        // Arrange
        await InitDatabase();

        // Act
        var groupAmount = await Context.UserGroups.CountAsync();

        // Assert
        Assert.Equal(2, groupAmount);
    }

    [Fact]
    public async Task Test_DefaultUserStatesExist_Pass() {
        // Arrange
        await InitDatabase();

        // Act
        var groupAmount = await Context.UserStates.CountAsync();

        // Assert
        Assert.Equal(2, groupAmount);
    }
    
    [Fact]
    public async Task Test_UserRegistrationWithCorrectCredentials_UserCreatedInDb() {
        // Arrange
        await InitDatabase();

        // Act
        await UserService.Register(new RegisterDto() {
            UserName = "username",
            Password = "P@ssw0rd",
            GroupCode = GroupCode.User
        });
        var userAmount = await Context.Users.CountAsync();

        // Assert
        Assert.Equal(1, userAmount);
    }
    
    [Fact]
    public async Task Test_GetUsersAfterUserRegistration_ReturnUsersList() {
        // Arrange
        await InitDatabase();
        await UserService.Register(new RegisterDto() {
            UserName = "username",
            Password = "P@ssw0rd",
            GroupCode = GroupCode.User
        });

        // Act
        var userAmount = (await UserService.GetAllUsers(1)).Items.Count();

        // Assert
        Assert.Equal(1, userAmount);
    }
    
    [Fact]
    public async Task Test_GetConcreteUserWithRightId_DoesNotThrowException() {
        // Arrange
        await InitDatabase();
        await UserService.Register(new RegisterDto() {
            UserName = "username",
            Password = "P@ssw0rd",
            GroupCode = GroupCode.User
        });
        var id = (await UserService.GetAllUsers(1)).Items.First().Id;

        // Act
        var exception = await Record.ExceptionAsync(async () => await UserService.GetUser(id));

        // Assert
        Assert.Null(exception);
    }
    
    [Fact]
    public async Task Test_GetConcreteUserWithWrongId_ThrowException() {
        // Arrange
        await InitDatabase();

        // Act

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await UserService.GetUser(Guid.NewGuid()));
    }
    
    [Fact]
    public async Task Test_UserRegistrationWithIncorrectPasswordLength_ThrowException() {
        // Arrange
        await InitDatabase();

        // Act

        // Assert
        await Assert.ThrowsAsync<BadRequestException>(async () => await UserService.Register(new RegisterDto() {
            UserName = "username",
            Password = "Q1@rt",
            GroupCode = GroupCode.User
        }));
    }
    
    [Fact]
    public async Task Test_UserRegistrationWithoutDigitInPassword_ThrowException() {
        // Arrange
        await InitDatabase();

        // Act

        // Assert
        await Assert.ThrowsAsync<BadRequestException>(async () => await UserService.Register(new RegisterDto() {
            UserName = "username",
            Password = "Qw@rtyqwerty",
            GroupCode = GroupCode.User
        }));
    }
    
    [Fact]
    public async Task Test_UserRegistrationWithoutUppercaseLetterInPassword_ThrowException() {
        // Arrange
        await InitDatabase();

        // Act

        // Assert
        await Assert.ThrowsAsync<BadRequestException>(async () => await UserService.Register(new RegisterDto() {
            UserName = "username",
            Password = "1w@rtyqwerty",
            GroupCode = GroupCode.User
        }));
    }
    
    [Fact]
    public async Task Test_UserRegistrationWithoutSymbolInPassword_ThrowException() {
        // Arrange
        await InitDatabase();

        // Act

        // Assert
        await Assert.ThrowsAsync<BadRequestException>(async () => await UserService.Register(new RegisterDto() {
            UserName = "username",
            Password = "Qw0rtyqwerty",
            GroupCode = GroupCode.User
        }));
    }
    
    [Fact]
    public async Task Test_UserRegistrationWithExistingUsername_ThrowException() {
        // Arrange
        await InitDatabase();
        await UserService.Register(new RegisterDto() {
            UserName = "username",
            Password = "P@ssw0rd",
            GroupCode = GroupCode.User
        });

        // Act

        // Assert
        await Assert.ThrowsAsync<ConflictException>(async () => await UserService.Register(new RegisterDto() {
            UserName = "username",
            Password = "P@ssw0rd",
            GroupCode = GroupCode.User
        }));
    }
    
    [Fact]
    public async Task Test_UserRegistrationWithExistingAdminUser_ThrowException() {
        // Arrange
        await InitDatabase();
        await UserService.Register(new RegisterDto() {
            UserName = "username",
            Password = "P@ssw0rd",
            GroupCode = GroupCode.Admin
        });

        // Act

        // Assert
        await Assert.ThrowsAsync<ConflictException>(async () => await UserService.Register(new RegisterDto() {
            UserName = "adminName",
            Password = "P@ssw0rd",
            GroupCode = GroupCode.Admin
        }));
    }

    public UnitTests(IUserService userService, ApplicationDbContext context) : base(userService, context) {
    }
}