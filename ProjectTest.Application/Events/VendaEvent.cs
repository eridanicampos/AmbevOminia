using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Events
{
    public abstract class VendaEvent
    {
        public Guid VendaId { get; }
        public DateTime Timestamp { get; }

        protected VendaEvent(Guid vendaId)
        {
            VendaId = vendaId;
            Timestamp = DateTime.UtcNow;
        }
    }

    public class CompraCriadaEvent : VendaEvent
    {
        public CompraCriadaEvent(Guid vendaId) : base(vendaId) { }
    }

    public class CompraAlteradaEvent : VendaEvent
    {
        public CompraAlteradaEvent(Guid vendaId) : base(vendaId) { }
    }

    public class CompraCanceladaEvent : VendaEvent
    {
        public CompraCanceladaEvent(Guid vendaId) : base(vendaId) { }
    }

    public class ItemCanceladoEvent : VendaEvent
    {
        public Guid ItemId { get; }

        public ItemCanceladoEvent(Guid vendaId, Guid itemId) : base(vendaId)
        {
            ItemId = itemId;
        }
    }
}
