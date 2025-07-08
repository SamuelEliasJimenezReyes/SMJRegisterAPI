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

    public override async Task<Entities.Camper> GetByIdAsync(int id)
    {
        var entiy = await context.Campers.Include(c=>c.Church)
            .Where(x=>x.ID == id)
            .FirstOrDefaultAsync();
        return entiy;
    }

    public async Task<List<Entities.Camper>> GetAllByChurchIDAsync(int churchID)
    => await context.Campers
        .Where(x => x.ChurchId == churchID)
        .Include(x=>x.Church)
        .ToListAsync();

}