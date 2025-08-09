using System.Security.Claims;
using SMJRegisterAPI.Entities.Enums;

namespace SMJRegisterAPI.Services.Tenant;

public class TenantServices(IHttpContextAccessor httpContextAccessor) : ITenantServices
{
    public Conference Conference
    {
        get
        {
            var claimValue = httpContextAccessor.HttpContext?.User?.FindFirstValue("conference");
            if (Enum.TryParse<Conference>(claimValue, out var conference))
                return conference;
            
            return Conference.General;
        }
    }
}