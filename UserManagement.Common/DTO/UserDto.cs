using System.ComponentModel;

namespace UserManagement.Common.DTO; 

/// <summary>
/// Data transfer object for user
/// </summary>
public class UserDto {
    /// <summary>
    /// Identifier
    /// </summary>
    [DisplayName("id")]
    public Guid Id { get; set; }
    /// <summary>
    /// Email
    /// </summary>
    [DisplayName("email")]
    public string? Email { get; set; }
    /// <summary>
    /// User nickname/login
    /// </summary>
    [DisplayName("login")]
    public string? UserName { get; set; }
    /// <summary>
    /// Creation date
    /// </summary>
    [DisplayName("created_date")]
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// User group
    /// </summary>
    [DisplayName("user_group")]
    public required UserGroupDto Group { get; set; }
    /// <summary>
    /// User state
    /// </summary>
    [DisplayName("user_state")]
    public required UserStateDto State { get; set; }
}