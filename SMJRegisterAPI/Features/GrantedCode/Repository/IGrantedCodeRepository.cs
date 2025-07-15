using SMJRegisterAPI.Features.Common;

namespace SMJRegisterAPI.Features.GrantedCode.Repository;

public interface IGrantedCodeRepository : IGenericRepository<Entities.GrantedCode>
{
    public Task<Entities.GrantedCode> AddAsync(Entities.GrantedCode entity, int Amount);

}