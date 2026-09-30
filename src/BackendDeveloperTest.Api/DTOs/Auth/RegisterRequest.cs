using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendDeveloperTest.Api.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("password_confirmation")]
    public string PasswordConfirmation { get; set; } = string.Empty;
}