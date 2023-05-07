using System.Text.Json.Serialization;
using UserManagement.Common.Enumerations;

namespace UserManagement.Common.DTO;

/// <summary>
/// Data transfer object for user state
/// </summary>
public class UserStateDto {
    /// <summary>
    /// Identifier
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// State code
    /// </summary>
    [JsonPropertyName("code")]
    public StateCode Code { get; set; }

    /// <summary>
    /// State description
    /// </summary>
    [JsonPropertyName("description")]
    public String? Description { get; set; }
}