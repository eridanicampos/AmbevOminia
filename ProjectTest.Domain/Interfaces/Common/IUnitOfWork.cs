using ProjectTest.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Domain.Interfaces.Common
{
    public interface IUnitOfWork
    {
        void Commit();
        Task CommitAsync();

        IUserRepository UserRepository { get; }
        IAcessoUsuarioRepository AcessoUsuarioRepository { get; }
        IVendaRepository VendaRepository { get; }
        IItemVendaRepository ItemVendaRepository { get; }


    }
}
