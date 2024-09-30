using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using Transaction.API.Repositories.Interfaces;

namespace Transaction.API.EventBusConsumer
    {
    public class AccountTransactionConsumer(
        ITransactionRepository transactionRepository,
        IMapper mapper)
        : IConsumer<AccountTransactionEvent>
        {
        public async Task Consume(ConsumeContext<AccountTransactionEvent> context)
        {
            var transaction = mapper.Map<Entities.Transaction>(context.Message);
            if (transaction is not null)
                await transactionRepository.Add(transaction);
        }
        }
    }