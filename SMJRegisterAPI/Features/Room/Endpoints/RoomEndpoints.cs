using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using SMJRegisterAPI.Features.Church.Queries.GetAll;
using SMJRegisterAPI.Features.Room.Command;
using SMJRegisterAPI.Features.Room.Dtos;
using SMJRegisterAPI.Features.Room.Queries.GetAll;
using SMJRegisterAPI.Features.Room.Queries.GetById;

namespace SMJRegisterAPI.Features.Room.Endpoints;

public class RoomEndpoints() : CarterModule("/room")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetAll);
        app.MapGet("/{id:int}", GetById);
        app.MapPost("/", Create);
    }
    
    private async Task<Results<Ok<IList<RoomSimpleDto>>, NotFound>> GetAll(ISender sender)
    {
        var result = await sender.Send(new GetAllRoomsQuery());
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }
    private async Task<Results<Ok<RoomDto>, NotFound>> GetById(ISender sender, int id)
    {
        var result = await sender.Send(new GetRoomByIdQuery()
        {
            Id = id
        });
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }
    
    private async Task<Created> Create(ISender sender
        , CreateRoomDto dto)
    {
        var command = new CreateRoomCommand(dto);
        var result = await sender.Send(command);
        return TypedResults.Created();
    }
}