using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Exceptions
{
    public abstract class BadRequestException : System.ApplicationException
    {
        protected BadRequestException(string message)
            : base("Bad Request", new Exception(message))
        {
        }
    }
}
