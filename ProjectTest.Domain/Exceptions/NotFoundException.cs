using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Exceptions
{
    public abstract class NotFoundException : System.ApplicationException
    {
        protected NotFoundException(string message)
            : base("Not Found", new Exception(message))
        {
        }
    }
}
