using System.ComponentModel.DataAnnotations;

namespace BackendDeveloperTest.Api.DTOs;

public class UpdateProductRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    public string? Description { get; set; }

    [Required]
    public string Category { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public List<string> Images { get; set; } = [];
}