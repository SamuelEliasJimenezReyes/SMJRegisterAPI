using AutoMapper;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using SMJRegisterAPI.Features.Camper.Command.Create;
using SMJRegisterAPI.Features.Camper.Dtos;
using SMJRegisterAPI.Features.Camper.Queries.GetAll;
using SMJRegisterAPI.Features.Camper.Queries.GetById;

namespace SMJRegisterAPI.Features.Camper.Endpoints;

public class CamperEndpoints() : CarterModule("/camper")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetAllCampers);
        app.MapGet("/{id:int}", GetCamperById);
        app.MapPost("/", CreateCamper);
    }

    
    private async Task<Results<Ok<IList<CamperDTO>>, NotFound>> GetAllCampers(ISender sender)
    {
        var result = await sender.Send(new GetAllCampersQuery());
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);

    }

    private async Task<Results<Ok<CamperDTO>, NotFound>> GetCamperById(ISender sender, int id)
    {
        var result = await sender.Send(new GetCamperByIdQuery
        {
            ID = id
        });

        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }

    private async Task<Created> CreateCamper(ISender sender
        , CreateCamperCommand command)
    {
        var result = await sender.Send(command);
        return TypedResults.Created();
    }
}