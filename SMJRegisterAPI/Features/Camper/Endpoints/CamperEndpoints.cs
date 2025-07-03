using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using SMJRegisterAPI.Features.Camper.Dtos;
using SMJRegisterAPI.Features.Camper.Queries.GetAll;

namespace SMJRegisterAPI.Features.Camper.Endpoints;

public class CamperEndpoints() : CarterModule("/camper")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetAllCampers);
    }

    
    private async Task<Results<Ok<IList<CamperDTO>>, NotFound>> GetAllCampers(ISender sender)
    {
        var result = await sender.Send(new GetAllCampersQuery());
        
        if (result == null || !result.Any())
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(result);
    }
}