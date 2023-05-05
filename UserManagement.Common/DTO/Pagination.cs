namespace UserManagement.Common.DTO; 

/// <summary>
/// Generic pagination class
/// </summary>
/// <typeparam name="T"></typeparam>
public class Pagination<T> {
    /// <summary>
    /// List of items (content)
    /// </summary>
    public List<T> Items { get; set; } = new List<T>();
    /// <summary>
    /// Current page
    /// </summary>
    public int CurrentPage { get; set; }
    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// Page total count
    /// </summary>
    public int PageTotalCount { get; set; }
}