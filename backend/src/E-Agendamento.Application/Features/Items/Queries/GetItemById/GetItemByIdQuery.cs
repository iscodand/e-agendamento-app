using E_Agendamento.Application.Parameters;
using E_Agendamento.Application.Wrappers;
using MediatR;

namespace E_Agendamento.Application.Features.Items.Queries.GetItemById
{
    public class GetItemByIdQuery : IRequest<Response<IEnumerable<GetItemByIdViewModel>>>
    {
        public string ItemId { get; set; }
        public string CompanyId { get; set; }
    }

    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, Response<IEnumerable<GetItemByIdViewModel>>>
    {
        public Task<Response<IEnumerable<GetItemByIdViewModel>>> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}