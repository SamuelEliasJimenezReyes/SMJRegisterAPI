using SMJRegisterAPI.Database.Contexts;
using SMJRegisterAPI.Features.Common;
using SMJRegisterAPI.Services.CodeGenerator;

namespace SMJRegisterAPI.Features.GrantedCode.Repository;

public class GrantedCodeRepository (ApplicationDbContext context, IGenerateCodeService codeGeneratorService) : GenericRepository<Entities.GrantedCode>(context) , IGrantedCodeRepository
{
    public async  Task<Entities.GrantedCode> AddWithCodeAsync(Entities.GrantedCode entity, int Amount)
    {
        var codeGenerated = codeGeneratorService.GenerateAlphanumericCode();
        var codeExits =  context.GrantedCodes.Any(grantedCode => grantedCode.Code == codeGenerated); 
        do
        {
            codeGenerated = codeGeneratorService.GenerateAlphanumericCode();
        } while (codeExits);
        entity.Code = codeGenerated;
        entity.GrantAmount = Amount;
        entity.IsUsed = false;
        
        await context.GrantedCodes.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}