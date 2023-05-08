using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UserManagement.Common.Enumerations;

namespace UserManagement.Common.DTO; 

/// <summary>
/// Data transfer object for user registration
/// </summary>
public class RegisterDto {
    /// <summary>
    /// User nickname/login
    /// </summary>
    [Required]
    [JsonPropertyName("login")]
    public required string UserName { get; set; }
    
    /// <summary>
    /// User password
    /// </summary>
    [Required]
    [MinLength(6)]
    [DefaultValue("P@ssw0rd")]
    [JsonPropertyName("password")]
    public required string Password { get; set; }
    
    /// <summary>
    /// Group code
    /// </summary>
    [Required]
    [JsonPropertyName("group_code")]
    public GroupCode GroupCode { get; set; }
}