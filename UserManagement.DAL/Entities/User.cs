using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace UserManagement.DAL.Entities; 

/// <summary>
/// User entity
/// </summary>
public class User: IdentityUser<Guid> {
    /// <summary>
    /// Created date
    /// </summary>
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// User group
    /// </summary>
    public required UserGroup UserGroup { get; set; }
    /// <summary>
    /// User state
    /// </summary>
    public required UserState UserState { get; set; }
}