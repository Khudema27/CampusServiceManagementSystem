using System;

namespace CampusServices.Domain.Entities;

public enum ServiceCategory { ITSupport = 0, HostelMaintenance = 1, ElectricalRepair = 2, Sanitation = 3 }
public enum RequestStatus { Pending = 0, InProgress = 1, Completed = 2 }

public class ServiceRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ServiceCategory Category { get; private set; }
    public RequestStatus Status { get; private set; } = RequestStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? StudentId { get; private set; }
    public string? StaffId { get; private set; }

    public ServiceRequest() { }

    public ServiceRequest(string title, string description, ServiceCategory category, string studentId)
    {
        Title = title;
        Description = description;
        Category = category;
        StudentId = studentId;
        CreatedAt = DateTime.UtcNow;
        Status = RequestStatus.Pending;
    }

    public void UpdateStatus(RequestStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignToStaff(string staffId)
    {
        StaffId = staffId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string title, string description, ServiceCategory category)
    {
        Title = title;
        Description = description;
        Category = category;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(Enums.RequestStatus status)
    {
        throw new NotImplementedException();
    }
}