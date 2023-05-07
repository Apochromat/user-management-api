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
            if (Context.UserGroups.Count(g => g.Code == groupCode) == 0) {
                await Context.UserGroups.AddAsync(new UserGroup() {
                    Id = Guid.NewGuid(),
                    Code = groupCode,
                    Description = groupCode.ToString()
                });
            }
        }
        foreach (var stateCode in Enum.GetValues(typeof(StateCode)).Cast<StateCode>()) {
            if (Context.UserStates.Count(g => g.Code == stateCode) == 0) {
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
        var groupAmount = Context.UserGroups.Count();

        // Assert
        Assert.Equal(2, groupAmount);
    }

    [Fact]
    public async Task Test_DefaultUserStatesExist_Pass() {
        // Arrange
        await InitDatabase();

        // Act
        var groupAmount = Context.UserStates.Count();

        // Assert
        Assert.Equal(2, groupAmount);
    }
    
    [Fact]
    public async Task Test_UserRegistration_UserCreatedInDb() {
        // Arrange
        await InitDatabase();

        // Act
        await UserService.Register(new RegisterDto() {
            UserName = "username",
            Password = "P@ssw0rd",
        });
        var userAmount = Context.Users.Count();

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

    public UnitTests(IUserService userService, ApplicationDbContext context) : base(userService, context) {
    }
}