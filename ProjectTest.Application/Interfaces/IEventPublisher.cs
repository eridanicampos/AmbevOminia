using ProjectTest.Application.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Application.Interfaces
{
    public interface IEventPublisher
    {
        void Publish(VendaEvent vendaEvent);
    }
}
