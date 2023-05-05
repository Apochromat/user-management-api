using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Common.DTO; 

/// <summary>
/// Data transfer object for user registration
/// </summary>
public class RegisterDto {
    /// <summary>
    /// User email
    /// </summary>
    [Required]
    [EmailAddress]
    [DisplayName("email")]
    public required string Email { get; set; }
    
    /// <summary>
    /// User nickname/login
    /// </summary>
    [Required]
    [DisplayName("login")]
    public required string UserName { get; set; }
    
    /// <summary>
    /// User password
    /// </summary>
    [Required]
    [StringLength(64, MinimumLength = 8)]
    [DefaultValue("P@ssw0rd")]
    [DisplayName("password")]
    public required string Password { get; set; }
}