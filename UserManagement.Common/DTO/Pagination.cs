using System.Text.Json.Serialization;

namespace UserManagement.Common.DTO; 

/// <summary>
/// Generic pagination class
/// </summary>
/// <typeparam name="T"></typeparam>
public class Pagination<T> {
    /// <summary>
    /// List of items (content)
    /// </summary>
    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = new List<T>();
    /// <summary>
    /// Current page
    /// </summary>
    [JsonPropertyName("current")]
    public int CurrentPage { get; set; }
    /// <summary>
    /// Page size
    /// </summary>
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }
    /// <summary>
    /// Page total count
    /// </summary>
    [JsonPropertyName("page_amount")]
    public int PageTotalCount { get; set; }
}