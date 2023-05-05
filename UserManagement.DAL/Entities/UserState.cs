using System.ComponentModel.DataAnnotations;
using UserManagement.Common.Enumerations;

namespace UserManagement.DAL.Entities; 

/// <summary>
/// User state entity
/// </summary>
public class UserState {
    /// <summary>
    /// Identifier
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    /// <summary>
    /// State code
    /// </summary>
    [EnumDataType(typeof(StateCode))]
    public StateCode Code { get; set; }
    /// <summary>
    /// State description
    /// </summary>
    [MaxLength(256)]
    [DataType(DataType.Text)]
    public String? Description { get; set; }
    /// <summary>
    /// Users with the state
    /// </summary>
    public IEnumerable<User> Users { get; set; } = new List<User>();
}