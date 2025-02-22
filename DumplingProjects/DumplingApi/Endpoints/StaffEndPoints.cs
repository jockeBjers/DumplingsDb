using DumplingApi.Services;
using Microsoft.EntityFrameworkCore;
using publisherData;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Reflection.Metadata.BlobBuilder;

namespace DumplingApi.Endpoints;

public class StaffEndPoints
{
    public static void Map(WebApplication app)
    {
        // Get All Staff members
        app.MapGet("api/staff", async (IStaffService service) =>
        {
            return await service.GetAllStaffAsync();
        });

        // get single staff by ID
        app.MapGet("api/staff/{id}", async (int id, IStaffService service) =>
        {
            var staffMember = await service.GetStaffByIdAsync(id);
            if (staffMember == null) return Results.NotFound();
            return Results.Ok(staffMember);
        });

        // Post create new staff member
        app.MapPost("api/staff", async (Staff newStaff, IStaffService service) =>
        {
            var createdStaff = await service.CreateStaffAsync(newStaff);

            return Results.Created($"api/staff/{createdStaff.Id}", createdStaff);
        });

        // Update staff member
        app.MapPut("/api/staff/update/{id}", async (int id, Staff staffMember, IStaffService service) =>
        {
            var newStaffMember = await service.UpdateStaffAsync(id, staffMember);
            if (newStaffMember == null) return Results.NotFound();
            return Results.Ok(newStaffMember);
        });

        // Update staff member
        app.MapDelete("/api/staff/delete/{id}", async (int id, IStaffService service) =>
        {
            var deleted = await service.DeleteStaffAsync(id);
            if (!deleted) return Results.NotFound();
            return Results.Ok();
        });
    }
}