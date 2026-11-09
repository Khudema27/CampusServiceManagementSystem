using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CampusServices.Application.DTOs;
using CampusServices.Domain.Entities;

namespace CampusServices.Application.Interfaces;

public record UpdateStatusDto(RequestStatus Status, string? StaffId = null);

public interface IServiceRequestService
{
    Task<RequestResponseDto> CreateRequestAsync(CreateRequestDto dto, string studentId);
    Task<IEnumerable<RequestResponseDto>> GetAllRequestsAsync(string? userId, string role);
    Task<RequestResponseDto?> GetByIdAsync(Guid id);
    Task<RequestResponseDto?> AssignStaffAsync(Guid id, string staffId);
    Task<RequestResponseDto?> UpdateStatusAsync(Guid id, UpdateStatusDto dto);
    Task<RequestResponseDto?> UpdateRequestAsync(Guid id, CreateRequestDto dto);
    Task<bool> DeleteRequestAsync(Guid id);
}