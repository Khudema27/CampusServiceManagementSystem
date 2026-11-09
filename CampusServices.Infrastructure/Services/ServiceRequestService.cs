using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampusServices.Application.DTOs;
using CampusServices.Application.Interfaces;
using CampusServices.Domain.Entities;
using CampusServices.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CampusServices.Infrastructure.Services;

public class ServiceRequestService : IServiceRequestService
{
    private readonly ApplicationDbContext _context;

    public ServiceRequestService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RequestResponseDto> CreateRequestAsync(CreateRequestDto dto, string studentId)
    {
        var categoryEnum = (ServiceCategory)dto.Category;
        var entity = new ServiceRequest(dto.Title, dto.Description, categoryEnum, studentId ?? string.Empty);

        _context.ServiceRequests.Add(entity);
        await _context.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<IEnumerable<RequestResponseDto>> GetAllRequestsAsync(string? userId, string role)
    {
        var query = _context.ServiceRequests.AsNoTracking();

        if (role == "Student" && !string.IsNullOrEmpty(userId))
        {
            query = query.Where(r => r.StudentId == userId);
        }

        var list = await query.ToListAsync();
        return list.Select(MapToDto);
    }

    public async Task<RequestResponseDto?> GetByIdAsync(Guid id)
    {
        var r = await _context.ServiceRequests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return r == null ? null : MapToDto(r);
    }

    public async Task<RequestResponseDto?> AssignStaffAsync(Guid id, string staffId)
    {
        var entity = await _context.ServiceRequests.FindAsync(id);
        if (entity == null) return null;

        entity.AssignToStaff(staffId);
        await _context.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<RequestResponseDto?> UpdateStatusAsync(Guid id, CampusServices.Application.Interfaces.UpdateStatusDto dto)
    {
        var entity = await _context.ServiceRequests.FindAsync(id);
        if (entity == null) return null;

        entity.UpdateStatus(dto.Status);

        if (!string.IsNullOrEmpty(dto.StaffId))
        {
            entity.AssignToStaff(dto.StaffId);
        }

        await _context.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<RequestResponseDto?> UpdateRequestAsync(Guid id, CreateRequestDto dto)
    {
        var entity = await _context.ServiceRequests.FindAsync(id);
        if (entity == null) return null;

        var categoryEnum = (ServiceCategory)dto.Category;
        entity.UpdateDetails(dto.Title, dto.Description, categoryEnum);

        await _context.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<bool> DeleteRequestAsync(Guid id)
    {
        var entity = await _context.ServiceRequests.FindAsync(id);
        if (entity == null) return false;

        _context.ServiceRequests.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    private static RequestResponseDto MapToDto(ServiceRequest r) => new(
        r.Id, r.Title, r.Description, r.Status.ToString(),
        r.Category.ToString(), r.CreatedAt, r.StudentId ?? string.Empty, r.StaffId
    );
}