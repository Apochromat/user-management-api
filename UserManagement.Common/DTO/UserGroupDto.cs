using System.Text.Json.Serialization;
using UserManagement.Common.Enumerations;

namespace UserManagement.Common.DTO;

/// <summary>
/// Data transfer object for user group
/// </summary>
public class UserGroupDto {
    /// <summary>
    /// Identifier
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Group code
    /// </summary>
    [JsonPropertyName("code")]
    public GroupCode Code { get; set; }

    /// <summary>
    /// Group description
    /// </summary>
    [JsonPropertyName("description")]
    public String? Description { get; set; }
}