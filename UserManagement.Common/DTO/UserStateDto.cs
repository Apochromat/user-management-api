using System.ComponentModel;
using UserManagement.Common.Enumerations;

namespace UserManagement.Common.DTO;

/// <summary>
/// Data transfer object for user state
/// </summary>
public class UserStateDto {
    /// <summary>
    /// Identifier
    /// </summary>
    [DisplayName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// State code
    /// </summary>
    [DisplayName("code")]
    public StateCode Code { get; set; }

    /// <summary>
    /// State description
    /// </summary>
    [DisplayName("description")]
    public String? Description { get; set; }
}