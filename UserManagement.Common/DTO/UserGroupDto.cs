using System.ComponentModel;
using UserManagement.Common.Enumerations;

namespace UserManagement.Common.DTO;

/// <summary>
/// Data transfer object for user group
/// </summary>
public class UserGroupDto {
    /// <summary>
    /// Identifier
    /// </summary>
    [DisplayName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Group code
    /// </summary>
    [DisplayName("code")]
    public GroupCode Code { get; set; }

    /// <summary>
    /// Group description
    /// </summary>
    [DisplayName("description")]
    public String? Description { get; set; }
}