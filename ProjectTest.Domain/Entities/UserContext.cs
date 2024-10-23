using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Entities
{
    public class UserContext
    {
        public Usuario? User { get; set; }
        public IEnumerable<Claim>? Claims { get; set; }
    }
}
