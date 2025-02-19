using Microsoft.EntityFrameworkCore;
using publisherData;
using static System.Reflection.Metadata.BlobBuilder;

namespace DumplingApi.Endpoints;

public class StaffEndPoints
{
    public static void Map(WebApplication app)
    {

        // Get All Staff members
        app.MapGet("api/staff", async (PubContext dbContext) =>
        {
            return await dbContext.Staff.ToListAsync();
        });

        // get single staff by ID
        app.MapGet("api/staff/{id}", async (int id, PubContext dbContext) =>
        {
            var staffMember = await dbContext.Staff.FindAsync(id);
            if (staffMember == null) return Results.NotFound();

            return Results.Ok(staffMember);
        });

        // Post create new staff member
        app.MapPost("api/staff", async (Staff newStaff, PubContext dbContext) =>
        {
            dbContext.Staff.Add(newStaff);
            await dbContext.SaveChangesAsync();
            return Results.Created($"api/staff/{newStaff.Id}", newStaff);
        });

        // Update staff member
        app.MapPut("/api/staff/update/{id}", async (int id, Staff staffMember, PubContext dbContext) =>
        {
            var newStaffMember = await dbContext.Staff.FindAsync(id);
            if (newStaffMember == null) return Results.NotFound();

            newStaffMember.Name = staffMember.Name;
            newStaffMember.Telephone = staffMember.Telephone;
            newStaffMember.Role = staffMember.Role;

            await dbContext.SaveChangesAsync();
            return Results.Ok(newStaffMember);
        });

        // Update staff member
        app.MapDelete("/api/staff/delete/{id}", async (int id, PubContext dbContext) =>
        {
            var staffMember = await dbContext.Staff.FindAsync(id);
            if (staffMember == null) return Results.NotFound();

            dbContext.Staff.Remove(staffMember);
            await dbContext.SaveChangesAsync();
            return Results.Ok();
        });
    }
}