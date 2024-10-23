using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Interfaces
{
    public interface ICurrentUserInfo
    {
        string UserId { get; }
        string UserName { get; }
    }
}
