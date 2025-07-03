using Microsoft.EntityFrameworkCore;
using SMJRegisterAPI.Database.Contexts;
using SMJRegisterAPI.Features.Common;

namespace SMJRegisterAPI.Features.Camper.Repository;

public class CamperRepository(ApplicationDbContext context) :  GenericRepository<Entities.Camper>(context), ICamperRepository
{
   
    public override async Task<List<Entities.Camper>> GetAllAsync()
        =>await context.Campers
            .AsSplitQuery()
            .Include(c => c.Church)
            .ToListAsync();
    
}