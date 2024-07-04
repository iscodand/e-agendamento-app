using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Exceptions;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using E_Agendamento.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Agendamento.Application.Features.Schedules.Commands.CreateSchedule
{
    public class CreateScheduleCommand : IRequest<Response<CreateScheduleCommand>>
    {
        public string ItemId { get; set; }
        public string Observation { get; set; }
        
        [JsonIgnore]
        public string Id { get; set; }
        [JsonIgnore]
        public string Status { get; set; }
        [JsonIgnore]
        public string RequestedBy { get; set; }
        [JsonIgnore]
        public string ConfirmedBy { get; set; }
        [JsonIgnore]
        public DateTime StartedAt { get; set; }
        [JsonIgnore]
        public DateTime EndAt { get; set; }
        [JsonIgnore]
        public string CompanyId { get; set; }

        public static Schedule Map(CreateScheduleCommand command)
        {
            return new Schedule()
            {
                ItemId = command.ItemId,
                Observation = command.Observation,
                RequestedById = command.RequestedBy,
                CompanyId = command.CompanyId
            };
        }
    }

    public class CreateScheduleCommandHandler : IRequestHandler<CreateScheduleCommand, Response<CreateScheduleCommand>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public CreateScheduleCommandHandler(IItemRepository itemRepository, IScheduleRepository scheduleRepository)
        {
            _itemRepository = itemRepository;
            _scheduleRepository = scheduleRepository;
        }

        public async Task<Response<CreateScheduleCommand>> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
        {
            // 1. item precisa estar disponivel para agendamento
            // 2. item precisa ter quantidade disponivel maior que 1
            // 3. usuario so pode agendar aquele item uma vez
            // 4. tipos de itens que so podem ser agendados um por vez
            //     4.1 => adicionar campo para categoria (se for uma sala, usuario nao podera agendar duas salas simultaneamente)
            // 5. 

            Item item = await _itemRepository.GetByIdAsync(request.ItemId);
            if (item is null)
            {
                throw new ApiException("Item não encontrado.");
            }

            if (!item.IsAvailable || item.QuantityAvailable == 0)
            {
                throw new ApiException("Este item não está disponível para agendamento.");
            }

            bool itemAlreadyScheduled = await _scheduleRepository.ItemAlreadyScheduledByUserAsync(request.ItemId, request.RequestedBy);
            if (itemAlreadyScheduled)
            {
                throw new ApiException("Você já possui um agendamento em aberto com esse item.");
            }

            Schedule schedule = CreateScheduleCommand.Map(request);
            schedule = await _scheduleRepository.CreateAsync(schedule);
            request.Id = schedule.Id;

            return new("Item agendado com sucesso. Agora você precisa aguardar uma confirmação de um superior.", request);
        }
    }
}
