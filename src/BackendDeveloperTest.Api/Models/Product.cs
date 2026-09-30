using System.Text.Json.Serialization;

namespace BackendDeveloperTest.Api.Models;

public class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string Description { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public List<string> Images { get; set; } = [];

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("created_by")]
    public string CreatedBy { get; set; } = string.Empty;

    [JsonPropertyName("created_by_id")]
    public string CreatedById { get; set; } = string.Empty;

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("updated_by")]
    public string? UpdatedBy { get; set; }

    [JsonPropertyName("updated_by_id")]
    public string? UpdatedById { get; set; }
}