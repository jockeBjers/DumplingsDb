using Microsoft.EntityFrameworkCore;
using publisherData;

namespace DumplingApi.Services;

public interface IStaffService
{
    Task<List<Staff>> GetAllStaffAsync();
    Task<Staff?> GetStaffByIdAsync(int id);
    Task<Staff> CreateStaffAsync(Staff newStaff);
    Task<Staff?> UpdateStaffAsync(int id, Staff updatedStaff);
    Task<bool> DeleteStaffAsync(int id);
}

public class StaffService : IStaffService
{
    private readonly PubContext dbContext;
    public StaffService(PubContext dbContext)
    {
        this.dbContext = dbContext;
    }

    // Get all staff members
    public async Task<List<Staff>> GetAllStaffAsync()
    {
        return await dbContext.Staff.ToListAsync();
    }

    // Get staff member by ID
    public async Task<Staff?> GetStaffByIdAsync(int id)
    {

        return await dbContext.Staff.FindAsync(id);
    }

    // Create new staff member
    public async Task<Staff> CreateStaffAsync(Staff newStaff)
    {
        dbContext.Staff.Add(newStaff);
        await dbContext.SaveChangesAsync();
        return newStaff;
    }

    // Update staff member
    public async Task<Staff?> UpdateStaffAsync(int id, Staff updatedStaff)
    {
        var staffMember = await dbContext.Staff.FindAsync(id);
        if (staffMember == null) return null;
        staffMember.Name = updatedStaff.Name;
        staffMember.Telephone = updatedStaff.Telephone;
        staffMember.Role = updatedStaff.Role;
        await dbContext.SaveChangesAsync();
        return staffMember;
    }

    // Delete staff member
    public async Task<bool> DeleteStaffAsync(int id)
    {
        var staffMember = await dbContext.Staff.FindAsync(id);
        if (staffMember == null) return false;
        dbContext.Staff.Remove(staffMember);
        await dbContext.SaveChangesAsync();
        return true;
    }
}

