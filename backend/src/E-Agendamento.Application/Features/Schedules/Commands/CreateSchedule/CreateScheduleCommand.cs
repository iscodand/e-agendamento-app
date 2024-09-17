using E_Agendamento.Application.Contracts.Repositories;
using E_Agendamento.Application.Exceptions;
using E_Agendamento.Application.Features.Schedules.Queries.Common;
using E_Agendamento.Application.Wrappers;
using E_Agendamento.Domain.Entities;
using MediatR;
using System.Text.Json.Serialization;

namespace E_Agendamento.Application.Features.Schedules.Commands.CreateSchedule
{
    public class CreateScheduleCommand : IRequest<Response<ScheduleViewModel>>
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

        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }

        [JsonIgnore]
        public string CompanyId { get; set; }

        public static Schedule Map(CreateScheduleCommand command)
        {
            return new Schedule()
            {
                StartAt = command.StartAt,
                EndAt = command.EndAt,
                ItemId = command.ItemId,
                Observation = command.Observation,
                RequestedById = command.RequestedBy,
                CompanyId = command.CompanyId
            };
        }
    }

    public class CreateScheduleCommandHandler : IRequestHandler<CreateScheduleCommand, Response<ScheduleViewModel>>
    {
        private readonly IItemRepository _itemRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IUserRepository _userRepository;

        public CreateScheduleCommandHandler(IItemRepository itemRepository, IScheduleRepository scheduleRepository, IUserRepository userRepository)
        {
            _itemRepository = itemRepository;
            _scheduleRepository = scheduleRepository;
            _userRepository = userRepository;
        }

        public async Task<Response<ScheduleViewModel>> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
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
                throw new ValidationException([new("ItemId", "Este item não está cadastrado. Verifique e tente novamente.")]);
            }

            ApplicationUser user = await _userRepository.GetByIdAsync(request.RequestedBy);

            if (!item.IsAvailable || item.QuantityAvailable == 0)
            {
                throw new ValidationException([new("ItemId", "Este item não está disponível para agendamento.")]);
            }

            bool itemAlreadyScheduled = await _scheduleRepository.ItemAlreadyScheduledByUserAsync(request.ItemId, request.RequestedBy);
            if (itemAlreadyScheduled)
            {
                throw new ValidationException([new("ItemId", "Você já possui um agendamento em aberto com esse item.")]);
            }

            Schedule schedule = CreateScheduleCommand.Map(request);
            schedule = await _scheduleRepository.CreateAsync(schedule);
            request.Id = schedule.Id;


            // Diminuindo a quantidade disponível do item em 1
            item.QuantityAvailable -= 1;
            await _itemRepository.UpdateAsync(item);

            schedule.Item = item;
            schedule.RequestedBy = user;

            ScheduleViewModel response = ScheduleViewModel.Map(schedule);

            return new("Item agendado com sucesso. Agora você precisa aguardar uma confirmação de um superior.", response);
        }
    }
}
