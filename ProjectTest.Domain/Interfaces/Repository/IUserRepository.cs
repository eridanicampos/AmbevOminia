using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Interfaces.Repository
{
    public interface IUserRepository : IGenericAsyncRepository<Usuario>
    {
        Task<Usuario> FindUser(string email);
    }
}
