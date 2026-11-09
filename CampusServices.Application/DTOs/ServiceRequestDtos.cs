using System.ComponentModel.DataAnnotations;
using CampusServices.Domain.Enums;

namespace CampusServices.Application.DTOs;

public record CreateRequestDto(
    [Required][StringLength(100, MinimumLength = 5)] string Title,
    [Required][StringLength(1000, MinimumLength = 10)] string Description,
    [Required] RequestCategory Category
);

public record UpdateStatusDto(
    [Required] RequestStatus Status,
    string? StaffId
);

public record RequestResponseDto(
    Guid Id,
    string Title,
    string Description,
    string Status,
    string Category,
    DateTime CreatedAt,
    string StudentId,
    string? StaffId
);

public record AuthResponseDto(
    string Token,
    string Email,
    string Role,
    DateTime Expiration
);

public record LoginDto(
    [Required][EmailAddress] string Email,
    [Required] string Password
);