using E_Agendamento.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Agendamento.Application.Features.Schedules.Commands.CreateSchedule
{
    public class CreateScheduleCommand : IRequest<Response<CreateScheduleCommand>>
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string RequestedBy { get; set; }
        public string ConfirmedBy { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndAt { get; set; }
        public string CompanyId { get; set; }
    }

    public class CreateScheduleCommandHandler : IRequestHandler<CreateScheduleCommand, Response<CreateScheduleCommand>>
    {
        public Task<Response<CreateScheduleCommand>> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
