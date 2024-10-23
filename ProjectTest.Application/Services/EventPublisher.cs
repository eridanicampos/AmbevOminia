using Microsoft.Extensions.Logging;
using ProjectTest.Application.Events;
using ProjectTest.Application.Interfaces;
using Serilog;
namespace ProjectTest.Application.Services
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ILogger<EventPublisher> _logger;

        public EventPublisher(ILogger<EventPublisher> logger)
        {
            _logger = logger;
        }


        public void Publish(VendaEvent vendaEvent)
        {
            var logMessage = vendaEvent switch
            {
                CompraCriadaEvent e => $"Evento: CompraCriada | VendaId: {e.VendaId} | Timestamp: {e.Timestamp}",
                CompraAlteradaEvent e => $"Evento: CompraAlterada | VendaId: {e.VendaId} | Timestamp: {e.Timestamp}",
                CompraCanceladaEvent e => $"Evento: CompraCancelada | VendaId: {e.VendaId} | Timestamp: {e.Timestamp}",
                ItemCanceladoEvent e => $"Evento: ItemCancelado | VendaId: {e.VendaId} | ItemId: {e.ItemId} | Timestamp: {e.Timestamp}",
                _ => "Evento desconhecido"
            };

            _logger.LogInformation(logMessage);
        }
    }
}
