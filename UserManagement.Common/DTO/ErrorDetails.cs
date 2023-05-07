using System.Text.Json;
using System.Text.Json.Serialization;

namespace UserManagement.Common.DTO; 

/// <summary>
/// Error details model
/// </summary>
public class ErrorDetails {
    /// <summary>
    /// Http status code
    /// </summary>
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }
    /// <summary>
    /// Message to user
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    /// <summary>
    /// Trace id
    /// </summary>
    [JsonPropertyName("trace_id")]
    public string? TraceId { get; set; }
    
    /// <summary>
    /// Convert to string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}