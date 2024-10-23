using Microsoft.EntityFrameworkCore;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces.Repository;
using ProjectTest.Infrastructure.Data.Context;
using ProjectTest.Infrastructure.Data.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTest.Infrastructure.Data.Repositories
{
    public class AcessoUsuarioRepository : GenericAsyncRepository<AcessoUsuario>, IAcessoUsuarioRepository
    {

        public AcessoUsuarioRepository(ProjectTestContext context)
            : base(context) { }

        public async Task<int> GetAcessoUltimosDias(string userId, int qtdDias)
        {
            var data = DateTime.Now.AddDays(-qtdDias);
            return await DbSet.Where(x => !x.IsDeleted && x.UsuarioId.Equals(userId) && x.CreatedAt >= data).OrderByDescending(x => x.CreatedAt).CountAsync();
        }

    }
}
