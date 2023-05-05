using System.ComponentModel.DataAnnotations;
using UserManagement.Common.Enumerations;

namespace UserManagement.DAL.Entities; 

/// <summary>
/// User group entity
/// </summary>
public class UserGroup {
    /// <summary>
    /// Identifier
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    /// <summary>
    /// Group code
    /// </summary>
    [EnumDataType(typeof(GroupCode))]
    public GroupCode Code { get; set; }
    /// <summary>
    /// Group description
    /// </summary>
    [MaxLength(256)]
    [DataType(DataType.Text)]
    public String? Description { get; set; }
    /// <summary>
    /// Users within the group
    /// </summary>
    public IEnumerable<User> Users { get; set; } = new List<User>();
}