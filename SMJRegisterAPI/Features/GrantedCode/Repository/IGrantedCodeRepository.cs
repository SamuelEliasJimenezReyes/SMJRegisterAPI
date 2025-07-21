using SMJRegisterAPI.Features.Common;

namespace SMJRegisterAPI.Features.GrantedCode.Repository;

public interface IGrantedCodeRepository : IGenericRepository<Entities.GrantedCode>
{
    public Task<Entities.GrantedCode> AddWithCodeAsync(Entities.GrantedCode entity, int Amount);
}