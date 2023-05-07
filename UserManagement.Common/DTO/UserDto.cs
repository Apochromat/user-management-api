using System.Text.Json.Serialization;

namespace UserManagement.Common.DTO; 

/// <summary>
/// Data transfer object for user
/// </summary>
public class UserDto {
    /// <summary>
    /// Identifier
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    /// <summary>
    /// User nickname/login
    /// </summary>
    [JsonPropertyName("login")]
    public string? UserName { get; set; }
    /// <summary>
    /// Creation date
    /// </summary>
    [JsonPropertyName("created_date")]
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// User group
    /// </summary>
    [JsonPropertyName("user_group")]
    public required UserGroupDto Group { get; set; }
    /// <summary>
    /// User state
    /// </summary>
    [JsonPropertyName("user_state")]
    public required UserStateDto State { get; set; }
}