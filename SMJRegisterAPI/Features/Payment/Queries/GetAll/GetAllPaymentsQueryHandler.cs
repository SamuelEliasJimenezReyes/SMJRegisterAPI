using AutoMapper;
using MediatR;
using SMJRegisterAPI.Features.Payment.Dtos;
using SMJRegisterAPI.Features.Payment.Repository;

namespace SMJRegisterAPI.Features.Payment.Queries.GetAll;

public class GetAllPaymentsQueryHandler(
    IPaymentRepository repository, 
    IMapper mapper) : IRequestHandler<GetAllPaymentsQuery, IList<PaymentDto>>
{
    public async Task<IList<PaymentDto>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
    {
        var list = await repository.GetAllAsync();
        var result = new List<PaymentDto>();
        foreach (var paymentDto in list)
        { 
            await repository.LoadReferenceAsync(paymentDto, payment => payment.Camper);
            await repository.LoadReferenceAsync(paymentDto, payment => payment.BanksInformation);
            var dto = mapper.Map<PaymentDto>(paymentDto);
            result.Add(dto);
        }
        return result;
    }
}